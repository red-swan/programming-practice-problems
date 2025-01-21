

(*
Improvements:
    - 2D Array instead of list of lists?
    - Passing state around instead of calculating diagonals each time
        - CE??

*)

type Square = 
    | Placed
    | Unplaced

type Row = Square list
type Board = Row list

module List = 
    let rec transpose = function
        | (_::_)::_ as M -> 
            List.map List.head M :: transpose (List.map List.tail M)
        | _ -> []
    let replace i v l =
        let rec loop acc step xs = 
            match step,xs with
            | _    , [] -> List.rev acc
            | step , x::xs when step = i -> loop (v :: acc) (step + 1) xs
            | _    , x::xs -> loop (x::acc) (step+1) xs
        loop [] 0 l

let createRow n i : Row = 
    [ for j in 1 .. n -> if j = i+1 then Placed else Unplaced]    

let createBoard n i : Board = 
    [for j in 0 .. n-1 -> if j = 0 then createRow n i else createRow n (n+1)]

let createBoards n : Board list =
    [
        for i in 0 .. n-1 ->
            createBoard n i
    ]

let oneQueenInRow row : bool = 
    let queenCount = Seq.sumBy (function | Placed -> 1 | Unplaced -> 0) row
    queenCount <= 1

let generateCoords r c dr dc n = 
    let rec loop r c = 
        seq { 
            yield (r,c)
            if 0 <= r + dr && r + dr < n && 0 <= c + dc && c + dc < n then
                yield! loop (r + dr) (c + dc)
        }
    loop r c
let _genForwardDiags n (r, c) = generateCoords r c -1 1 n
let genForwardDiags n = 
    seq {
        for r in [ 0 .. n-1 ] do
            yield (r,0)
        for c in [ 1 .. n-1 ] do
            yield (n-1,c)
    }
    |> Seq.map (_genForwardDiags n)
let _genBackwardDiags n (r, c) = generateCoords r c 1 1 n
let genBackwardDiags n = 
    seq {
        for r in [0 .. n-1] do
            yield (r,0)
        for c in [1 .. n-1] do
            yield (0,c)
        
    }
    |> Seq.map (_genBackwardDiags n)
let invalidForwardDiags (board : Board) = 
    board 
    |> List.length
    |> genForwardDiags
    |> Seq.map  (Seq.map (fun (r,c) -> board.[r].[c])) 
    |> Seq.exists (oneQueenInRow >> not)
let invalidBackwardDiags (board : Board) = 
    board 
    |> List.length
    |> genBackwardDiags
    |> Seq.map  (Seq.map (fun (r,c) -> board.[r].[c])) 
    |> Seq.exists (oneQueenInRow >> not)

let notValidDiags board = 
    invalidForwardDiags board || invalidBackwardDiags board

let isInvalid board : bool = 
    List.exists (oneQueenInRow >> not)  board || 
    List.exists (oneQueenInRow >> not) (List.transpose board) ||
    notValidDiags board

let isFilled board : bool = 
    let n = List.length board
    let queenCount = board |> Seq.collect id |> Seq.filter (function | Placed -> true | Unplaced -> false) |> Seq.length
    n = queenCount

let nextBoards n board : Board list = 
    let r = List.findIndex (fun l -> l |> List.contains Placed |> not) board
    [ 
        for i in 0 .. n-1 ->
            let newRow = createRow n i
            List.replace r newRow board
    ]



let toStringList board : string list = 
    board 
    |> List.map ((List.map (function | Placed -> "Q" | Unplaced -> ".")) >> String.concat "")

let solve n = 
    let rec loop (board : Board) = 
        seq {
            if isInvalid board then
                ()
            elif isFilled board then
                yield board
            else
                yield! Seq.collect loop (nextBoards n board)
        }
    n
    |> createBoards
    |> Seq.collect loop
    |> Seq.map toStringList
    |> Seq.toList
    



solve 8
