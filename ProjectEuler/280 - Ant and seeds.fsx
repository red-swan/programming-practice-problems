#r @"C:\Users\JDKS\TEMP\Euler280\packages\MathNet.Numerics.3.13.1\lib\net40\MathNet.Numerics.dll"
#r @"C:\Users\JDKS\TEMP\Euler280\packages\MathNet.Numerics.FSharp.3.13.1\lib\net40\MathNet.Numerics.FSharp.dll"
#r @"C:\Users\JDKS\FSharp.Charting.0.90.14\lib\net40\FSharp.Charting.dll"

open System
open System.Text
open System.Collections
open System.Collections.Concurrent
open System.Collections.Generic
open MathNet
open MathNet.Numerics
open FSharp.Charting

fsi.ShowDeclarationValues <- false
// creating the gamestate type ----------------------------------------------------------
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
//        Console.WriteLine(this.GetHashCode())
//        Console.WriteLine(output)
        output.ToString()

let isStateWon state = 
    ((Seq.sum state.board) = 10y) && (Set.contains 0y state.board)

let createState seedList ant laden = 
    {board = Set.ofSeq seedList; ant = ant; laden = laden}

let initialState = createState [20y .. 24y] 12y false

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

let nextStates state = 
    state
    |> nextMoves
    |> Array.Parallel.map (fun newPosition -> moveAnt state newPosition)

let nextStep (input:(GameState*decimal) []) = 
    input
    |> Array.Parallel.collect (fun (state,prob) -> 
                    let newStates = nextStates state
                    let newProb = prob / (newStates |> Array.length |> decimal)
                    Array.Parallel.map (fun x -> (x,newProb)) newStates )

// loop for iterating through states --------------------------------------------------------------------
let finalStep = 2500

let rec findAllSteps (iteration:int) (stepState:(GameState*decimal) []) expectedValue = 
    match iteration with
    | step when step = finalStep -> expectedValue
    | _ -> 
        let wonStates,nextStepState = 
            stepState
            |> nextStep
            |> Seq.groupBy fst
            |> Seq.toArray
            |> Array.Parallel.map (fun (keyState,valueStates) -> 
                                  (keyState, Seq.sumBy (fun (s,prob) -> prob) valueStates))
            |> Array.Parallel.partition (fun (s,p) -> isStateWon s)
        let i = iteration |> decimal
        let probOfWin = wonStates |> Array.sumBy snd
        let updatedExpectation = expectedValue + (i * probOfWin)
        printfn "Step: %i Expected Value: %.9f" iteration updatedExpectation
        findAllSteps (iteration + 1) nextStepState updatedExpectation

// find answer ----------------------------------------------------------------------------------------------
let stopWatch = System.Diagnostics.Stopwatch.StartNew() 

let euler280 = 
    findAllSteps 1 [|(initialState,1.0M)|] 0.0M

stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMinutes // 7.057 seconds






// brute force simulation ------------------------------------------------------------------
let ITERATIONS = 47
let rng = Random()

let runSimulation() = 
    let mutable state = initialState
    let mutable acc = 0
    while (not (isStateWon state)) do
//        Console.WriteLine(state.ToString())
        let nextMoves = nextMoves state
        let nextMovesLength = Array.length nextMoves
        let nextMove = Seq.nth (rng.Next(nextMovesLength)) nextMoves
//        printfn "%A" nextMove
        state <- (moveAnt state nextMove)
        acc <- acc + 1
    float (acc + 1)

let mutable average = 0.0
let simulations = Array.create (int 1e6) 0.0
for i in 1.0 .. 1e6 do
//    if i % 10000.0 = 0.0 then printfn "%6f" (simulations |> Seq.take (int i) |>Seq.average)
//    simulations.[int i - 1] <- runSimulation()
    if i % 10000.0 = 0.0 then printfn "%6f" average
    average <- ((i-1.0)*(average)/(i)) + (runSimulation())/i


let mutable i = 0
while i < 20 do
    printfn "%A" i
    i <- i + 1


