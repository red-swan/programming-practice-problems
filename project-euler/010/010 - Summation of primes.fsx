(*
The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.

Find the sum of all the primes below two million.
*)
#load "../fsharp-lib/PrimeNumbers.fsx"
open System
open PrimeNumbers

let answer = 
    sieveOfAtkin 2000000
    |> List.sumBy Numerics.BigInteger
