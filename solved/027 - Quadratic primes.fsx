(*
Euler discovered the remarkable quadratic formula:

n² + n + 41

It turns out that the formula will produce 40 primes for the consecutive values n = 0 to 39.
However, when n = 40, 40^2 + 40 + 41 = 40(40 + 1) + 41 is divisible by 41, and certainly when n = 41, 41² + 41 + 41 is clearly divisible by 41.

The incredible formula  n² - 79n + 1601 was discovered, which produces 80 primes for the consecutive values n = 0 to 79. The product of the coefficients, -79 and 1601, is -126479.

Considering quadratics of the form:

n² + an + b, where |a| < 1000 and |b| < 1000

where |n| is the modulus/absolute value of n
e.g. |11| = 11 and |-4| = 4
Find the product of the coefficients, a and b, for the quadratic expression that produces the 
maximum number of primes for consecutive values of n, starting with n = 0.

*)


// The method is slow because it is checking way too many
// What is the way to pair it down?

// (n-p)^2 + (n-p) + 41
// n^2 + n*(-2p+1) + (p^2-p+41)
//            a          b
#load "Tools.fs"
open Tools

// Generate a list of primes we will reference later
let primeslist = sieveOfAtkin 2000000 

// Create the p candidates
let pcandidates = List.filter (fun p -> abs(p*p-p+41) < 1000) [-1000 .. 1000]
// Change the candidates into (a,b) form
let candidates = List.map (fun p -> ( 2*p-1 , p*p-p+41 ) ) pcandidates

//This function returns the amazing function output from the inputs
let amazfunc n (a :int, b :int) =  n*n + n*a + b
//This function tests whether or not the inputs of the amazing function return a prime
let amazcheck n (a,b) = List.exists (function x -> x = amazfunc n (a,b)) primeslist

// This gives us the sequence of primes that are created from a pair (a,b) 
let amazseq (a,b) = Seq.unfold ( fun n ->
                                if amazcheck n (a,b) then Some(amazfunc n (a,b), n+1)
                                else None ) 0
// Find the answer
let answer = 
    candidates
    |> Seq.maxBy (fun x -> Seq.length (amazseq x))
    |> (fun (a,b) -> a*b)





