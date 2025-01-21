(*
A permutation is an ordered arrangement of objects. For example, 3124 is one 
possible permutation of the digits 1, 2, 3 and 4. If all of the permutations 
are listed numerically or alphabetically, we call it lexicographic order. The 
lexicographic permutations of 0, 1 and 2 are:

012   021   102   120   201   210

What is the millionth lexicographic permutation of the digits 0, 1, 2, 3, 4, 5, 
6, 7, 8 and 9?
*)

#load "../fsharp-lib/Combinatorics.fsx"
open Combinatorics


// brute force method ----------------------------------------------------------

let stopWatchBrute = System.Diagnostics.Stopwatch.StartNew()

let answer = 
    (permutations 10 [0 .. 9]) 
    |> Seq.map (fun listOfNums -> listOfNums 
                                  |> Seq.map (fun y -> string y) 
                                  |> String.concat "")
    |> Seq.sort
    |> Seq.item 1000001

stopWatchBrute.Stop()
printfn "%f" stopWatchBrute.Elapsed.TotalSeconds 
// 47 s - ouch!

// constructing the nth of a list generally ------------------------------------

// function for quick integer factorials
let intFact = function
    | 0 | 1 -> 1
    | x -> List.reduce (fun first second -> first*second) [1 .. x]

// function to find the nth sorted permutation of any list
let FindNth (index:int) (lst: 'a list) = 
    
    // first check if the requested item is sensible
    if index > (intFact lst.Length) 
    then failwith "index must be smaller than number of permutations"
    
    // this will loop through, finding how many permutations are possible for
    // all the items behind, and taking the item in the list that is that
    // many blocks/cycles behind

    let rec loop target (lstRmndr: 'b list) (acc:'b list) =
        match lstRmndr with
        // we are at the last item and it must be chosen
        | [value] -> acc @ [value]
        // we are somewhere in our loop with a certain set of blocks
        // to go until we hit our required iteration
        // we search until we get right under but not over our 
        // required number
        | head::tail  ->
            // the number of permuations of all the items behind
            let numPermutations = intFact (lstRmndr.Length - 1)
            // the number of items that will be cycled through
            let takeIndex = target / numPermutations
            // the remaining iterations of permutations until we hit our goal
            let newTarget = target - (takeIndex * numPermutations)
            // with our item chosen, we move on to choosing the next item
            loop 
                newTarget 
                (List.drop takeIndex lstRmndr)
                (acc @ [(List.item takeIndex lstRmndr)])

    // run the search loop
    loop (index-1) lst []

let stopWatchCalculated = System.Diagnostics.Stopwatch.StartNew()

let answer2 =
    FindNth 1000000 [0 .. 9]

stopWatchCalculated.Stop()
printfn "%f" stopWatchCalculated.Elapsed.TotalMilliseconds
// 2.8487 ms


(*
Takeaways:
----------
- again, constructing the answer is faster than constucting all answers and 
  searching for the right one
- the ordering allows us to know which ones come next and 'seek ahead' to 
  construct the answer
*)