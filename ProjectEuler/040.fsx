(*An irrational decimal fraction is created by concatenating the positive integers:

0.123456789101112131415161718192021...

It can be seen that the 12th digit of the fractional part is 1.

If dn represents the nth digit of the fractional part, find the value of the following expression.

d1 × d10 × d100 × d1000 × d10000 × d100000 × d1000000*)

open System

// integer -> char list of digits of the input
let digits number =  seq {for c in (string number) -> c} |> Seq.map (fun x -> (int x) - (int '0'))  
// This is the sequence of decimals
let irrationaldecimal = Seq.unfold (fun (x) -> Some(digits x,x+1)) 1 |> Seq.collect (fun x->x)
// this is the function to get the nth element of the decimal
// index starts at 0 so we subtract one
let d n = Seq.nth (n-1) irrationaldecimal

let start = DateTimeOffset.Now
let answer = (d 1) * (d 10) * (d 100) * (d 1000) * (d 10000) * (d 100000) * (d 1000000)
let stop = DateTimeOffset.Now
stop-start
