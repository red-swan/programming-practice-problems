(*It was proposed by Christian Goldbach that every odd composite number can be written as the sum of a prime and twice a square.

9 = 7 + 2×1^2
15 = 7 + 2×2^2
21 = 3 + 2×3^2
25 = 7 + 2×3^2
27 = 19 + 2×2^2
33 = 31 + 2×1^2

It turns out that the conjecture was false.

What is the smallest odd composite that cannot be written as the sum of a prime and twice a square?*)



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

let squares = Seq.unfold (fun n-> Some(n*n,n+1)) 1 |> Seq.cache
let candidatesquares = Seq.map (fun x-> x*2) squares |> Seq.cache
let primes = sieveOfAtkin 1000000
let isprime x = 
    let rec loop input = 
        match input with
        | [] -> false
        | head::tail when head = x -> true
        | head::tail -> loop tail
    loop primes

let primefactors n = 
  let rec loop c p =
    if c < (p * p) then [c]
    elif c % p = 0UL then p :: (loop (c/p) p)
    else loop c (p + 1UL)
  loop n 2UL

let matchsome input = 
    match input with
        | Some x -> x
        | None -> -1

let findpair n = 
    let candidateprimes = Seq.takeWhile (fun x-> x<n) primes |> Seq.toList |> List.rev
    Seq.takeWhile (fun x-> x<n) candidatesquares 
    |> Seq.map (fun x-> n-x) 
    |> Seq.tryFind (fun x-> isprime x)
    |> matchsome
    |> fun x-> (n,x)

let candidates = 
    let rec loop x = seq{ 
            if (isprime x |> not) then yield x; yield! loop (x+2)
            else yield! loop (x+2) }
    loop 3

let answer = 
    candidates
    |> Seq.map findpair
    |> Seq.find (fun (a,b) -> b = -1)
    |> fun (a,b) -> a




// THIS IS SLOW BUT STEADY
// LUCKY THAT THE NUMBER IS SO LOW








