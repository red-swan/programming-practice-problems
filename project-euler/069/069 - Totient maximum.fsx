(*
Euler's Totient function, φ(n) [sometimes called the phi function], is used to
determine the number of numbers less than n which are relatively prime to n. 
For example, as 1, 2, 4, 5, 7, and 8, are all less than nine and relatively 
prime to nine, φ(9)=6.

n	Relatively Prime	φ(n)	n/φ(n)
2	1	                1	    2
3	1,2	                2	    1.5
4	1,3	                2	    2
5	1,2,3,4	            4	    1.25
6	1,5	                2	    3
7	1,2,3,4,5,6	        6	    1.1666...
8	1,3,5,7	            4	    2
9	1,2,4,5,7,8	        6	    1.5
10	1,3,7,9	            4	    2.5
It can be seen that n=6 produces a maximum n/φ(n) for n ≤ 10.

Find the value of n ≤ 1,000,000 for which n/φ(n) is a maximum.
*)

// where p is prime and divides n
// n / phi(n) = prod(p/(p-1))

// if n_k is the product of the k smallest primes
// n_k / phi(n_k)  >  n

#load "../fsharp-lib/PrimeNumbers.fsx"
open PrimeNumbers
open System.Numerics
let primes = 
    1000 |> sieveOfAtkin |> List.map float
let phi n =
    primes
    |> Seq.takeWhile (fun x -> x <= n)
    |> Seq.filter (fun x -> n % x = 0.0)
    |> Seq.map (fun p -> (1.0 - (1.0/p)))
    |> Seq.reduce (*)
    |> ((*)n)
let totRatio n = 
    n / (phi n)
let maximalTotients = 
    primes
    |> List.scan (fun acc elem -> acc * elem) 1.0
    |> List.takeWhile (fun x -> x < (1e6))

let answer = 
    List.max maximalTotients


let smallestPhi k = 
    primes 
    |> List.map BigInteger
    |> List.windowed k
    |> List.map (fun x -> List.reduce (*) x)
    |> List.filter (fun x -> x < BigInteger(1e7))
    |> List.rev

smallestPhi 8