(*
The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.

Find the sum of all the primes below two million.
*)
#load "Tools.fs"
open System
open Tools

let answer = 
    sieveOfAtkin 2000000
    |> List.sumBy Numerics.BigInteger
