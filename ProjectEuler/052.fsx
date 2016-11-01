(*It can be seen that the number, 125874, and its double, 251748, contain exactly the same digits, but in a different order.

Find the smallest positive integer, x, such that 2x, 3x, 4x, 5x, and 6x, contain the same digits.*)

let rec digits number = 
    let rec loop = function
    | 0 -> []
    | x -> (x % 10)::(loop (x/10))
    loop number |> List.sort

let answer = 
    Seq.unfold (fun x-> Some(x,x+1)) 1
    |> Seq.find (fun x -> List.map (fun y-> digits (y*x)= digits x) [1 .. 6] |> List.reduce (&&) )
