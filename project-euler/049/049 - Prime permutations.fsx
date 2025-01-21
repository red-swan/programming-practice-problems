(*The arithmetic sequence, 1487, 4817, 8147, in which each of the terms increases by 3330, is unusual in two ways:
(i) each of the three terms are prime, and,
(ii) each of the 4-digit numbers are permutations of one another.

There are no arithmetic sequences made up of three 1-, 2-, or 3-digit primes, exhibiting this property,
but there is one other 4-digit increasing sequence.

What 12-digit number do you form by concatenating the three terms in this sequence?*)

// Used to create Hashset further below
open System.Collections.Generic

// Generates a list of all primes below a specified limit
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

// Get the list of all the primes we're interested in
let candidates = sieveOfAtkin 10000 |> List.filter (fun x-> x>999)

// This function will map a number into a sorted list of its digits
// Useful for grouping all the numbers by their digits
let rec digitgroups number = 
    let rec loop = function
    | 0 -> []
    | x -> (x % 10)::(loop (x/10))
    loop number |> List.sort

//  This function takes a list of numbers and looks for any numbers that are twice/half of each other
//  The function then returns the first of this pair and only of the first pair that was found
let hasdoubles (inputlist : int list) =
    let hash = new HashSet<int>()
    let rec loop (something : int list) = 
        if List.isEmpty something then None
        elif hash.Contains((List.head something)/2) then Some ((List.head something)/2)
        else 
            hash.Add(List.head something) |> ignore
            loop (List.tail something)
    loop inputlist

//  Take a list of numbers and returns the first three to all have the same distance between them
//  Returns None otherwise
//  The mechanism here is that it subtracts the head from each element of the tail, and then uses the
//  above hasdoubles function to see if any are twice/half of each other, those being the two in question
let rec find (inputlist : int list) = 
    if inputlist = [] then None
    else
        let search = (List.map (fun t-> t-inputlist.Head) inputlist.Tail |> hasdoubles)
        if search.IsSome then Some(inputlist.Head,inputlist.Head+search.Value,inputlist.Head+search.Value*2)
        else find inputlist.Tail

//  Bool function built to test if the digit groups have more than 2 numbers that share digits
let filteroutsmallgroups (input : int list * seq<int>) = input |> snd |> (fun x -> Seq.length x >2)

//  Start the timer and run the optimzed algorithm
let stopWatch = System.Diagnostics.Stopwatch.StartNew()
let answer = 
    candidates
    |> Seq.groupBy digitgroups
    |> Seq.filter filteroutsmallgroups
    |> Seq.map (fun (a,b) -> (b |> Seq.toList |> find))
    |> Seq.filter (fun x-> x.IsSome)
    |> Seq.nth 1
    |> (fun x-> x.Value |> (fun (a,b,c) -> string a + string b + string c))
stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMilliseconds

(*
timer:
9.123400
*)
