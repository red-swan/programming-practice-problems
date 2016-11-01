(*There are exactly ten ways of selecting three from five, 12345:

123, 124, 125, 134, 135, 145, 234, 235, 245, and 345

In combinatorics, we use the notation, 5C3 = 10.

In general,

nCr =	
n!
r!(n−r)!
,where r ≤ n, n! = n×(n−1)×...×3×2×1, and 0! = 1.
It is not until n = 23, that a value exceeds one-million: 23C10 = 1144066.

How many, not necessarily distinct, values of  nCr, for 1 ≤ n ≤ 100, are greater than one-million?*)


// euler 53 --------------------------------------------------------------
open System.Collections.Generic 

let cache = Dictionary<_,_>()  // TODO move inside 
let memoizedTRFactorial =
    let rec fac n k =  // must make tailcalls to k
        match cache.TryGetValue(n) with
        | true, r -> k r
        | _ -> 
            if n=0I then
                k 1I
            else
                fac (n-1I) (fun r1 ->
//                    printfn "multiplying by %d" n  //***
                    let r = r1 * n
                    cache.Add(n,r)
                    k r)
    fun n -> fac n id



let ncr n r = (memoizedTRFactorial n) / ((memoizedTRFactorial r)*(memoizedTRFactorial (n-r)))

let candidates = 
    [1I .. 100I]
    |> List.map (fun n -> (List.map (fun r -> ncr n r) [1I .. n]))
    |> List.map (fun nList -> List.filter (fun x -> x > 1000000I) nList)
    |> List.collect id
    |> List.filter (fun x -> x > 1000000I)
//    |> List.filter (fun (x:seq<Numerics.BigInteger>) -> not (List.isEmpty x))
//    |> List.collect (fun x -> x)

// use pascal's triangle
let answer =candidates |> List.length