[<AutoOpen>]
module NumberTheory = 
    // Greatest Common Divisor -----------------------------------------------------
    let rec gcd x y =
        if y = 0 then abs x
        else gcd y (x % y)

    // Least Common Multiple -------------------------------------------------------
    let lcm x y = x * y / (gcd x y)

    // Number of trailing zeroes
    let ntz (intg : int) = 
        let mutable i = intg
        let mutable z = 1
        if i &&& 0xffff = 0 then do
            z <- z + 16
            i <- i >>> 16
        if i &&& 0x00ff = 0 then do
            z <- z + 8
            i <- i >>> 8
        if i &&& 0x000f = 0 then do
            z <- z + 4
            i <- i >>> 4
        if i &&& 0x0003 = 0 then do
            z <- z + 2
            i <- i >>> 2
        z - (i &&& 1)

    // Extended Euclidean Algorithm
    let eea a b =
        let rec loop rOld sOld tOld rCurr sCurr tCurr = 
            if rCurr = 0 then (sOld,tOld)
            else 
                let q = rOld / rCurr
                loop rCurr sCurr tCurr (rOld - q*rCurr) (sOld - q*sCurr) (tOld - q*tCurr)
        loop a 1 0 b 0 1


module Diophantine =
    let solveLinear a b c=
        let g = gcd a b
        if c % g <> 0 then failwith "No Integer Solutions"
        let s,t = eea a b
        seq{ yield (s,t); yield (-s,t); yield (-s,-t); yield (s,-t)}
        |> Seq.find (fun (s,t) -> a*s+b*t = g)


module ContinuedFraction =
    // For Rational numbers only
    type T = T of seq<int>

    let fromFraction (p : int) (q : int) =
        let rec loop p q = 
            match q with
            | 0 -> Seq.empty
            | _ -> let a = p/q
                   let b = p - q*a
                //    printfn "P:%A\tQ:%A" p q
                   seq{ yield p / q; yield! loop q b }
        loop p q
        |> Seq.toList

    // pi should be approximated by 355 / 113
    // or [3;7;15;1]
    // fromFraction 415 93
    
    // fromFraction 355 113
    // |> Seq.toList


    // let fromFloat (num : float) = 
    //     let rec loop (m,d,a) = 
    //         let m' = d*a-m
    //         let d' = (number - (m'*m'))/d
    //         let a' = int (float (a0 + m') / float d')
    //         seq { yield (int a'); yield! loop (m',d',a')}
    //     seq { yield (int a0); yield! (loop (0I,1I,a0)) }

(*  Used to find continued fractions of sqrts only

    let continuedFraction (number : BigInteger) = 
        let a0 = number |> float |> sqrt |> BigInteger
        let rec loop (m,d,a) = 
            let m' = d*a-m
            let d' = (number - (m'*m'))/d
            let a' = BigInteger (float (a0 + m') / float d')
            seq { yield (int a'); yield! loop (m',d',a')}
        seq { yield (int a0); yield! (loop (0I,1I,a0)) }
*)