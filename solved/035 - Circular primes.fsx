(*The number, 197, is called a circular prime because all rotations of the digits: 197, 971, and 719, are themselves prime.

There are thirteen such primes below 100: 2, 3, 5, 7, 11, 13, 17, 31, 37, 71, 73, 79, and 97.

How many circular primes are there below one million?*)

// Function Declarations //

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



let digits number =  [for c in (string number) -> c] |> List.map (fun x -> (int x) - (int '0'))
let cycle number = number / 10 + (number % 10)*( int (10.** float ((List.length (digits number))-1)))
let contains x = Seq.exists ((=) x)
let primeslist = sieveOfAtkin 1000000
let isprime value = contains value primeslist

let getdigitcycle number = 
    Seq.unfold (fun x-> Some(x, (cycle x))) number
        |> Seq.take (List.length (digits number))
let iscircularprime number = 
    Seq.unfold (fun x-> if(isprime x) then Some(true, (cycle x)) else Some(false,(cycle x))) number
        |> Seq.take (List.length (digits number))
        |> Seq.exists ((=) false)
        |> not

// If the digits contain an even number or a 5, then it isn't circular because the offending digit will come around and is divisible
let onlyodddigits number = 
    digits number |> List.map (fun x -> x % 2=0 || x % 5 = 0) |> contains true |> not

let candidates = List.filter (onlyodddigits) primeslist |> List.filter iscircularprime

// Runs slow, could be improved with the circular prime function




let mutable temp = set [1 .. 3]
temp <- Set.map (fun x -> x*x) temp

  
let iscircularprime = not (contains false (cyclesequence number))
let allEvens = 
    let rec loop x = seq { yield x; yield! loop (x + 2) }
    loop 0;;

[1 .. 100] |> List.filter cyclesequence
// This works but has repeat members
// Thing about cutting down the amount by sieving out the higher numbers if the lower fails


// SCRATCH AND FURTHER IDEAS
rec functionsomething number =
	if (contains number thelistofnumbersalready) then thelistofnumbers @ (functionsomething number+1) 
	else checkforcycle
	
// The model should be start low, for each input: cycle it to get the listofcycles, add the set to either a list of circularprimes or a list of notcircularprimes, then move to the next input
// next step should be checking if the input is on either list already




