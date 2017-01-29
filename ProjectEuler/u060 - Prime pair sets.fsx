(*
The primes 3, 7, 109, and 673, are quite remarkable. By taking any two primes 
and concatenating them in any order the result will always be prime. For 
example, taking 7 and 109, both 7109 and 1097 are prime. The sum of these four 
primes, 792, represents the lowest sum for a set of four primes with this 
property.

Find the lowest sum for a set of five primes for which any two primes 
concatenate to produce another prime.
*)

#load "Tools.fsx"

open Tools
open System
open System.Numerics

// set the upper limit of primes we are going to check
let UPPERLIMIT = 10000

// gather a set of primes
let PrimeSet = 
    sieveOfAtkin (UPPERLIMIT)
    |> List.map (fun x -> BigInteger(x))
    |> Set.ofList 
// let's pair down the primes we want to check
let candidatePrimes =
    Set.filter (fun x -> x < BigInteger(UPPERLIMIT)) PrimeSet

// check if a value is in the candidate prime set
let isPrime value = 
    if value > BigInteger(UPPERLIMIT) || value < 1I
    then failwith "cannot check if number outside of candidates is prime"
    Set.contains value candidatePrimes

// function that returns the string concatenation of numbers
let concatNums (bigInt1:BigInteger) (bigInt2:BigInteger) = 
    (Convert.ToString bigInt1, Convert.ToString bigInt2)
    |> (fun (a,b) -> a+b)
    |> (fun x -> BigInteger.Parse(x))

// return the set of numbers that concat front and back and are still prime
// for a single number
let concatPrimesOf (bigInt:BigInteger) = 
    candidatePrimes
    |> Set.filter (fun x -> concatNums bigInt x |> isPrime)
    |> Set.filter (fun x -> concatNums x bigInt |> isPrime)

let test = concatPrimesOf 3I


// would it be faster to look up primes by miller rabin or by searching for the 
// number in our prime set ??

let isPrime2 value = 
    Set.contains value candidatePrimes

let concatPrimesOf2 (bigInt:BigInteger) = 
    candidatePrimes
    |> Set.filter 
        (fun x -> (concatNums bigInt x |> isPrime2) && 
                  (concatNums x bigInt |> isPrime2))


#time

let test = concatPrimesOf 3I
let test2 = concatPrimesOf2 3I


#time




















// should we be using arrays rather than sets?

#time

let candidatePrimes2 = 
    sieveOfAtkin 30000
    |> List.toArray
    |> Array.Parallel.map (fun x -> BigInteger(x))    

let concatPrimesOf2 (bigInt:BigInteger) = 
    candidatePrimes2
    |> Array.filter (fun x -> concatNums bigInt x |> isPrime)
    |> Array.filter (fun x -> concatNums x bigInt |> isPrime)

let test2 = concatPrimesOf2 3I

// arrays offer fewer set operations and not much speed up

