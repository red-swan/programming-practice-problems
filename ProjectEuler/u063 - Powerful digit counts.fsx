(*
The 5-digit number, 16807=7^5, is also a fifth power. Similarly, the 9-digit 
number, 134217728=8^9, is a ninth power.

How many n-digit positive integers exist which are also an nth power?
*)





[2 .. 9] |> List.map (fun x -> pown 9 x)


// ninth power upper limit is 9
// eighth power upper limit is
// x^8 = 10 ^ 8 
