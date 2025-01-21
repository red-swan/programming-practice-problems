(*
A perfect number is a number for which the sum of its proper divisors is exactly 
equal to the number. For example, the sum of the proper divisors of 28 would be 
1 + 2 + 4 + 7 + 14 = 28, which means that 28 is a perfect number.

A number n is called deficient if the sum of its proper divisors is less than n 
and it is called abundant if this sum exceeds n.

As 12 is the smallest abundant number, 1 + 2 + 3 + 4 + 6 = 16, the smallest 
number that can be written as the sum of two abundant numbers is 24. By 
mathematical analysis, it can be shown that all integers greater than 28123 can 
be written as the sum of two abundant numbers. However, this upper limit cannot 
be reduced any further by analysis even though it is known that the greatest 
number that cannot be expressed as the sum of two abundant numbers is less than 
this limit.

Find the sum of all the positive integers which cannot be written as the sum of 
two abundant numbers.
*)


// function to find sum of proper divisors
let propDivisorSum (number:int) = 
    let mutable output = 1
    let upper = number |> float |> sqrt |> int
    for candidate in 2 .. upper do
        if number % candidate = 0
        then if   candidate*candidate = number
             then output <- output + candidate
             else 
                  output <- output + candidate + (number / candidate)
    output

// tick
let stopWatch = System.Diagnostics.Stopwatch.StartNew()

// find all abundant numbers
let abundantNumbers = 
    [|1 .. 28123|]
    |> Array.filter (fun x -> x < (propDivisorSum x))

// create a dictionary of integers and if they're the sum of abundant numbers
let sumOfAbundants = new System.Collections.Generic.Dictionary<int, bool>(28123)

for i in 1 .. 28123 do
    sumOfAbundants.Add(i,false)

for i in abundantNumbers do 
    for j in abundantNumbers do
        if i + j > 28123 then ()
        else sumOfAbundants.[i+j] <- true

let answer = 
    sumOfAbundants
    |> Seq.sumBy (fun (KeyValue(value, boolean)) -> if boolean then 0 else value)

// tock
stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMilliseconds
// 600 ms

// The big takeaway from this is that it is sometimes easier to construct all 
// numbers of interest rather than searching for them