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


#load "Tools.fsx"

open Tools

//let phi n = 
//    [1 .. n]
//    |> List.filter (fun x -> gcd n x = 1)
//
//let answer = 
//    [1 .. 1000000]
//    |> List.map (fun x -> (x, List.length (phi x)))
//    |> List.maxBy (fun (n,p) -> (float n) / (float p))




let primes = 
    1000000 |> sieveOfAtkin |> List.map float

let phi (n:float) = 
    primes 
    |> Seq.takeWhile (fun x -> x <= n) 
    |> Seq.filter (fun x -> n % x = 0.0)
    |> Seq.fold (fun acc elem -> acc*(1.0 - 1.0/elem)) n

let answer = 
    [1.0 .. 100000.0]
    |> List.map (fun x -> (x, (phi x)))
    |> List.maxBy (fun (n,p) -> n / p)

let eulerSeeds = 
    [1.0 .. 1000.0]
    |> List.map (fun x -> (x, (phi x)))
    |> Map.ofList    


let generateTotientMultiples n = 
    Seq.unfold (fun x -> Some((x,phi x),x+n)) n
   
(generateTotientMultiples 2.0 ) |> Seq.take 1000 |> Seq.toList


// number of trailing zeroes
let ntz (intg : int) = 
    let mutable i = intg
    let mutable z = 1
    if i &&& 0xffff = 0 then do
        z <- z + 16
        i <- i >>> 16
    if i &&& 0x00ff = 0 then do
        z <- z + 8
        i <- i >>> 8
    if i &&& 0x000f = 0 then do
        z <- z + 4
        i <- i >>> 4
    if i &&& 0x0003 = 0 then do
        z <- z + 2
        i <- i >>> 2
    z - (i &&& 1)
    

let getPhi (i:int) (phi : int []) = 
    if i &&& 1 > 0 then phi.[i >>> 1]
    elif i &&& 3 > 0 then phi.[i >>> 2]
    else 
        let z = ntz i
        phi.[i >>> z >>> 1] <<< z - 1

