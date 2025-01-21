(*Nim is a game played with heaps of stones, where two players take it in turn to remove any number of stones from any heap until no stones remain.

We'll consider the three-heap normal-play version of Nim, which works as follows:
- At the start of the game there are three heaps of stones.
- On his turn the player removes any positive number of stones from any single heap.
- The first player unable to move (because no stones remain) loses.

If (n1,n2,n3) indicates a Nim position consisting of heaps of size n1, n2 and n3 then there is a simple function X(n1,n2,n3) — that you may look up or attempt to deduce for yourself — that returns:

zero if, with perfect strategy, the player about to move will eventually lose; or
non-zero if, with perfect strategy, the player about to move will eventually win.
For example X(1,2,3) = 0 because, no matter what the current player does, his opponent can respond with a move that leaves two heaps of equal size, at which point every move by the current player can be mirrored by his opponent until no stones remain; so the current player loses. To illustrate:
- current player moves to (1,2,1)
- opponent moves to (1,0,1)
- current player moves to (0,0,1)
- opponent moves to (0,0,0), and so wins.

For how many positive integers n ≤ 2^30 does X(n,2n,3n) = 0 ?*)

// memoized fib function using uint64
let rec fib =
    let dict = new System.Collections.Generic.Dictionary<_,_>()
    fun n ->
        match dict.TryGetValue(n) with
        | true, v -> v
        | false, _ -> 
            let temp =
                if n = 0UL then 0UL
                elif n = 1UL then 1UL
                else fib (n - 1UL) + fib(n - 2UL)
            dict.Add(n, temp)
            temp

let answer = fib 32UL



// Or by brute force
let mutable count = 0
for n in 1 .. int( 2.0**30.0) do
    if n ^^^ 2*n ^^^ 3*n = 0 then count <- count + 1
let answer = count




/// THIS IS ALL SCRATCH

open System

//  returns a binary string of a decimal integer
let binary (n:int) = Convert.ToString(n,2)

//  takes a tuple and returns the tuple as binary strings
let binarytuple (a:int,b:int,c:int) = 
    let max = [a;b;c] |> List.max |> binary
    let length = max.Length
    [a;b;c] |> List.map binary |> List.map (fun x-> x.PadLeft(length,'0'))

//  takes a binary string and returns a reversed 
let reversebinary (binarystring : string) = 
    binarystring.ToCharArray() |>Array.rev |> Array.map (fun x-> if x='1' then 1 else 0)


let nimsum input1 input2 input3 =
    let [a;b;c] = (input1,input2,input3)|> binarytuple |> List.map reversebinary
    let lastindex = a.Length - 1
    let answer = 
        Seq.unfold (fun n -> 
            if n > lastindex then None else 
            Some([a.[n];b.[n];c.[n]],n+1)) 0
    Seq.tryFind (fun x -> (List.sum x) |> (fun x-> x % 2) |> (fun x -> x =1)) answer
    |> (fun x-> match x with
                | Some x-> 1
                | None -> 0)

let candidates = 
    [1 .. 1000000]
    |> List.map (fun x-> (x,2*x,3*x))
    |> List.map (fun (a,b,c) -> nimsum a b c)
    |> List.sum

let answer = 
    Seq.unfold (fun n -> Some(nimsum n (2*n) (3*n),n+1)) 1
    |> Seq.take (int (2.0**30.0))
    |> Seq.filter (fun x-> x=0)
    |> Seq.length

let getcandidatetuple n = 
    binarytuple (n,2*n,3*n)
