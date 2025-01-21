(*
Consider the following "magic" 3-gon ring, filled with the numbers 1 to 6, and 
each line adding to nine.


Working clockwise, and starting from the group of three with the numerically 
lowest external node (4,3,2 in this example), each solution can be described 
uniquely. For example, the above solution can be described by the set: 
4,3,2; 6,2,1; 5,1,3.

It is possible to complete the ring with four different totals: 
9, 10, 11, and 12. There are eight solutions in total.

Total	Solution Set
9	4,2,3; 5,3,1; 6,1,2
9	4,3,2; 6,2,1; 5,1,3
10	2,3,5; 4,5,1; 6,1,3
10	2,5,3; 6,3,1; 4,1,5
11	1,4,6; 3,6,2; 5,2,4
11	1,6,4; 5,4,2; 3,2,6
12	1,5,6; 2,6,4; 3,4,5
12	1,6,5; 3,5,4; 2,4,6
By concatenating each group it is possible to form 9-digit strings; 
the maximum string for a 3-gon ring is 432621513.

Using the numbers 1 to 10, and depending on arrangements, it is possible 
to form 16- and 17-digit strings. What is the maximum 16-digit string 
for a "magic" 5-gon ring?
*)

#load "../fsharp-lib/Combinatorics.fsx"
open Combinatorics

let testIfInnerNumsWork (n : int) (target : int) (inners : seq<int>)= 
    let remainder = [1 .. n] |> List.filter (fun x -> not (Seq.contains x inners))
    let seeds = 
        seq { yield (Seq.head inners) }
        |> Seq.append (inners)
        |> Seq.pairwise
        |> Seq.toList

    let rec loop output seeds remainders = 
        match seeds with
        | [] -> output
        | (a,b) :: tail -> 
            let requiredNum = target - a - b
            if Seq.contains requiredNum remainders
            then loop ([requiredNum;a;b] :: output) tail (Seq.filter (fun x -> x <> requiredNum) remainders)
            else []
    (loop [] seeds remainder)
    |> List.rev

let testTargets n (targetList : int list) inners = 
    List.map (fun x -> testIfInnerNumsWork n x inners) targetList

let rotateNagon (agon : int list list) = 
    let min = List.min agon
    let rec loop intListList = 
        match intListList with
        | head::tail when head = min -> intListList
        | head::tail -> loop (tail @ [head])
    loop agon

let answer = 
    (permutations 5 [1 .. 10])
    |> Seq.filter (fun x -> List.contains 2 x)
    |> Seq.collect (testTargets 10 [10 .. 20])
    |> Seq.filter (fun x -> x |> List.isEmpty |> not)
    |> Seq.map rotateNagon
    |> Seq.map (fun x -> Seq.collect id x)
    |> Seq.map (fun x -> x |> Seq.map (fun y -> string y))
    |> Seq.map (fun x -> Seq.reduce (+) x)
    |> Seq.filter (fun x -> x.Length = 16)
    |> Seq.map (fun x -> System.Numerics.BigInteger.Parse(x))
    |> Seq.max