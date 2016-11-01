(*By listing the first six prime numbers: 2, 3, 5, 7, 11, and 13, we can see that the 6th prime is 13.

What is the 10 001st prime number?*)
#load "Tools.fs"
open Tools

// Generates a list of all primes below limit


let answer = sieveOfAtkin 500000 |> List.nth <| 10000
