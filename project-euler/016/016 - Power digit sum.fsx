(*
2^15 = 32768 and the sum of its digits is 3 + 2 + 7 + 6 + 8 = 26.

What is the sum of the digits of the number 2^1000?
*)


// #load "../fsharp-lib/"
#r "nuget: MathNet.Numerics, 5.0.0"
#r "nuget: BigRational, 1.0.0.7"
// !todo: update this to F# 9.0

open System
open MathNet.Numerics
open BigRational

let answer = 
    BigRational.Pow (2N,1000)
    |> Convert.ToString
    |> (fun x -> x.ToCharArray())
    |> Array.sumBy (fun x -> x |> string |> Int32.Parse)
