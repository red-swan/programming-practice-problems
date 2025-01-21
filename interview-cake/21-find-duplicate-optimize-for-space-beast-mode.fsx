

// the integers are in the range 1..n
// the list has a length of n+1
let walkArr steps startPos (arr : int []) = 
    let mutable position = startPos
    for i in 1 .. steps do
        position <- arr.[position - 1]  
    position
let getCycleLength (arr : int []) startPos  = 
    let rec loop step pos = 
        seq { yield step; 
              if pos <> startPos || step = 0 then yield! loop (step + 1) arr.[pos - 1]}
    loop 0 startPos |> Seq.last
let findFirstInCycle arr cycleLength = 
    let n = Array.length arr
    let pos1 = arr.[n-1]
    let pos2 = walkArr (cycleLength+1) n arr
    (pos1,pos2)
    |> Seq.unfold (fun (pos1,pos2) -> 
        let newState = (arr.[pos1 - 1], arr.[pos2 - 1])
        Some((pos1,pos2), newState))
    |> Seq.find (fun (a,b) -> a = b)
    |> fst
let findDuplicate (input : int []) = 
    let n = Array.length input - 1
    input |> walkArr n (n+1) |> getCycleLength input |> findFirstInCycle input
    
let sample1 = [|1;4;7;2;4;9;8;3;5|]
let sample2 = [|3; 4; 2; 3; 1; 5|]
let sample3 = [|3; 1; 2; 2|]
let sample4 = [|4; 3; 1; 1; 4|]


findDuplicate sample1
findDuplicate sample2
findDuplicate sample3
findDuplicate sample4










