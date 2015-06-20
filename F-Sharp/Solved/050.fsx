(*The prime 41, can be written as the sum of six consecutive primes:

41 = 2 + 3 + 5 + 7 + 11 + 13
This is the longest sum of consecutive primes that adds to a prime below one-hundred.

The longest sum of consecutive primes below one-thousand that adds to a prime, contains 21 terms, and is equal to 953.

Which prime, below one-million, can be written as the sum of the most consecutive primes?*)

// Generates a list of all primes below limit
let sieveOfAtkin limit =
    // initialize the sieve
    let sieve = Array.create (limit + 1) false
    // put in candidate primes: 
    // integers which have an odd number of
    // representations by certain quadratic forms
    let inline invCand n pred =
        if n < limit && pred then sieve.[n] <- not sieve.[n] 
    let sqrtLimit = int (sqrt (float limit))
    for x = 1 to sqrtLimit do
        for y = 1 to sqrtLimit do
            let xPow2 = x * x
            let yPow2 = y * y
            let n = 4 * xPow2 + yPow2 in invCand n (let m = n % 12 in m = 1 || m = 5)
            let n = 3 * xPow2 + yPow2 in invCand n (n % 12 = 7)
            let n = 3 * xPow2 - yPow2 in invCand n (x > y && n % 12 = 11)
    // eliminate composites by sieving
    let rec eliminate n =
        if n <= sqrtLimit 
        then if sieve.[n]
             then let nPow2 = n * n
                  for k in nPow2 .. nPow2 .. limit do
                      Array.set sieve k false
             eliminate (n + 2)
    eliminate 5
    // Generate list from the sieve (backwards)
    let rec generateList acc n =
        if n >= 5 then generateList (if sieve.[n] then n :: acc else acc) (n - 1)
        else acc
    2 :: 3 :: (generateList [] limit)


open System.Collections.Generic
let primes = new Dictionary<uint64,uint64>()
for i in (sieveOfAtkin 10000000) do
    primes.Add(uint64 i, 0UL)

let isprime x = primes.ContainsKey(x)

let matchsome  = function
    | Some x-> x
    | None -> 0UL

let candidates = sieveOfAtkin 1000000
let checkwindow n = 
    candidates
    |> Seq.windowed n
    |> Seq.map (fun x-> uint64 (Seq.sum x))
    |> Seq.filter (fun x -> isprime x && x< 1000000UL)
    |> (fun x-> if Seq.isEmpty x then None else Some(Seq.max x))

let searchsequence = [1 .. 600] |> List.rev

let answer = 
    searchsequence
    |> Seq.tryFind (fun x->  (checkwindow x) |> matchsome |> (<>)0UL )


// THIS IS SLOW BUT WORKS, WHY 600? TOTAL GUESS AND CHECK

