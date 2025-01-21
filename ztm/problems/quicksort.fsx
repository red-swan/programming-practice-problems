

let rec quicksort xs = 
    match xs with
    | [] -> []
    | [a] -> [a]
    | x::xs ->
        let lower,higher = List.partition (fun arg -> arg < x) xs
        (quicksort lower) @ [x] @ (quicksort higher)


quicksort [6;4;2;3;5;1]













