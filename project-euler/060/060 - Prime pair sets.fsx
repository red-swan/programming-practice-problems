(*
The primes 3, 7, 109, and 673, are quite remarkable. By taking any two primes 
and concatenating them in any order the result will always be prime. For 
example, taking 7 and 109, both 7109 and 1097 are prime. The sum of these four 
primes, 792, represents the lowest sum for a set of four primes with this 
property.

Find the lowest sum for a set of five primes for which any two primes 
concatenate to produce another prime.
*)

#load "../fsharp-lib/PrimeNumbers.fsx"
#load "../fsharp-lib/HigherOrder.fsx"
open PrimeNumbers
open HigherOrder
open System

// over all this was a fun problem
// our approach was to start with a few primes (<30) and see what familyTrees they form
// by adding primes to each family
// 
// Things that worked well
//   - working with sets made the checking for duplicate families easy
//   - creating a primelookup function made primality easier to check
//   - setting an upper bound on the primeSpace was necessary
//   - creating functions for each set step made creating the solution easier
//
// improvements: 
//   - create a lookup for what primes concat with each other so we don't have to compute 
//     that each time
//   - concat primes by adding, not string operations for prime Lookup


// type declarations
type prime = uint64
let inline prime a = uint64 a
type family = Set<prime>

// prime number related functions ----------------------------------------------
let PRIMEUPPERBOUND = 10000000
// cached primes - poor man's memoization
let cachedPrimes = 
    PRIMEUPPERBOUND 
    |> sieveOfAtkin 
    |> List.map prime
    |> Set.ofList
// function to find primality of number
let isPrimeLookup (number : uint64) = 
    if number < (Set.maxElement cachedPrimes)
    then Set.contains number cachedPrimes
    else isPrime (System.Numerics.BigInteger(number))
// function to see if two numbers' concatenations make prime numbers
let doPrimesConcat (x : prime) (y : prime) =
        let xstr,ystr = x.ToString(),y.ToString()
        isPrimeLookup (UInt64.Parse(xstr + ystr)) &&
        isPrimeLookup (UInt64.Parse(ystr + xstr))
// function to check if all primes in list make concatenations
let doAllPrimesConcat (primeList: uint64 list) = 
    let rec loop primeList = 
        match primeList with
        | [] -> true
        | head::tail -> 
            if List.forall (fun v -> doPrimesConcat head v) tail
            then loop tail
            else false
    loop primeList
// wrapper function of above function
let doesNewPrimeConcat (fam : family) (candidate : prime) = 
    doAllPrimesConcat ((Set.toList fam) @ [candidate])



// Family/Tree related functions -----------------------------------------------

// function to create sets from all primes and existing set
let introduceFamilyToNewMember (candidate : prime) (fam : family) : family =
    if fam.Contains candidate then Set.empty//fam
    elif doesNewPrimeConcat fam candidate
         then Set.add candidate fam
    else Set.empty//fam
// function like above, but to process list of candidates
let introduceFamilyToNewMemberSet (candidates : Set<prime> ) (fam : family) : Set<family>=
    candidates
    |> Set.map (fun candidate -> introduceFamilyToNewMember candidate fam )
    |> Set.filter (fun x -> not x.IsEmpty)
// function to map 
let introduceFamilyTreeToNewMemberSet (candidateSet : Set<prime>) (familyTree : Set<family>) : Set<family>= 
    familyTree
    |> Set.fold (fun accOuter famSet 
                  -> Set.fold (fun accInner fam 
                                -> Set.add fam accInner) 
                               accOuter 
                               (introduceFamilyToNewMemberSet candidateSet famSet)) 
                 Set.empty



// Running the solution --------------------------------------------------------

// You have to cut off the prime search somewhere
let primeSpace = sieveOfAtkin 10000 |> List.map uint64 |> Set.ofList

// we assume that the families must include a small prime
let familySeeds = 
    primeSpace
    |> Set.filter (fun x -> x < 30UL)
    |> Set.map (fun x -> (set[x]))

#time
let answer = 
    [2 .. 5]
    |> List.fold (fun ans famTree -> introduceFamilyTreeToNewMemberSet primeSpace ans) familySeeds 
    |> Set.toList
    |> List.map (fun x -> (x |> Set.toList |> List.sum, x))
    |> List.minBy fst
#time // 37.793 seconds


////////////////////////////////////////////////////////////////////////////////
//                                 OPTIMIZATION                               //
////////////////////////////////////////////////////////////////////////////////

let countDigitsUint64 (number:uint64) =
    int ((log10 (float number)) + 1.0)

let concatsToPrime'' (a, b) = 
    (countDigitsUint64 b)
    |> pown 10
    |> uint64
    |> (fun x -> a*x + b)
    |> isPrimeLookup

let concatsToPrime' a b = 
    memoize concatsToPrime'' (a,b)

let concatsToPrime a b = 
    concatsToPrime' a b
    &&
    concatsToPrime' b a




///////////////// unoptimized portion

// function to check if all primes in list make concatenations
let doAllPrimesConcat (primeList: uint64 list) = 
    let rec loop primeList = 
        match primeList with
        | [] -> true
        | head::tail -> 
            if List.forall (fun v -> concatsToPrime head v) tail
            then loop tail
            else false
    loop primeList
// wrapper function of above function
let doesNewPrimeConcat (fam : family) (candidate : prime) = 
    doAllPrimesConcat ((Set.toList fam) @ [candidate])



// Family/Tree related functions -----------------------------------------------

// function to create sets from all primes and existing set
let introduceFamilyToNewMember (candidate : prime) (fam : family) : family =
    if fam.Contains candidate then Set.empty//fam
    elif doesNewPrimeConcat fam candidate
         then Set.add candidate fam
    else Set.empty//fam
// function like above, but to process list of candidates
let introduceFamilyToNewMemberSet (candidates : Set<prime> ) (fam : family) : Set<family>=
    candidates
    |> Set.map (fun candidate -> introduceFamilyToNewMember candidate fam )
    |> Set.filter (fun x -> not x.IsEmpty)
// function to map 
let introduceFamilyTreeToNewMemberSet (candidateSet : Set<prime>) (familyTree : Set<family>) : Set<family>= 
    familyTree
    |> Set.fold (fun accOuter famSet 
                  -> Set.fold (fun accInner fam 
                                -> Set.add fam accInner) 
                               accOuter 
                               (introduceFamilyToNewMemberSet candidateSet famSet)) 
                 Set.empty



// Running the solution --------------------------------------------------------

// You have to cut off the prime search somewhere
let primeSpace = sieveOfAtkin 10000 |> List.map uint64 |> Set.ofList

// we assume that the families must include a small prime
let familySeeds = 
    primeSpace
    |> Set.filter (fun x -> x < 30UL)
    |> Set.map (fun x -> (set[x]))

#time 
let answer = 
    [2 .. 5]
    |> List.fold (fun ans famTree -> introduceFamilyTreeToNewMemberSet primeSpace ans) familySeeds 
    |> Set.toList
    |> List.map (fun x -> (x |> Set.toList |> List.sum, x))
    |> List.minBy fst
#time // 35.397 seconds