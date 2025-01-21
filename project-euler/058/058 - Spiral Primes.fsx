
// euler 58 ------------------------------------------------------------------
//Starting with 1 and spiralling anticlockwise in the following way, 
//a square spiral with side length 7 is formed.
//
//37 36 35 34 33 32 31
//38 17 16 15 14 13 30
//39 18  5  4  3 12 29
//40 19  6  1  2 11 28
//41 20  7  8  9 10 27
//42 21 22 23 24 25 26
//43 44 45 46 47 48 49
//
//It is interesting to note that the odd squares lie along the bottom 
//right diagonal, but what is more interesting is that 8 out of the 13 
//numbers lying along both diagonals are prime; that is, a ratio of 8/13 ≈ 62%.
//
//If one complete new layer is wrapped around the spiral above, a square 
//spiral with side length 9 will be formed. If this process is continued, 
//what is the side length of the square spiral for which the ratio of primes 
//along both diagonals first falls below 10%?

#load "../fsharp-lib/PrimeNumbers.fsx"
open PrimeNumbers

let isPrime (x:int) = 
    let checks = [2.0 .. sqrt(float x)]
    let rec loop input = 
        match input with
        |[] -> 1
        | head::tail when (float x) % head = 0.0 -> 0
        | head::tail -> loop tail
    if x < 2 then 0
    elif x = 2 then 1
    elif x = 3 then 1
    else (loop checks)

let corners2 = 
    let rec loop x = seq{yield (x, [(x-2)*(x-2)+(x-1);(x-2)*(x-2)+(x-1)*2;(x-2)*(x-2)+(x-1)*3;(x-2)*(x-2)+(x-1)*4])
                         yield! (loop (x+2)) }
    seq{ yield! (loop 3)}


let pctgs = 
    corners2
    |> Seq.map (fun (idx,corners) -> (idx,List.sumBy (fun x -> isPrimeI x) corners, List.length corners))
    |> Seq.scan (fun (_,acc1,acc2) (idx, primes, corners) -> (idx,acc1 + primes,acc2+corners)) (1,0,1)
    |> Seq.map (fun (idx,primes,corners) -> (idx,float(primes)/float(corners)))
    |> Seq.skip 1
    |> Seq.cache

let answer = 
    pctgs |> Seq.find (fun (idx,ratio) -> ratio < 0.1)


