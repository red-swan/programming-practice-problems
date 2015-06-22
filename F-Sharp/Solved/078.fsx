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

let pentagonalseq = 
    Seq.unfold (fun n -> if n > 0I then Some((3I*n*n-n)/2I,-1I*n) else Some((3I*n*n-n)/2I,1I+ -1I*n)) 1I
    |> Seq.cache

let getindexes n = 
    Seq.takeWhile (fun x -> x<=n) pentagonalseq
    |> Seq.map (fun x-> n-x)
    |> Seq.toList

let get1s n = 
    let rec loop = seq { yield 1I; yield 1I; yield -1I; yield -1I; yield! loop}
    loop |> Seq.take (int n)

let rec integerpartitions = 
    let dict = new System.Collections.Generic.Dictionary<_,_>()
    fun n ->
        match dict.TryGetValue(n) with
        | true, v -> v
        | false, _ -> 
            let temp = 
                if n = 0I || n=1I then 1I
                else getindexes(n) |> Seq.map integerpartitions |> Seq.map2 (fun x y -> x*y) (get1s n) |> Seq.sum
            dict.Add(n, temp)
            temp

let naturals = 
    Seq.unfold (fun x-> Some(x,x+1I)) 1I
    |> Seq.cache

let answer = 
    naturals
    |> Seq.find (fun x -> (integerpartitions x) % 1000000I = 0I)
