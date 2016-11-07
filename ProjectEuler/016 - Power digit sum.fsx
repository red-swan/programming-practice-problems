(*
2^15 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26.

What is the sum of the digits of the number 2^1000?
*)


#load "Tools.fsx"


open Tools
open System
open MathNet.Numerics

let answer = 
    BigRational.Pow (2N,1000)
    |> Convert.ToString
    |> (fun x -> x.ToCharArray())
    |> Array.sumBy (fun x -> x |> string |> Int32.Parse)
