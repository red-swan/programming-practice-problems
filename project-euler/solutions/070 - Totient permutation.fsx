(*
    Euler's Totient function, φ(n) [sometimes called the phi function], is used 
    to determine the number of positive numbers less than or equal to n which 
    are relatively prime to n. For example, as 1, 2, 4, 5, 7, and 8, are all 
    less than nine and relatively prime to nine, φ(9)=6.
    
    The number 1 is considered to be relatively prime to every positive number, 
    so φ(1)=1.

    Interestingly, φ(87109)=79180, and it can be seen that 87109 is a 
    permutation of 79180.

    Find the value of n, 1 < n < 10^7, for which φ(n) is a permutation of n and 
    the ratio n/φ(n) produces a minimum.
*)

#load "../support/HigherOrder.fs"
#load "../support/PrimeNumbers.fs"
#load "../packages/MathNet.Numerics.FSharp/MathNet.Numerics.fsx"

open ProjectEuler
open System.Numerics
open MathNet.Numerics


let primeCeiling = int(1e4)
let algoCeiling = int(1e7)
let primes = primeCeiling |> sieveOfAtkin |> List.map BigInteger


// Return an int list representing the digits of a BigInt
let digitsInt (number : BigInteger)= 
    let rec loop (digits : int list) (num : BigInteger)= 
        match num with
        | x when x = 0I -> digits
        | _ -> loop (int(num % 10I) :: digits) (num / 10I)
    loop [] number

// Return a sorted list of digits of a BigInt
let digitsSorted number = 
    number 
    |> digitsInt
    |> List.sort

// Return euler's totient function froma  list of divisors
let phiDivisors (divisors : BigInteger list) =
    // let recip n = (1. - (1./(float n)))
    let recip n = (1N - (1N / BigRational.FromBigInt(n)))
    let n = 
        divisors
        |> List.reduce (*) 
        |> BigRational.FromBigInt
    divisors
        |> List.map recip
        |> List.reduce (*)
        |> ((*) n)
        |> BigRational.ToBigInt

// return n/phi(n) from a list of divisors
let totRatioDivisors divisors = 
    let n = List.reduce (*) divisors
    BigRational.FromBigInt(n) / BigRational.FromBigInt(phiDivisors divisors)

// Return true if a phi(n) is a permutation of the digits of n
// where n is the multiple of the specified divisors
let isPhiPermutationDivisors divisors =
    let number = List.reduce (*) divisors
    let numberDigits = number |> digitsSorted
    let phiDigits = divisors |> phiDivisors |> digitsSorted
    numberDigits = phiDigits


// returns all the pairs of divisors of numbers under the search ceiling
// also contingent on the ceiling we set for the primes
let searchSpace = 
    list { let upper = BigInteger(algoCeiling)
           let! x = primes
           let! y = primes
           if x <= y && (x*y) < upper
           then return [x;y]}


let answer = 
    searchSpace
    |> List.filter isPhiPermutationDivisors
    |> List.sortBy totRatioDivisors
    |> List.take 1
    |> List.map (fun divisors -> (List.reduce (*) divisors, 
                                  divisors, 
                                  divisors |> totRatioDivisors |> BigRational.ToDouble))