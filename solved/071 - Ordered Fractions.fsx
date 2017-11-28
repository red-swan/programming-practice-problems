(*
Consider the fraction, n/d, where n and d are positive integers. 
If n<d and HCF(n,d)=1, it is called a reduced proper fraction.

If we list the set of reduced proper fractions for d ≤ 8 in 
ascending order of size, we get:

1/8, 1/7, 1/6, 1/5, 1/4, 2/7, 1/3, 3/8, 2/5, 3/7, 1/2, 
4/7, 3/5, 5/8, 2/3, 5/7, 3/4, 4/5, 5/6, 6/7, 7/8

It can be seen that 2/5 is the fraction immediately to the left of 3/7.

By listing the set of reduced proper fractions for d ≤ 1,000,000 
in ascending order of size, find the numerator of the fraction 
immediately to the left of 3/7.
*)

#load "../support/NumberTheory.fs"
open ProjectEuler
let upperLimit = int(1e6)

// Neighbors in a Farey sequence have a property
// if a/b < c/d are Farey pairs, then bc - ad = 1
// We can find all neighbors across all Farey sequences
// by solving the above linear diophantine equation
// We generate these solutions and then pick the one
// with the largest denominator under 1e6
let answer = 
    let x,y = 3,7
    let s,t = Diophantine.solveLinear x -y 1
    Seq.unfold (fun n -> Some((t + x*n, s + y*n), n + 1)) 0
    |> Seq.takeWhile (fun (_,q) -> q < upperLimit)
    |> Seq.maxBy snd
    // 0.015s
   
// Another method is to find the closest fraction
// for every d we're going to check
// c/d < a/b => c < da/b => p <= floor(d(a-1)/b)
let answerAlt = 
    let a,b = 3,7 
    [1 .. upperLimit]
    |> List.maxBy (fun d -> float((d*a-1) / b) / float(d))
    |> (fun d -> ((d*a-1) / b,d))
    // 0.150s



// Resources
// https://en.wikipedia.org/wiki/Farey_sequence
// https://en.wikipedia.org/wiki/Stern%E2%80%93Brocot_tree
// https://www.math.uwaterloo.ca/~snburris/cgi-bin/linear-query