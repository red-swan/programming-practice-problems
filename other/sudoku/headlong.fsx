
#time
open System.Collections


type Grid = int [,]
    
module Grid = 
    let readCell (c : char) = 
        match int c with
        | 46 -> 0
        | x when 49 <= x && x <= 57 -> x - 48
        | _ -> failwith "Input not valid in sudoku"

    let fromString s : Grid = 
        if String.length s <> 81 then
            failwith "Not valid input"
        else
            s
            |> Seq.chunkBySize 9
            |> Seq.map (Seq.map readCell)
            |> array2D
    let toString g = 
        let cellToChar c = 
            match c with
            | 0 -> '.'
            | x -> char (48 + x)

        seq {
            for r in 1 .. Array2D.length1 g do
            yield seq {
                for c in 1 .. Array2D.length2 g do
                yield cellToChar g.[r-1,c-1]    
            }
            
        }
        |> Seq.map System.String.Concat
        |> String.concat "\n"
        
        
    let print g = 
        printfn "%s" (toString g)


    let getBoxIdx r c = 3 * (r / 3) + (c / 3)
    let isValid (b : BitArray) (r : BitArray) (c : BitArray) num = 
        not(b.[num - 1] || r.[num - 1] || c.[num - 1])

    let nextStepCoordFromGrid g r c = 
        match r,c with
        | _,_ when c = Array2D.length2 g - 1  -> (r+1),0
        | _ -> r,(c+1)

    let rec backtrack g (boxes : BitArray []) (rows : BitArray []) (cols : BitArray []) r c : bool = 
        if Array2D.length1 g <= r then true else
        match g.[r,c] with
        | 0 -> 
            loop g boxes rows cols r c [1 .. 9]
        | _ -> 
            let newR, newC = nextStepCoordFromGrid g r c
            backtrack g boxes rows cols newR newC
        
    and loop g boxes rows cols r c candidates = 
        match candidates with
        | [] -> false
        | num :: nums ->
            g.[r,c] <- num
            if isValid boxes.[getBoxIdx r c] rows.[r] cols.[c] num then
                boxes.[getBoxIdx r c].[num - 1] <- true
                rows.[r].[num - 1] <- true
                cols.[c].[num - 1] <- true
                let newR, newC = nextStepCoordFromGrid g r c
                if backtrack g boxes rows cols newR newC then 
                    true
                else
                    boxes.[getBoxIdx r c].[num - 1] <- false
                    rows.[r].[num - 1] <- false
                    cols.[c].[num - 1] <- false
                    g.[r,c] <- 0
                    loop g boxes rows cols r c nums    
            else
                g.[r,c] <- 0
                loop g boxes rows cols r c nums

        
        

    let createBitArraysFromGrid g = 
        let n = Array2D.length1 g
        let boxSets = Array.init n (fun _ -> BitArray(9,false))
        let rowSets = Array.init n (fun _ -> BitArray(9,false))
        let colSets = Array.init n (fun _ -> BitArray(9,false))

        do 
            for r in 0 .. n-1 do
                for c in 0 .. n-1 do
                    let num = g.[r,c]
                    if num <> 0 then do
                        // printfn "Adding %i to (%i,%i)" num r c
                        let boxId = getBoxIdx r c
                        boxSets.[boxId].[num - 1] <- true
                        rowSets.[r].[num - 1] <- true
                        colSets.[c].[num - 1] <- true
        
        boxSets,rowSets,colSets

    let solve gridStr =
        let g = fromString gridStr
        let boxSets,rowSets,colSets = createBitArraysFromGrid g
        backtrack g boxSets rowSets colSets 0 0 |> ignore
        g


        
        



let s1 = "53..7....6..195....98....6.8...6...34..8.3..17...2...6.6....28....419..5....8..79"
let s2 = "..9748...7.........2.1.9.....7...24..64.1.59..98...3.....8.3.2.........6...2759.."
let s3 = ".......1.4.........2...........5.4.7..8...3....1.9....3..4..2...5.1........8.6..."
let s4 = ".6....3..4..7............8......8.125..6............5..82...7.....5..6......1...."


Grid.solve s1 // 0.011 seconds
Grid.solve s2 // 0.018 seconds
Grid.solve s3 // 7.635 seconds
Grid.solve s4

let samples = 
    "in-depth/sudoku/sudoku17.txt"
    |> System.IO.File.ReadLines
    |> Seq.take 10
fsi.ShowDeclarationValues <- false

// 37.75 seconds
// 10 puzzles
// 3.775 seconds per puzzle
let answers =
    samples
    |> Seq.map Grid.solve
    |> Seq.toList



