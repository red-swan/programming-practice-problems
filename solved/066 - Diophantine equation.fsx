(*
Consider quadratic Diophantine equations of the form:

x^2 – D*y^2 = 1

For example, when D=13, the minimal solution in x is 649^2 – 13×180^2 = 1.

It can be assumed that there are no solutions in positive integers when D is square.

By finding minimal solutions in x for D = {2, 3, 5, 6, 7}, we obtain the following:

3^2 – 2*2^2 = 1
2^2 – 3*1^2 = 1
9^2 – 5*4^2 = 1
5^2 – 6*2^2 = 1
8^2 – 7*3^2 = 1

Hence, by considering minimal solutions in x for D ≤ 7, the largest x is obtained when D=5.

Find the value of D ≤ 1000 in minimal solutions of x for which the largest value of x is obtained.
*)

open System.Numerics


let continuedFraction (number : BigInteger) = 
    let a0 = number |> float |> sqrt |> BigInteger
    let rec loop (m,d,a) = 
        let m' = d*a-m
        let d' = (number - (m'*m'))/d
        let a' = BigInteger (float (a0 + m') / float d')
        seq { yield a'; yield! loop (m',d',a')}
    seq { yield a0; yield! loop (0I,1I,a0) }

let convergents (intSeq : seq<BigInteger>) = 
    intSeq
    |> Seq.scan (fun (p2,q2,p1,q1) elem -> (elem*p2+p1, elem*q2+q1, p2,q2) ) (1I,0I,0I,1I) 
    |> Seq.map (fun (p2,q2,_,_) -> (p2,q2))

let SolvePells D =
    D
    |> continuedFraction
    |> convergents
    |> Seq.skip 1
    |> Seq.find (fun (x,y) -> x*x - D*y*y = 1I) 

let squareNumbers = 
    seq { for i in 2I .. 1000I do yield i*i}
    |> Set.ofSeq

let notSquare number = 
    not (Set.contains number squareNumbers)

let answer = 
    [2I .. 1000I] 
    |> Seq.filter notSquare
    |> Seq.map (fun x -> (x,SolvePells x))
    |> Seq.maxBy (fun (D,(x,y)) -> x)