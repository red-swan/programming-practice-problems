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
    let tryOfSet s = 
        match Set.count s with
        | 0 -> None
        | 1 -> Some(Fixed (Seq.head s))
        | _ -> Some(Possible(s))


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
        printfn "\n%s" output

    let exclusivePossibilities (r : Row) = 
        r
        |> Seq.zip [1 .. 9]
        |> Seq.filter (snd >> Cell.isPossible)
        |> Seq.fold (fun acc (i, Possible xs) -> 
            Seq.fold (fun m x -> 
                Map.addDefault [] List.append x [i] m
            
            ) acc xs
        
        ) Map.empty
        |> Map.filter (fun k v -> List.length v < 4)
        |> Map.fold (fun m x is -> Map.addDefault [] List.append is [x] m) Map.empty
        |> Map.filter (fun k v -> List.length k = List.length v)
        |> Map.fold (fun acc k v -> List.append [List.rev v] acc ) []
        // |> Map.fold (fun acc _ v -> List.append (List.rev v) acc) []


    let _pruneCellsByFixed (cells : Row) : Row option = 

        let fixeds = Seq.fold (fun s x -> match x with | Fixed(x) -> Set.add x s | _ -> s ) Set.empty cells

        cells
        |> Seq.map ( function | Possible xs -> Cell.tryOfSet (Set.difference xs fixeds) | x -> Some x)
        |> Seq.fold (fun (acc : Cell seq option) (x : Cell option) -> 
                match acc,x with
                | Some acc, Some x -> Some(Seq.append acc [x])
                | _,_ -> None
            ) (Some(Seq.empty))
        |> Option.map Seq.toList
    let rec pruneCellsByFixed cells =
        match _pruneCellsByFixed cells with
        | Some cells' when cells = cells' -> Some cells'
        | Some cells' -> pruneCellsByFixed cells'
        | None -> None

    let _pruneCellsByExclusives (cells : Row) =
        
        match exclusivePossibilities cells with
        | [] -> Some cells
        | exclusives ->
            let allExclusives = exclusives |> Seq.collect id |> Set.ofSeq
            cells 
            |> Seq.map (fun c -> 
                match c with 
                | Fixed _ -> Some c
                | Possible xs -> 
                    let intersection = Set.intersect xs allExclusives
                    if List.contains (Set.toList intersection) exclusives then
                        Cell.tryOfSet intersection
                    else
                        Some c
                    )
            |> Seq.fold (fun (acc : Cell seq option) (x : Cell option) -> 
                match acc,x with
                | Some acc, Some x -> Some(Seq.append acc [x])
                | _,_ -> None
            ) (Some(Seq.empty))
            |> Option.map Seq.toList
    let rec pruneCellsByExclusives cells = 
        match _pruneCellsByExclusives cells with
        | Some cells' when cells' = cells -> Some cells'
        | Some cells' -> pruneCellsByExclusives cells'
        | None -> None
        
        
    let _pruneCells cells = 
        maybe {
            let! a = pruneCellsByFixed cells
            let! b = pruneCellsByExclusives a
            return b
        }

    let rec pruneCells (cells : Row) : Row option =
        match _pruneCells cells with
        | Some cells' when cells' = cells -> Some cells'
        | Some cells' -> pruneCells cells'
        | None -> None
        
        
    let subGridToRows (cells : Grid) : Grid = 
        cells
        |> List.chunkBySize 3
        |> List.collect (fun [r1;r2;r3] -> List.zip3 r1 r2 r3 |> List.chunkBySize 3)
        |> List.map (List.unzip3 >> (fun (r1,r2,r3) -> List.concat [r1;r2;r3]))



        
        


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
            
        loop (prune g)
        |> Seq.head

    let solveFromString = fromString >> solve



let samples = 
    "in-depth/sudoku/sudoku17.txt"
    |> System.IO.File.ReadAllLines
    |> Seq.take 1000    

// 1000 boards
// 17.633 seconds
// 0.017633 seconds per board
fsi.ShowDeclarationValues <- false
let answers = 
    samples 
    |> Seq.map Grid.solveFromString
    |> Seq.toList
