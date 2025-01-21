(*If p is the perimeter of a right angle triangle with integral length sides, {a,b,c}, there are exactly three solutions for p = 120.

{20,48,52}, {24,45,51}, {30,40,50}

For which value of p ≤ 1000, is the number of solutions maximised?*)


// redoing 39

open System

let gettriples s = 
    let mapitout kormore m  =
        let rec loop = function
        | [] -> []
        | k::tail -> [m;k-m;s/(2*m*k)] :: loop tail
        loop kormore
    let getabc [m;n;d] =
        let m2 = m*m
        let n2 = n*n
        ((m2-n2)*d,2*m*n*d,(m2+n2)*d)
    let ms = [1 .. int (sqrt (float s))] |> List.filter (fun x-> s/2 % x = 0)
    let findks m = [m+1 .. 2*m-1] |> List.filter (fun k -> k%2=1 && (s/(2*m))%k=0 && (gcd k m) = 1)
    let ks = ms |> List.map (fun m-> findks m)
    let mnds = List.map2 (fun m k-> mapitout k m) ms ks |> List.collect (fun x->x)
    List.map getabc mnds

let stopwatch = new System.Diagnostics.Stopwatch()
stopwatch.Start()
let answer =
    [3 .. 1000]
    |> List.maxBy (fun x-> gettriples x |> List.length)
stopwatch.Stop()
System.Console.WriteLine(stopwatch.ElapsedMilliseconds)









// OLD VERSION OLD VERSION OLD VERSION OLD VERSION OLD VERSION OLD VERSION OLD VERSION

// triples (if any) for a single perimeter and a single a
let getsolution perimeter a =
    let candidates = [1 .. (perimeter - a)]
    let rec loop = function
    | [] -> (0,0,0)
    | b::tail when a*a+b*b= (perimeter-a-b)*(perimeter-a-b) -> (a,b,perimeter-a-b)
    | b::tail -> loop tail
    loop candidates
// This gets all triples for a given perimeter
// THIS RETURNS DUPLICATES
let gettriples perimeter = 
    let candidates = [1 .. 1+ perimeter/2]
    let rec loop = function
        | [] -> []
        | head::tail -> getsolution perimeter head :: loop tail
    loop candidates |> List.filter (fun (a,_,_) -> a<>0)

let answer =
    [1 .. 1000]
    |> List.map gettriples
    |> List.filter (fun x-> List.isEmpty x |> not)
    |> List.maxBy (fun x-> List.length x)
    |> List.head
    |> (fun (a,b,c) -> a+b+c)

// Simple once I did 44, could easily be optimized by not returning empty lists or removing duplicates


