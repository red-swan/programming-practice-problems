(*The number 3797 has an interesting property. Being prime itself, it is possible to continuously remove digits 
from left to right, and remain prime at each stage: 3797, 797, 97, and 7. Similarly we can work from right to 
left: 3797, 379, 37, and 3.
Find the sum of the only eleven primes that are both truncatable from left to right and right to left.
NOTE: 2, 3, 5, and 7 are not considered to be truncatable primes.*)

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


let primeslist = set (sieveOfAtkin 1000000)
let isprime number = Set.exists ((=) number) primeslist

let digits number =  [for c in (string number) -> c] |> List.map (fun x -> (int x) - (int '0'))    

let contains x = Seq.exists ((=)x)

let truncatelr (number : int) = 
    let maxtens = (int (10. ** float (List.length (digits number) - 1)))
    //number - (int (10. ** float (List.length (digits number))) - 2)
    number - (number / maxtens)*maxtens
let truncaterl (number : int) = 
    number / 10

let leftrightprime number =
    Seq.unfold ( fun number -> if (number = 0) then None elif (isprime number) then Some(true, truncatelr number) else Some(false, truncatelr number)) number
    |> contains false
    |> not
let rightleftprime number = 
    Seq.unfold ( fun number -> if (number = 0) then None elif (isprime number) then Some(true, truncaterl number) else Some(false, truncaterl number)) number
    |> contains false
    |> not

// want to keep numbers that start with 2 or 5
let rec onlycandidateprimes number = 
    let losdigits = digits number
    if List.head losdigits = 2 || List.head losdigits = 5 then (number |> truncatelr |> onlycandidateprimes)
    else losdigits |> List.map (fun x -> x % 2=0 || x % 5 = 0) |> contains true |> not
    //THIS KEEPS IN NUMBER LIKE 222255555 with repeated 2's or 5's

// Alternative version of filtering candidates; maybe not optimized either
let onlycandidateprimes number =
    let filteritout numberasdigits = numberasdigits |> List.map (fun x -> x % 2=0 || x % 5 = 0) |> contains true |> not
    let losdigits = digits number
    if List.head losdigits = 2 || List.head losdigits = 5 then filteritout (List.tail losdigits)
    else filteritout (List.tail losdigits)

let answer = 
    sieveOfAtkin 1000000
        |> List.filter onlycandidateprimes
        |> List.filter leftrightprime
        |> List.filter rightleftprime
        |> List.filter (fun x -> x > 10)
        |> List.sum
