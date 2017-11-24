(*
All square roots are periodic when written as continued fractions and can be written in the form:

√N = a0 +	
1
 	a1 +	
1
 	 	a2 +	
1
 	 	 	a3 + ...
For example, let us consider √23:

√23 = 4 + √23 — 4 = 4 + 	
1
 = 4 + 	
1
 	
1
√23—4
 	1 + 	
√23 – 3
7
If we continue we would get the following expansion:

√23 = 4 +	
1
 	1 +	
1
 	 	3 +	
1
 	 	 	1 +	
1
 	 	 	 	8 + ...
The process can be summarised as follows:

a0 = 4,	 	
1
√23—4
 = 	
√23+4
7
 = 1 + 	
√23—3
7
a1 = 1,	 	
7
√23—3
 = 	
7(√23+3)
14
 = 3 + 	
√23—3
2
a2 = 3,	 	
2
√23—3
 = 	
2(√23+3)
14
 = 1 + 	
√23—4
7
a3 = 1,	 	
7
√23—4
 = 	
7(√23+4)
7
 = 8 + 	√23—4
a4 = 8,	 	
1
√23—4
 = 	
√23+4
7
 = 1 + 	
√23—3
7
a5 = 1,	 	
7
√23—3
 = 	
7(√23+3)
14
 = 3 + 	
√23—3
2
a6 = 3,	 	
2
√23—3
 = 	
2(√23+3)
14
 = 1 + 	
√23—4
7
a7 = 1,	 	
7
√23—4
 = 	
7(√23+4)
7
 = 8 + 	√23—4
It can be seen that the sequence is repeating. For conciseness, we use the 
notation √23 = [4;(1,3,1,8)], to indicate that the block (1,3,1,8) repeats 
indefinitely.

The first ten continued fraction representations of (irrational) square 
roots are:

√2=[1;(2)], period=1
√3=[1;(1,2)], period=2
√5=[2;(4)], period=1
√6=[2;(2,4)], period=2
√7=[2;(1,1,1,4)], period=4
√8=[2;(1,4)], period=2
√10=[3;(6)], period=1
√11=[3;(3,6)], period=2
√12= [3;(2,6)], period=2
√13=[3;(1,1,1,1,6)], period=5

Exactly four continued fractions, for N ≤ 13, have an odd period.

How many continued fractions for N ≤ 10000 have an odd period?
*)

// https://math.stackexchange.com/questions/90406/how-to-detect-when-continued-fractions-period-terminates

open System.Numerics

let squareNumbers = 
    seq { for i in 2I .. 100I do yield i*i}
    |> Set.ofSeq

let notSquare number = 
    not (Set.contains number squareNumbers)

let continuedFraction (number : BigInteger) = 
    let a0 = number |> float |> sqrt |> BigInteger
    let rec loop (m,d,a) = 
        let m' = d*a-m
        let d' = (number - (m'*m'))/d
        let a' = BigInteger (float (a0 + m') / float d')
        seq { yield (int a'); yield! loop (m',d',a')}
    seq { yield (int a0); yield! (loop (0I,1I,a0)) }

let periodicity num = 
    let cfSeq = continuedFraction num
    let head = Seq.head cfSeq
    cfSeq
    |> Seq.takeWhile (fun x -> x <> 2*head)
    |> Seq.length

let answer = 
    [2I .. 10000I] 
    |> Seq.filter notSquare
    |> Seq.map periodicity
    |> Seq.filter (fun x -> x % 2 = 1)
    |> Seq.length

