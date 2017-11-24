namespace ProjectEuler

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