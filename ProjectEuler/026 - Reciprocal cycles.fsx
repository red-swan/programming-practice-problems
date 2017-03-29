(*
A unit fraction contains 1 in the numerator. The decimal representation of the 
unit fractions with denominators 2 to 10 are given:

1/2	= 	0.5
1/3	= 	0.(3)
1/4	= 	0.25
1/5	= 	0.2
1/6	= 	0.1(6)
1/7	= 	0.(142857)
1/8	= 	0.125
1/9	= 	0.(1)
1/10	= 	0.1
Where 0.1(6) means 0.166666..., and has a 1-digit recurring cycle. It can be seen 
that 1/7 has a 6-digit recurring cycle.

Find the value of d < 1000 for which 1/d contains the longest recurring cycle in 
its decimal fraction part.
*)


let remainders num den = 
    let nextRemainder x = 
        if x = 0 
        then None
        else let rmdr = x % den 
             Some(rmdr, rmdr*10)

    Seq.unfold nextRemainder num

// we cheat and include the terminating ones as well because we know that
// they will never be as long as the prime ones
// https://en.wikipedia.org/wiki/Repeating_decimal#Fractions_with_prime_denominators
let cycleLength num den = 
    let remainderSet = System.Collections.Generic.HashSet<int>()
    (remainders num den)
    |> Seq.takeWhile (fun x -> remainderSet.Add(x))
    |> Seq.length


let stopWatch = System.Diagnostics.Stopwatch.StartNew()
let answer = 
    [1 .. 1000]
    |> Seq.maxBy (cycleLength 1)
stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMilliseconds
// 16ms