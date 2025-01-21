

[<AutoOpen>]
module PrimeNumbers = 
    open System.Numerics

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


    // Miller-Rabin Primality Check ------------------------------------------------

    ///This implementation is based on the Miller-Rabin Haskell implementation 
    ///from http://www.haskell.org/haskellwiki/Testing_primality
    let private pow' mul sq x' n' = 
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
        
    let private mulMod (a :bigint) b c = (b * c) % a
    let private squareMod (a :bigint) b = (b * b) % a
    let private powMod m = pow' (mulMod m) (squareMod m)
    let private iterate f = Seq.unfold(fun x -> let fx = f x in Some(x,fx))

    ///See: http://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test
    let private millerRabinPrimality n a =
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
    let private isPrimeW witnesses = function
        | n when n < 2I      -> false
        | n when n = 2I 
              || n = 3I 
              || n = 5I
              || n = 7I      -> true
        | n when n % 2I = 0I -> false
        | n                  -> witnesses |> Seq.forall(millerRabinPrimality n)

    let isPrime = isPrimeW [2I; 3I; 5I; 7I]
    let isPrimeInt (integer:int) = isPrime (BigInteger(integer))