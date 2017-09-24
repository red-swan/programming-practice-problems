#I @"C:\Users\jdks\Github\Project-Euler-Solutions\packages\MathNet.Numerics.FSharp.3.13.1\lib\net40\"
#I @"C:\Users\jdks\Github\Project-Euler-Solutions\packages\MathNet.Numerics.3.13.1\lib\net40\"

#r "MathNet.Numerics.dll"
#r "MathNet.Numerics.FSharp.dll"

open System
open System.Numerics
open MathNet.Numerics


// how to use the stopwatch ----------------------------------------------------
let stopWatch = System.Diagnostics.Stopwatch.StartNew()
//...
stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMilliseconds

// compute number of combinations of k elements from a set of n ----------------
let nextPascalRow pascalRow = 
    [0I] @ pascalRow @ [0I]
    |> List.pairwise
    |> List.map (fun (a,b) -> a+b)

let pascalRow n = 
    let rec loop iteration pascalRow =  
        match iteration with 
        | value when value = n -> pascalRow
        | _ -> pascalRow |> nextPascalRow |> loop (iteration + 1)
    loop 1 [1I]

let choose n k = 
    (n+1)
    |> pascalRow
    |> List.item k




//let choose(n:int) (k:int) = 
//    let n',k' = BigRational.FromInt(n),BigRational.FromInt(k)
//    Array.init k (fun i -> (n' - k' + BigRational.FromInt(i) + 1N) / (BigRational.FromInt(i) + 1N) )
//    |> Array.fold (fun acc elem -> acc*elem) 1N
//    |> BigRational.ToBigInt

// memoize a function ----------------------------------------------------------
let memoize f =
    let dict = new System.Collections.Generic.Dictionary<_,_>()
    fun n ->
        match dict.TryGetValue(n) with
        | (true, v) -> v
        | _ ->
            let temp = f(n)
            dict.Add(n, temp)
            temp

// memoized factorial ----------------------------------------------------------
let rec factorial = memoize(fun n -> 
    if n = 0I then 1I
    else (n-1I) |> factorial |> ((*)n))


// Generates a list of all primes below limit ----------------------------------
let sieveOfAtkin limit =
    // initialize the sieve
    let sieve = Array.create (limit + 1) false
    // put in candidate primes: 
    // integers which have an odd number of
    // representations by certain quadratic forms
    let inline invCand n pred =
        if n < limit && pred then sieve.[n] <- not sieve.[n] 
    let sqrtLimit = int (sqrt (float limit))
    for x = 1 to sqrtLimit do
        for y = 1 to sqrtLimit do
            let xPow2 = x * x
            let yPow2 = y * y
            let n = 4 * xPow2 + yPow2 in invCand n (let m = n % 12 in m = 1 || m = 5)
            let n = 3 * xPow2 + yPow2 in invCand n (n % 12 = 7)
            let n = 3 * xPow2 - yPow2 in invCand n (x > y && n % 12 = 11)
    // eliminate composites by sieving
    let rec eliminate n =
        if n <= sqrtLimit 
        then if sieve.[n]
             then let nPow2 = n * n
                  for k in nPow2 .. nPow2 .. limit do
                      Array.set sieve k false
             eliminate (n + 2)
    eliminate 5
    // Generate list from the sieve (backwards)
    let rec generateList acc n =
        if n >= 5 then generateList (if sieve.[n] then n :: acc else acc) (n - 1)
        else acc
    2 :: 3 :: (generateList [] limit)

// Permute Elements in a list --------------------------------------------------
//let rec distribute e = function
//    | [] -> [[e]]
//    | x::xs' as xs -> (e::xs)::[for xs in distribute e xs' -> x::xs]
//
//let rec permute = function
//    | [] -> [[]]
//    | e::xs -> List.collect (distribute e) (permute xs)
// Get combinations of a list --------------------------------------------------
// SEE BELOW
//let rec combinations' acc (size:int) (set: 'a list) = seq {
//  match size, set with 
//  | n, x::xs -> 
//      if n > 0 then yield! combinations' (x::acc) (n - 1) xs
//      if n >= 0 then yield! combinations' acc n xs 
//  | 0, [] -> yield acc 
//  | _, [] -> () }
//
//let combinations size set = combinations' [] size set

// Miller-Rabin Primality Check ------------------------------------------------


///This implementation is based on the Miller-Rabin Haskell implementation 
///from http://www.haskell.org/haskellwiki/Testing_primality
let pow' mul sq x' n' = 
    let rec f x n y = 
        if n = 1I then
            mul x y
        else
            let (q,r) = BigInteger.DivRem(n, 2I)
            let x2 = sq x
            if r = 0I then
                f x2 q y
            else
                f x2 q (mul x y)
    f x' n' 1I
    
let mulMod (a :bigint) b c = (b * c) % a
let squareMod (a :bigint) b = (b * b) % a
let powMod m = pow' (mulMod m) (squareMod m)
let iterate f = Seq.unfold(fun x -> let fx = f x in Some(x,fx))

///See: http://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test
let millerRabinPrimality n a =
    let find2km n = 
        let rec f k m = 
            let (q,r) = BigInteger.DivRem(m, 2I)
            if r = 1I then
                (k,m)
            else
                f (k+1I) q
        f 0I n
    let n' = n - 1I
    let iter = Seq.tryPick(fun x -> if x = 1I then Some(false) elif x = n' then Some(true) else None)
    let (k,m) = find2km n'
    let b0 = powMod n a m

    match (a,n) with
        | _ when a <= 1I && a >= n' -> 
            failwith (sprintf "millerRabinPrimality: a out of range (%A for %A)" a n)
        | _ when b0 = 1I || b0 = n' -> true
        | _  -> b0 
                 |> iterate (squareMod n) 
                 |> Seq.take(int k)
                 |> Seq.skip 1 
                 |> iter 
                 |> Option.exists id 

// +!+ TODO: Memoize this function
///For Miller-Rabin the witnesses need to be selected at random from the interval [2, n - 2]. 
///More witnesses => better accuracy of the test.
///Also, remember that if Miller-Rabin returns true, then the number is _probable_ prime. 
///If it returns false the number is composite.
let isPrimeW witnesses = function
    | n when n < 2I      -> false
    | n when n = 2I 
          || n = 3I 
          || n = 5I
          || n = 7I      -> true
    | n when n % 2I = 0I -> false
    | n                  -> witnesses |> Seq.forall(millerRabinPrimality n)

let isPrime = isPrimeW [2I; 3I; 5I; 7I]
let isPrimeInt (integer:int) = isPrime (BigInteger(integer))

// Greatest Common Divisor -----------------------------------------------------
let rec gcd x y =
    if y = 0 then abs x
    else gcd y (x % y)

// Least Common Multiple -------------------------------------------------------
let lcm x y = x * y / (gcd x y)

// Cartesian Product -----------------------------------------------------------
let cartesian xs ys = xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))

// Sequence Enumerator ---------------------------------------------------------
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



// perm and comb ???????????????????????????????????????????????
type ListBuilder() =
  let concatMap f m = List.concat( List.map (fun x -> f x) m )
  member this.Bind (m, f) = concatMap (fun x -> f x) m 
  member this.Return (x) = [x]
  member this.ReturnFrom (x) = x
  member this.Zero () = []
  member this.Combine (a,b) = a@b
  member this.Delay f = f ()

let list = ListBuilder()

let rec permutations n lst = 
  let rec selections = function
      | []      -> []
      | x::xs -> (x,xs) :: list { let! y,ys = selections xs 
                                  return y,x::ys }
  (n, lst) |> function
  | 0, _ -> [[]]
  | _, [] -> []
  | _, x::[] -> [[x]]
  | n, xs -> list { let! y,ys = selections xs
                    let! zs = permutations (n-1) ys 
                    return y::zs }

let rec combinations n lst = 
  let rec findChoices = function 
    | []    -> [] 
    | x::xs -> (x,xs) :: list { let! y,ys = findChoices xs 
                                return y,ys } 
  list { if n = 0 then return! [[]]
         else
           let! z,r = findChoices lst
           let! zs = combinations (n-1) r 
           return z::zs }


// create an infinite Sequence from any Ienumerable object ---------------------
module Seq =
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