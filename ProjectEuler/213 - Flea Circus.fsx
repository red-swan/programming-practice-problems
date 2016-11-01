#I @"C:\Users\JDKS\Desktop\ProjectEuler\packages\MathNet.Numerics.FSharp.3.13.1"

#load "Tools.fs"
#load "MathNet.Numerics.fsx"

open Tools
open MathNet.Numerics
open MathNet.Numerics.Random

// SIMILAR TO 280
let adjacentBoardPositions height width idx= 
    let horizontalMoves = 
        match idx with
        | position when position % width = 0 -> [position+1]
        | position when (position+1) % width = 0 -> [position-1]
        | _ -> [idx+1;idx-1]
    let verticalMoves = 
        match idx with
        | position when position / height = 0 -> [position+height]
        | position when position / height = (height-1) -> [position-height]
        | _ -> [idx-height;idx+height]
    List.toArray (horizontalMoves @ verticalMoves)

let rng = MersenneTwister(1337)

let chooseRandomAdjacentPosition height width idx = 
    let nextMoves = adjacentBoardPositions height width idx
    let numberOfMoves = Array.length nextMoves
    Array.get nextMoves (rng.Next(numberOfMoves)) 

// we're going to push a count to the 

let nextStates (sqBoardLength:int) (stateArray:(int*decimal) []) = 
    stateArray
    |> Array.Parallel.collect 
        (fun (position,prob) -> 
        let newStates = adjacentBoardPositions sqBoardLength sqBoardLength position
        let newProb = prob / decimal(newStates |> Array.length)
        Array.Parallel.map (fun x -> (x,newProb)) newStates)
    |> Array.groupBy fst
    |> Array.Parallel.map (fun (value,fleas) -> (value,Array.sumBy snd fleas))




let step50 boardSize position = 
    let initialState = [|(position,1.0M)|]
    let rec loop step stateSpace =
        match step with
        | 50 -> stateSpace
        | _ ->
            let newStateSpace = nextStates boardSize stateSpace
            loop (step+1) newStateSpace
    loop 0 initialState

let probabilityDistribution boardSize = 

//    let upperLeftCorner = 
//        (Array.init (boardSize/2) (fun x -> (x+0)*boardSize))
//        |> Array.collect (fun x -> Array.map (fun y -> y + x) [|0 .. 14|])

//    upperLeftCorner
    [|0 .. (boardSize*boardSize)-1|]
    |> Array.collect (fun initPos -> (step50 boardSize initPos))


let probOfNotFilling probArray = 
    probArray
    |> Array.Parallel.map (fun prob -> 1.0M-prob)
    |> Array.fold (fun acc elem -> acc * elem) 1.0M

// probability of no one being in a square is 
// (one minus the prob of 1st being there) times 
// (one minus the prob of the second being there) etc. etc.
let stopWatch = System.Diagnostics.Stopwatch.StartNew()
//...


let test = 
    30
    |> probabilityDistribution
    |> Array.groupBy fst
    |> Array.Parallel.map (fun (pos,valueArray) -> pos,(Array.Parallel.map snd valueArray))
    |> Array.Parallel.map (fun (pos, probs) -> (pos, probOfNotFilling probs))
    |> Array.sumBy snd

stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalSeconds

//Does this match the simulation??
