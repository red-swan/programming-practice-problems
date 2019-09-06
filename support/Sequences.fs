namespace ProjectEuler

module Sequences = 


    // Polygonal Numbers
    let polygonalNumbers s = 
        Seq.initInfinite (fun idx -> ((pown (idx+1) 2) * (s-2) - (idx+1)*(s-4))/2)
    // 1, -1, 2, -2, 3, -3, ...
    let generalizedIntegers = 
        Seq.unfold (fun k -> Some (k, if k > 0 then -k else 1-k)) 1
        |> Seq.cache
    //
    let pentagonalNumbersGeneralized = 
        generalizedIntegers
        |> Seq.map (fun k -> k*(3*k-1)/2)
        |> Seq.cache