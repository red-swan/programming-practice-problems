(*
The following iterative sequence is defined for the set of positive integers:

n → n/2 (n is even)
n → 3n + 1 (n is odd)

Using the rule above and starting with 13, we generate the following sequence:

13 → 40 → 20 → 10 → 5 → 16 → 8 → 4 → 2 → 1
It can be seen that this sequence (starting at 13 and finishing at 1) contains 10 terms.
Although it has not been proved yet (Collatz Problem), it is thought that all starting numbers finish at 1.

Which starting number, under one million, produces the longest chain?

NOTE: Once the chain starts the terms are allowed to go above one million.

*)

let Collatzseq n = (Seq.unfold (fun state -> if(state = 1L) then None else Some(state, if(state % 2L = 0L) then state/2L else 3L*state + 1L)) (n : int64))
let Colseqlen n = int64 (Seq.length (Collatzseq n))
let mutable answer  = 0L
let mutable record = 0L
let mutable temp = 0L
for i in [1L .. 1000000L] do
    temp <- Colseqlen i
    if temp > record then 
        record <- temp
        answer <- i
printfn "The longest chain comes from the number %i with a chain length of %i" answer (record+1L)




(*
let nextNumber n = if n%2L = 0L then n/2L else 3L*n+1L
 
let findSequenceLength n =
    let mutable count = 1L
    let mutable current = n
 
    while current > 1L do
        current <- nextNumber current
        count <- count + 1L
    count
 
let longestSeq = [1L..999999L] |> Seq.maxBy findSequenceLength

*)
