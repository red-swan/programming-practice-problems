(*
A googol (10100) is a massive number: one followed by one-hundred zeros; 
100100 is almost unimaginably large: one followed by two-hundred zeros. 
Despite their size, the sum of the digits in each number is only 1.

Considering natural numbers of the form, ab, where a, b < 100, what is the 
maximum digital sum?
*)

open System
open System.Numerics



let rec getDigits' (outList:int list) (bigInt:BigInteger) = 
    match bigInt with
    | number when number = 0I -> outList
    | number -> getDigits' (int (bigInt % 10I) :: outList) (bigInt / 10I)
let getDigits number = getDigits' [] number
    
let FindDigitalSum (a:int) (b:int) = 
    (pown (BigInteger(a)) b)
    |> getDigits
    |> List.sum

let answer = 
    seq { for a in [1 .. 100] do
          for b in [1 .. 100] do
          yield FindDigitalSum a b}
    |> Seq.max
