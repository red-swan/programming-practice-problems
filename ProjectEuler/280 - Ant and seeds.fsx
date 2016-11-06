(*
A laborious ant walks randomly on a 5x5 grid. The walk starts from the central square. 
At each step, the ant moves to an adjacent square at random, without leaving the grid; 
thus there are 2, 3 or 4 possible moves at each step depending on the ant's position.

At the start of the walk, a seed is placed on each square of the lower row. When the ant 
isn't carrying a seed and reaches a square of the lower row containing a seed, it will 
start to carry the seed. The ant will drop the seed on the first empty square of the upper 
row it eventually reaches.

What's the expected number of steps until all seeds have been dropped in the top row? 
Give your answer rounded to 6 decimal places.
*)

open System.Text
//fsi.ShowDeclarationValues <- false


// creating the gamestate type -------------------------------------------------
[<CustomEquality;NoComparison>]
type GameState = 
    {
    board : sbyte Set
    ant   : sbyte
    laden : bool
    }
    override this.GetHashCode() = 
        hash this.board + (int this.ant)
    override this.Equals(obj) = 
        match obj with 
            | :? GameState as that -> 
                    (this.board,this.ant,this.laden) = (that.board, that.ant, that.laden)
            | _ -> false
    override this.ToString() = 
        let output = StringBuilder("\n")
        let mutable temp = 0y
        for y in 0y .. 4y do
            for x in 0y .. 4y do
                temp <- y*5y+x
                if temp = this.ant
                then if this.laden then output.Append('8')
                        else output.Append('x')
                elif (Set.contains temp this.board)
                    then output.Append('o')
                else output.Append('-')
                |> ignore
            output.Append('\n')
            |> ignore
        output.ToString()

// this function returns true if the seed positions are all in the top row
let isStateWon state = 
    ((Seq.sum state.board) = 10y) && (Set.contains 0y state.board)

// create a GameState record from a list of positions
let createState seedList ant laden = 
    {board = Set.ofSeq seedList; ant = ant; laden = laden}

// create our initial game state
let initialState = createState [20y .. 24y] 12y false

// returns the adjacent squares' indices for an ant in a GameState
let nextMoves state = 
    match (isStateWon state) with
    | true -> [|state.ant|]
    | false -> 
        let horizontalMoves = 
            match state.ant with
            | position when position % 5y = 0y -> [position+1y]
            | position when (position+1y) % 5y = 0y -> [position-1y]
            | _ -> [state.ant+1y;state.ant-1y]
        let verticalMoves = 
            match state.ant with
            | position when position / 5y = 0y -> [position+5y]
            | position when position / 5y = 4y -> [position-5y]
            | _ -> [state.ant-5y;state.ant+5y]
        List.toArray (horizontalMoves @ verticalMoves)

// returns the GameState after resolving the ant's move to the specified newPosition
let moveAnt (state:GameState) newPosition = 
    match state.laden,newPosition,(Set.contains newPosition state.board) with 
    // ant moves to empty ending square and is laden
    | true, idx, false when idx < 5y -> {board = (Set.add newPosition state.board); 
                                         ant = newPosition; 
                                         laden = false}
    // ant moves to filled beginning square and is not laden
    | false,idx,true when idx >= 20y -> {board = Set.remove newPosition state.board;
                                        ant = newPosition;
                                        laden = true}
    // otherwise
    | _ -> {board = state.board; ant = newPosition; laden = state.laden} 

// create an array of new states after moving the ant to its adjacent squares
let nextStates state = 
    state
    |> nextMoves
    |> Array.Parallel.map (fun newPosition -> moveAnt state newPosition)

// map an array of states and their probabilities to it's adjacent states, diffusing the
// probability over those states
let nextStep (input:(GameState*decimal) []) = 
    input
    |> Array.Parallel.collect (fun (state,prob) -> 
                    let newStates = nextStates state
                    let newProb = prob / (newStates |> Array.length |> decimal)
                    Array.Parallel.map (fun x -> (x,newProb)) newStates )

// loop for iterating through states -------------------------------------------

let euler280 requiredIterations = 
    // the looping function
    let rec findAllSteps (iteration:int) (stepState:(GameState*decimal) []) expectedValue = 
        match iteration with
        | step when step = requiredIterations -> expectedValue
        | _ -> 
            // we're going to split the states into won and notWon states
            // the notWon states will go on into the next step
            let wonStates,nextStepState = 
                stepState
                |> nextStep
                // group all states together
                |> Seq.groupBy fst
                |> Seq.toArray
                // sum the probabilities for each group of states
                |> Array.Parallel.map (fun (keyState,valueStates) -> 
                                      (keyState, Seq.sumBy (fun (s,prob) -> prob) valueStates))
                // splitting the states by winner or not
                |> Array.Parallel.partition (fun (s,p) -> isStateWon s)
            let i = iteration |> decimal
            // sum the winners' probabilities together for this step
            let probOfWin = wonStates |> Array.sumBy snd
            // multiply probability by value and add it to the expectation thus far
            let updatedExpectation = expectedValue + (i * probOfWin)
            printfn "Step: %i Expected Value: %.9f" iteration updatedExpectation
            // iterate onto the next step
            findAllSteps (iteration + 1) nextStepState updatedExpectation

    // calling the looping function on the initial state
    findAllSteps 1 [|(initialState,1.0M)|] 0.0M

// find answer -----------------------------------------------------------------
let stopWatch = System.Diagnostics.Stopwatch.StartNew() 

let answer = 
    euler280 2500    

stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMinutes // 5.779914 Minutes




// brute force simulation ------------------------------------------------------
// This does not arrive at a perfect solution but does get close to the correct
// solution rather quickly. It is a quick and easy way to check if you're on the
// right track. It is left here only to make that point.

open System
let rng = Random()

let runSimulation() = 
    let mutable state = initialState
    let mutable acc = 0
    while (not (isStateWon state)) do
        let nextMoves = nextMoves state
        let nextMovesLength = Array.length nextMoves
        let nextMove = Seq.item (rng.Next(nextMovesLength)) nextMoves
        state <- (moveAnt state nextMove)
        acc <- acc + 1
    float (acc + 1)

let iterations = 1e6
let mutable average = 0.0
for i in 1.0 .. iterations do
    if i % 10000.0 = 0.0 then printfn "%6f" average
    // recursive average function
    average <- ((i-1.0)*(average)/(i)) + (runSimulation())/i



