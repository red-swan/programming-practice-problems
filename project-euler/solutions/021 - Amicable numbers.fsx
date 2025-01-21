(*
Let d(n) be defined as the sum of proper divisors of n (numbers less than n 
which divide evenly into n).
If d(a) = b and d(b) = a, where a ≠ b, then a and b are an amicable pair and 
each of a and b are called amicable numbers.

For example, the proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 
and 110; therefore d(220) = 284. The proper divisors of 284 are 1, 2, 4, 71 
and 142; so d(284) = 220.

Evaluate the sum of all the amicable numbers under 10000.
*)

let properDivisors myInt = 
    let upper = myInt |> float |> sqrt |> int
    let bottoms = 
        [2 .. upper]
        |> List.filter (fun x -> myInt % x = 0)
    let uppers = 
        bottoms
        |> List.map (fun x -> myInt / x)
    [1] @ bottoms @ uppers
    |> List.distinct

let d n = 
    n
    |> properDivisors
    |> List.sum 

let answer = 
    [1 .. 10000]
    |> List.filter (fun x -> x |> d |> d |> ((=)x))
    |> List.filter (fun x -> x |> d |> (<>)x)
    |> List.sum