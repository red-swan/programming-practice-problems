
#r "nuget: FSharpPlus"
open FSharpPlus.Control

#time

let constant x _ = x

module List = 
    let rec transpose = function
        | (_::_)::_ as M -> 
            List.map List.head M :: transpose (List.map List.tail M)
        | _ -> []
    let replace i f xs =
        [for (j,x) in Seq.indexed xs -> if i = j then f x else x]

    let hasDuplicates l = 
        ((false, Set.empty), l)
        ||> Seq.scan (fun (b,s) x -> 
            match b,s,x with
            | true,_,_ -> (b,s)
            | _,s,x  -> if Set.contains x s then (true,Set.empty) else (false, Set.add x s)
            
            )
        |> Seq.map fst
        |> Seq.contains true

module Map = 
    let addDefault (d : 'b) f (k : 'a) x (m : Map<'a,'b>) =
        match Map.containsKey k m with
        | true -> 
            let xs = Map.find k m
            Map.add k (f x xs) m
        | false ->
            Map.add k (f x d) m
    let elems m = 
        m
        |> Map.toSeq
        |> Seq.map snd
        |> Seq.toList

type MaybeBuilder() = 
    member this.Bind(x,f) = Option.bind f x
    member this.Return(x) = Some x
    member this.Zero() = None
let maybe = MaybeBuilder()

type Cell = 
    | Fixed of int
    | Possible of Set<int>
module Cell =
    let isFixed = function | Fixed _ -> true | _ -> false
    let isPossible x = x |> isFixed |> not
    let count = function 
        | Fixed _ -> 1
        | Possible s -> Set.count s
    let ofList l = 
        match l with
        | [] -> failwith "Cannot create empty Cell"
        | [x] -> Fixed x
        | _ -> Possible(Set.ofList l)

type Row = Cell list

module Row = 
    let isInvalid r = 
        let possibles, fixeds = List.partition Cell.isPossible r
        List.hasDuplicates fixeds || (List.exists (fun (Possible x) -> Set.count x = 0) possibles)


type Grid = Row list

module Grid = 
    let readCell (c : char) = 
        match int c with
        | 46 -> Possible(Set.ofList [ 1 .. 9] )
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
            | Possible s -> 
                let nums = 
                    [1 .. 9] 
                    |> Seq.map (fun digit -> if Set.contains digit s then string digit else " ")
                    |> System.String.Concat
                "[" + nums + "]"
        let output = 
            g
            |> List.map (List.map toList >> String.concat " ")
            |> String.concat "\n"
        "\n" + output

    let pruneCells (cells : Row) : Row option =
        let pruneCell xs nums = 
            let d = Set.difference xs nums
            match Set.count d with 
            | 0 -> None
            | 1 -> Some(Fixed(Seq.head d))
            | _ -> Some(Possible(d))
        let fixeds = Seq.fold (fun s x -> match x with | Fixed(x) -> Set.add x s | _ -> s ) Set.empty cells
        cells 
        |> Seq.map (function | Possible xs -> pruneCell xs fixeds | x -> Some x)
        |> Seq.fold (fun (acc : Cell seq option) (x : Cell option) -> 
                match acc,x with
                | Some acc, Some x -> Some(Seq.append acc [x])
                | _,_ -> None
            ) (Some(Seq.empty))
        |> Option.map Seq.toList
    let subGridToRows (cells : Grid) : Grid = 
        cells
        |> List.chunkBySize 3
        |> List.collect (fun [r1;r2;r3] -> List.zip3 r1 r2 r3 |> List.chunkBySize 3)
        |> List.map (List.unzip3 >> (fun (r1,r2,r3) -> List.concat [r1;r2;r3]))


    let simplifyRow (r : Row) = 
        r
        |> Seq.zip [1 .. 9]
        |> Seq.filter (snd >> Cell.isPossible)
        |> Seq.fold (fun acc (i, Possible xs) -> 
            Seq.fold (fun m x -> 
                Map.addDefault [] (List.append) x [i] m
            
            ) acc xs
        
        ) Map.empty
        |> Map.filter (fun k v -> List.length v < 4)
        |> Map.fold (fun m x is -> Map.addDefault [] List.append is [x] m) Map.empty
        |> Map.filter (fun k v -> List.length k = List.length v)
        |> Map.elems
        
        


    let pruneGrid (g : Grid) : Grid option = 
        g 
        |> Seq.map pruneCells
        |> Seq.fold (fun acc x ->
                match acc, x with 
                | Some acc, Some x -> Some(Seq.append acc [x])
                | _ -> None
                ) (Some(Seq.empty))
        |> Option.map Seq.toList
    let _prune g = 
        maybe {
            let! a = pruneGrid g
            let! b = a |> List.transpose |> pruneGrid |> Option.map List.transpose
            let! c = b |> subGridToRows |> pruneGrid |> Option.map subGridToRows
            return c
        }
    let rec prune g = 
        let _g = _prune g
        match _g with 
        | Some x when x = g -> Some x
        | Some x -> prune x
        | None -> None

    let nextGrids (g : Grid) : Grid * Grid = 
        let fixCell = function
            | i, Possible s when Set.count s = 2 -> 
                let [x;y] = Set.toList s
                (i, Fixed x, Fixed y)
            | i, Possible s -> 
                let head = Seq.head s
                (i, Fixed head, Possible(Set.remove head s))
            | _ -> failwith "Not possible"
        let replace2D i v l = 
            List.replace (i / 9) (List.replace (i % 9) (constant v)) l


        let i, first, rest = 
            g
            |> Seq.concat
            |> Seq.indexed
            |> Seq.filter (fun (_,c) -> Cell.isPossible c)
            |> Seq.minBy (snd >> Cell.count)
            |> fixCell
        
        (replace2D i first g, replace2D i rest g)
    
    let isFilled g = 
        g 
        |> Seq.concat 
        |> Seq.exists (Cell.isPossible)
        |> not

    let isInvalid g = 
        let f = List.exists Row.isInvalid 
        f g || f <| List.transpose g || f <| subGridToRows g

    let solve g = 
        let rec loop (g : Grid option) = 
            seq {
                match g with 
                | None -> ()
                | Some g ->
                    if isInvalid g then
                        ()
                    elif isFilled g then
                        g
                    else
                        let grid1, grid2 = nextGrids g
                        yield! grid1 |> prune |> loop 
                        yield! grid2 |> prune |> loop
            }
            
        loop (Some g)
        |> Seq.head

    let solveFromString = fromString >> solve




let s0str = "6......1.4.........2...........5.4.7..8...3....1.9....3..4..2...5.1........8.6..."
let s1str = "53..7....6..195....98....6.8...6...34..8.3..17...2...6.6....28....419..5....8..79"
let s2str = "..9748...7.........2.1.9.....7...24..64.1.59..98...3.....8.3.2.........6...2759.."
let s3str = ".......1.4.........2...........5.4.7..8...3....1.9....3..4..2...5.1........8.6..."

let board = s0str
let grid = Grid.fromString board
Grid.print grid
Grid.printWithPossibilities grid


let answer = Grid.solve grid 
Grid.print answer


let samples = 
    "in-depth/sudoku/sudoku17.txt"
    |> System.IO.File.ReadAllLines
    |> Seq.take 10

// 10 answers
// 46.505 seconds
// 4.65 seconds per answer
let answers = 
    samples 
    |> Seq.map (Grid.fromString >> Grid.solve)
    |> Seq.toList

for i in answers do printfn "%s\n\n" (Grid.toString i)


Grid.solveFromString s3str |> Grid.print



let sRow = [Cell.ofList [4;6;9]; Fixed 1; Fixed 5; Cell.ofList [6;9]; Fixed 7; Cell.ofList [2;3;6;8;9]; Cell.ofList [6;9]; Cell.ofList [2;3;6;8;9]; Cell.ofList [2;3;6;8;9]]
Grid.simplifyRow sRow

