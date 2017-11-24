(*
The 5-digit number, 16807=7^5, is also a fifth power. Similarly, the 9-digit 
number, 134217728=8^9, is a ninth power.

How many n-digit positive integers exist which are also an nth power?
*)


let rec BigIntDigitCount' digits (number : System.Numerics.BigInteger) =
    if number = 0I then digits
    else BigIntDigitCount' (digits + 1) (number / 10I)

let BigIntDigitCount number = BigIntDigitCount' 0 number

let BigPow number x = 
    System.Numerics.BigInteger.Pow(number, x)

let digitSeq (number) = 
    Seq.unfold (fun x -> Some(x,x+1I)) 1I
    |> Seq.skipWhile (fun x -> BigIntDigitCount (BigPow x number) < number)
    |> Seq.takeWhile (fun x -> BigIntDigitCount (BigPow x number) = number)
    |> Seq.map (fun x -> (x,pown x number))

#time
let answer = 
    Seq.unfold (fun x -> Some(x,x+1)) 1
    |> Seq.map digitSeq
    |> Seq.takeWhile (fun x -> Seq.length x > 0)
    |> Seq.collect id
    |> Seq.map snd
    |> Seq.distinct
    |> Seq.length
#time // 0.004 seconds
