namespace ProjectEuler

module Sequences = 


    // Polygonal Numbers
    let polygonalNumbers s = 
        Seq.initInfinite (fun idx -> ((pown (idx+1) 2) * (s-2) - (idx+1)*(s-4))/2)