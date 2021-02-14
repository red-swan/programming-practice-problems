
#time
open System.Collections


module BitArray = 
    let fold f state (a : BitArray) = 
        Seq.fold (fun s i -> f s a.[i]) state [0 .. a.Length - 1]
    let scan f state (a : BitArray) = 
        Seq.scan (fun s i -> f s a.[i]) state [0 .. a.Length - 1]
    let lmap f (a : BitArray) = 
        [for i in 1 .. a.Length -> f a.[i-1]]
    let lmap2 f l (a : BitArray) =
        List.map2 f l (lmap id a)

type Cell = 
    | Fixed of int
    | Possible of BitArray
type Row = Cell list
type Grid = Row list
    
module Grid = 
    let readCell (c : char) = 
        match int c with
        | 46 -> Possible(BitArray(9,true))
        | x when 49 <= x && x <= 57 -> Fixed(x - 48)
        | _ -> failwith "Input not valid in sudoku"

    let fromString s : Grid = 
        if String.length s <> 81 then
            failwith "Not valid input"
        else
            s
            |> Seq.chunkBySize 9
            |> Seq.map (Seq.map readCell >> Seq.toList)
            |> Seq.toList
    let toString g = 
        let cellToChar c = 
            match c with
            | Fixed x -> char (48 + x)
            | _ -> '.'
        g
        |> List.map (List.map cellToChar >> System.String.Concat)
        |> String.concat "\n"
    let print g = 
        printfn "%s" (toString g)
    
    let printWithPossibilities (g : Grid) = 
        let toList = function 
            | Fixed x -> (string x) + "          "
            | Possible arr -> 
                let nums = 
                    BitArray.lmap2 (fun s bit -> if bit then string s else " ") [1 .. 9] arr
                    |> System.String.Concat
                "[" + nums + "]"
        let output = 
            g
            |> List.map (List.map toList >> String.concat " ")
            |> String.concat "\n"
        "\n" + output
    let pruneCells (r : Row) : Row = 
        let fixeds = BitArray(9,true)
        
        do
            for c in r do
                match c with 
                | Fixed x -> fixeds.[x-1] <- false
                | _ -> ()
        
        for c in r do
            match c with
            | Possible arr -> arr.And(fixeds) |> ignore
            | _ -> ()

        r
    



let sample1Str = "53..7....6..195....98....6.8...6...34..8.3..17...2...6.6....28....419..5....8..79"
let sample2Str = "..9748...7.........2.1.9.....7...24..64.1.59..98...3.....8.3.2.........6...2759.."
let sample3Str = ".......1.4.........2...........5.4.7..8...3....1.9....3..4..2...5.1........8.6..."

let sample1Grid = Grid.fromString sample1Str
Grid.print sample1Grid
Grid.printWithPossibilities sample1Grid








