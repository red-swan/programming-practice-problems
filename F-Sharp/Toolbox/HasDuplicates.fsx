



// The naive checker
let hasduplicates inputlist = 
    let rec loop = function
    | head::tail when Seq.exists ((=)head) tail -> true
    | head::tail -> loop tail
    | [] -> false
    loop inputlist

(*
the naive approach was not tested
*)


// Using sets
let hasduplicates (inputlist : 'a list)=
    let dictionary = Set.empty
    let rec loop (cache : Set<'a>) (something : 'a list)=
        if (List.isEmpty something) then false
        elif Set.contains (List.head something) cache then true
        else loop (Set.add (List.head something) cache) (List.tail something)
    loop dictionary inputlist

(* hasduplicates ([1 .. 1000000]@[980397])
4879.022000
*)

// Using language constructs
let hasduplicates inputlist = 
    List.length inputlist = Set.count (set inputlist) |> not
(* hasduplicates ([1 .. 1000000]@[980397])
4442.176200
*)

// Using sorting
let hasduplicates (inputlist : 'a list) = 
    let rec loop = function
    | [] -> false
    | head::tail when head = List.head tail -> true
    | head::tail -> loop tail
    inputlist |> List.sort |> loop

(* hasduplicates ([1 .. 1000000]@[980397])
1589.325800
*)

// Using HashSet
open System
open System.Collections.Generic
let hasduplicates (inputlist : 'a list) =
    let hash = new HashSet<'a>()
    let rec loop (something : 'a list) = 
        if List.isEmpty something then false
        elif hash.Contains(List.head something) then true
        else 
            hash.Add(List.head something) |> ignore
            loop (List.tail something)
    loop inputlist

(* hasduplicates ([1 .. 1000000]@[980397])
1351.568200
*)

// Using dictionary
let hasduplicates (inputlist : 'a list) =
    let dict = new Dictionary<_,_>()
    let rec loop (something : 'a list) = 
        if List.isEmpty something then false
        elif dict.ContainsKey(List.head something) then true
        else 
            dict.Add(List.head something, 1) |> ignore
            loop (List.tail something)
    loop inputlist

(* hasduplicates ([1 .. 1000000]@[980397])
1036.464800
*)


// Running and timing
let stopWatch = System.Diagnostics.Stopwatch.StartNew()
hasduplicates ([1 .. 1000000]@[980397])
stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMilliseconds




