(*Let p(n) represent the number of different ways in which n coins can be separated into piles. \
For example, five coins can be separated into piles in exactly seven different ways, so p(5)=7.

OOOOO
OOOO   O
OOO   OO
OOO   O   O
OO   OO   O
OO   O   O   O
O   O   O   O   O
Find the least value of n for which p(n) is divisible by one million.*)




let sumdiff thelist = Seq.fold (fun acc elem -> if acc ) 1 thelist

///////////////////////////////////////////////////////////////////////////
// TUPLES TUPLES TUPLES TUPLES TUPLES TUPLES TUPLES TUPLES TUPLES TUPLES 


let pentagonalnumbers = Seq.unfold (fun n -> if n > 0 then Some((n,(3*n*n-n)/2),-1*n) else Some((n,(3*n*n-n)/2),1+ -1*n)) 1 |> Seq.cache



let pbelown n = pentagonalnumbers |> Seq.takeWhile (fun (_,elem) -> elem < n)
let p (n : int) = 
    let psupton = pentagonalnumbers |> Seq.take (n-1)
    let termstosum = pbelown n
    Seq.map (fun (_,x) -> (Seq.nth x psupton)) termstosum |> Seq.map (fun (k,x) -> )



///////////////////////////////////////////////////////////////////////////
// SINGLES SINGLES SINGLES SINGLES SINGLES SINGLES SINGLES SINGLES 

let pentagonalnumbers = Seq.unfold (fun n -> if n > 0 then Some((3*n*n-n)/2,-1*n) else Some((3*n*n-n)/2,1+ -1*n)) 1 |> Seq.cache


let getindexes n = 
    Seq.takeWhile (fun x -> x<n) pentagonalnumbers
    |> Seq.map (fun x-> n-x)
    |> Seq.toList

let sumdiff listofnumbers = Seq.mapi (fun i x -> -sign(double(i % 4) - 1.9)*x) listofnumbers |> Seq.sum



/////////////////////////////////////////
// SCRATCH SCRATCH
/////////////////////////////////////////

let temp inputlist = Seq.map2 (fun n-> if n >0 then Some((1.0 ** (k-1.0))*x,-1.0*n) else Some((1.0 ** (k-1.0))*x,1+ -1*n)) [1 .. (List.length inputlist)] inputlist
let ones n = 
    seq {for x in 1 .. (1+n/2) do if x % 2 = 1 then yield 1; yield 1; else yield -1; yield -1}
    |> Seq.take n
let rec loop n = 
    seq{}

seq {for i in 1 .. 4 do if i%2=0 then yield 1; yield 1 else yield -1; yield -1};;

//completely unnecessary
Seq.unfold (fun n-> if n > 0 then Some(int (-1.0 ** (float n-1.0)),-1*n) else Some(int (-1.0 ** (float n-1.0)),1+(-1*n))) 1

// DING DING we have a winner
let sumdiff listofnumbers = List.mapi (fun i x -> -sign(double(i % 4) - 1.9)*x) listofnumbers |> List.sum

let foo l = l |> Seq.fold (fun (n,k,r) value ->
  let n, k = if n = 1 then 0, -1*k else n+1, k
  printfn "%d" (value*k)
  n, k, r + (value * k)) (-1, 1, 0)


let temp x = (1.0+x)/(1.0+x*x)

// Best way to get seq of 1's and -1's
let get1s n = 
    let rec loop = seq { yield 1; yield 1; yield -1; yield -1; yield! loop}
    loop |> Seq.take n
////////////////////////////////////////////////////////////////
let pentagonalseq = 
    Seq.unfold (fun n -> if n > 0 then Some((3*n*n-n)/2,-1*n) else Some((3*n*n-n)/2,1+ -1*n)) 1 
    |> Seq.cache

let getindexes n = 
    Seq.takeWhile (fun x -> x<n) pentagonalseq
    |> Seq.map (fun x-> n-x)
    |> Seq.toList

let get1s n = 
    let rec loop = seq { yield 1; yield 1; yield -1; yield -1; yield! loop}
    loop |> Seq.take n

let rec pent =
    let dict = new System.Collections.Generic.Dictionary<_,_>()
    fun n ->
        match dict.TryGetValue(n) with
        | true, v -> v
        | false, _ -> 
            let temp = (3*n*n-n) / 2
            dict.Add(n, temp)
            temp 

let integerpartitions = 
    let dict = new System.Collections.Generic.Dictionary<_,_>()
    fun n ->
        match dict.TryGetValue(n) with
        | true, v -> v
        | false, _ -> 
            let temp = getindexes(n) |> Seq.map pent |> Seq.map2 (fun x y -> x*y) (get1s n) |> Seq.sum
            dict.Add(n, temp)
            temp 

// need to match indices right, but should work quickly


[-10 .. 10] |> Seq.map pent |> Seq.toList |> List.sort
