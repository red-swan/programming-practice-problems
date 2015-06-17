(*The first two consecutive numbers to have two distinct prime factors are:
14 = 2 × 7
15 = 3 × 5
The first three consecutive numbers to have three distinct prime factors are:
644 = 2² × 7 × 23
645 = 3 × 5 × 43
646 = 2 × 17 × 19.
Find the first four consecutive integers to have four distinct prime factors. What is the first of these numbers?*)

let distinctprimefactors n = 
  let rec loop c p =
    if c < (p * p) then [c]
    elif c % p = 0UL then p :: (loop (c/p) p)
    else loop c (p + 1UL)
  loop n 2UL
  |> Seq.distinct
  |> Seq.toList

let candidates = 
    let rec loop x = seq{ yield x; yield! loop (x+1UL)}
    loop 2UL
let stopWatch = System.Diagnostics.Stopwatch.StartNew()
let answer = 
    seq{1UL .. 1000000UL} 
    |> Seq.filter (fun x-> (distinctprimefactors x) |> List.length = 4)
    |> Seq.windowed 4
    |> Seq.filter (fun [|a;b;c;d|] -> a+1UL = b && a+2UL=c && a+3UL = d)
    |> Seq.head
    |> (fun x -> x.[0])
stopWatch.Stop()

// printfn "%f" stopWatch.Elapsed.TotalSeconds
// 1.124940
// val it : unit = ()
