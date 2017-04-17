(*
The cube, 41063625 (345^3), can be permuted to produce two other cubes: 
56623104 (384^3) and 66430125 (405^3). In fact, 41063625 is the smallest 
cube which has exactly three permutations of its digits which are also cube.

Find the smallest cube for which exactly five permutations of its digits are cube.
*)




let searchSpace = [1.0 .. 10000.0]

let rec getDigits' (outList:int list) (number:float) = 
    match number with
    | number when number < 1.0 -> outList
    | number -> getDigits' (int (number % 10.0) :: outList) (number / 10.0)
let getDigits number = getDigits' [] number
let getDigitSet number = number |> getDigits |> List.sort

#time
let answer = 
    searchSpace
    |> Seq.map (fun x -> x*x*x)
    |> Seq.groupBy getDigitSet
//    |> Seq.toList
    |> Seq.find (fun (intList, numbers) -> (Seq.length numbers) = 5 )
    |> snd
    |> Seq.sort
    |> Seq.item 0
    |> uint64
#time // 0.044 sec

