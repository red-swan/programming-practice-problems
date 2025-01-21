(*
It is possible to write five as a sum in exactly six different ways:

4 + 1
3 + 2
3 + 1 + 1
2 + 2 + 1
2 + 1 + 1 + 1
1 + 1 + 1 + 1 + 1

How many different ways can one hundred be written as a sum of at least two positive integers?
*)


#I "../fsharp-lib"
#load "Sequences.fsx"


open Sequences
open System.Collections.Generic

// Memoize the function with a dictionary in a closure
let f () = 
    let d = new Dictionary<int,int>()
    for pair in [(0,1); (1,1);(2,2);(3,3); (4,5)] do
        d.Add(pair)
    (fun n -> 
        if n > d.Count then failwith "has not been calculated yet"
        if d.ContainsKey n then d.[n] else
        let is = 
             pentagonalNumbersGeneralized
             |> Seq.takeWhile (fun i -> i <= n) 
             |> Seq.map (fun i -> d.[n - i] )
             |> Seq.zip generalizedIntegers
             |> Seq.sumBy (fun (k, i) -> (pown -1 (k-1)) * i)
        do d.Add(n,is)
        is         
        )

let g = f()
g 1
g 2
g 5
do [1 .. 100] |> List.map g |> ignore

let answer = g 100 - 1
