(*
145 is a curious number, as 1! + 4! + 5! = 1 + 24 + 120 = 145.

Find the sum of all numbers which are equal to the sum of the factorial of their digits.

Note: as 1! = 1 and 2! = 2 are not sums they are not included.
*)


let intFact x = 
    match x with
    | 0 | 1 -> 1
    | _     -> List.reduce (fun a b -> a*b) [1 .. x]


let getDigits number = 
    number.ToString().ToCharArray()
    |> Array.map string
    |> Array.map (fun x -> System.Int32.Parse(x))

let isCurious number =
    number 
    |> getDigits
    |> Array.map intFact
    |> Array.sum
    |> ((=) number)

let answer = 
    [10 .. 9999999]
    |> List.filter isCurious
    |> List.sum