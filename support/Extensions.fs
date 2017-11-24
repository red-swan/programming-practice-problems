namespace ProjectEuler

// TODO
// conform functions to Seq module type names ("'T" instead of "input")

open System.Collections.Generic


module List = 
    let splice fstLst sndLst = 
        let rec loop = function
        | (xs,[]) -> xs
        | ([], ys) -> ys
        | (x::xs,y::ys) -> x :: y :: (loop (xs, ys))
        loop (fstLst, sndLst)
    let removeFirst item lst =
        let rec loop listRemainder listAccumulator = 
            match listRemainder with
            | [] -> listAccumulator
            | head :: tail when head = item -> listAccumulator @ tail
            | head :: tail -> loop tail (listAccumulator @ [head])
        loop lst []
    let drop n lst = 
        if n > ((List.length lst) - 1) || n < 0 then failwith "drop index out of list bounds"
        let rec loop step listRemainder listAccumulator = 
            match listRemainder with
            | [] -> listAccumulator
            | head :: tail when step = n -> listAccumulator @ tail
            | head :: tail -> loop (step + 1) (tail) (listAccumulator @ [head])
        loop 0 lst []

module Seq =
    // create an infinite Sequence from any Ienumerable object ---------------------
    let infiniteOf repeatedList = 
        Seq.initInfinite (fun _ -> repeatedList) 
        |> Seq.concat
    
    let triplewise (source: seq<_>) =
        seq { use e = source.GetEnumerator() 
            if e.MoveNext() then
                let i = ref e.Current
                if e.MoveNext() then
                    let j = ref e.Current
                    while e.MoveNext() do
                        let k = e.Current 
                        yield (!i, !j, k)
                        i := !j
                        j := k }
    let hasduplicates (input : seq<'a>) =
        let ourDict = new Dictionary<_,_>()
        let rec loop (seqOfThings : seq<'a>) = 
            if    Seq.isEmpty seqOfThings then false
            elif  ourDict.ContainsKey(Seq.head seqOfThings) then true
            else  ourDict.Add(Seq.head seqOfThings, 1) |> ignore
                  loop (Seq.tail seqOfThings)
        loop input

    // How to enumerate a sequence
    let indexList indices (s : 'a seq) = 
        seq { let indices = ref indices
              let i = ref 0
              use e = s.GetEnumerator()
              while not (List.isEmpty !indices) && e.MoveNext() do
                  match !indices with
                  | index :: rest when !i = index ->
                      i := !i + 1
                      indices := rest
                      yield e.Current
                  | _ -> i := !i + 1 }

