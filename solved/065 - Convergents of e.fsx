(*
The square root of 2 can be written as an infinite continued fraction.

√2 = 1 +	
1
 	2 +	
1
 	 	2 +	
1
 	 	 	2 +	
1
 	 	 	 	2 + ...
The infinite continued fraction can be written, √2 = [1;(2)], (2) indicates that 2 repeats ad infinitum. In a similar way, √23 = [4;(1,3,1,8)].

It turns out that the sequence of partial values of continued fractions for 
square roots provide the best rational approximations. Let us consider the 
convergents for √2.

1 +	
1
= 3/2
 	
2
 
1 +	
1
= 7/5
 	2 +	
1
 	 	
2
 
1 +	
1
= 17/12
 	2 +	
1
 
 	 	2 +	
1
 
 	 	 	
2
 
1 +	
1
= 41/29
 	2 +	
1
 	 	2 +	
1
 
 	 	 	2 +	
1
 
 	 	 	 	
2
 
Hence the sequence of the first ten convergents for √2 are:

1, 3/2, 7/5, 17/12, 41/29, 99/70, 239/169, 577/408, 1393/985, 3363/2378, ...
What is most surprising is that the important mathematical constant,
e = [2; 1,2,1, 1,4,1, 1,6,1 , ... , 1,2k,1, ...].

The first ten terms in the sequence of convergents for e are:

2, 3, 8/3, 11/4, 19/7, 87/32, 106/39, 193/71, 1264/465, 1457/536, ...
The sum of digits in the numerator of the 10th convergent is 1+4+5+7=17.

Find the sum of digits in the numerator of the 100th convergent of the continued fraction for e.
*)

open System.Numerics

let convergents (intSeq : seq<BigInteger>) = 
    intSeq
    |> Seq.scan (fun (p2,q2,p1,q1) elem -> (elem*p2+p1, elem*q2+q1, p2,q2) ) (1I,0I,0I,1I) 
    |> Seq.map (fun (p2,q2,_,_) -> (p2,q2))

let continuedFractionE = 
    let rec loop (n : int) = 
        seq { yield 2*n; yield 1; yield 1; yield! (loop (n+1)) }
    seq{ yield 2; yield 1; yield! (loop 1)}

let answer = 
    continuedFractionE
    |> Seq.map BigInteger
    |> convergents
    |> Seq.item 100
    |> (fun (p,q) -> p.ToString().ToCharArray())
    |> Array.map (fun x -> System.Int32.Parse(string x))
    |> Array.sum

