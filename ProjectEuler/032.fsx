(*We shall say that an n-digit number is pandigital if it makes use of all the digits 1 to n exactly once;
for example, the 5-digit number, 15234, is 1 through 5 pandigital.
The product 7254 is unusual, as the identity, 39 × 186 = 7254, containing multiplicand, multiplier,
and product is 1 through 9 pandigital.
Find the sum of all products whose multiplicand/multiplier/product identity can be written as a 1 through 9 pandigital.
HINT: Some products can be obtained in more than one way so be sure to only include it once in your sum.*)
(* Could get all perumations of 123456789 and run through them all checking for a*b=c at some portion
362,880 different ways to get permute 123456789 (i.e. 9!)
28 possible placements of * and = (i.e. Sum(1:7)) -> THIS CAN BE WHITTLED DOWN
10,160,640 possibilities *)

//	FUNCTION TO PERMUTE
#load "Tools.fs"

open Tools

let permute myList = permutations (List.length myList) myList

// FUNCTION TO GET DIGITS
let digits number =  [for c in (string number) -> c] |> List.map (fun x -> (int x) - 48)

// FUNCTIONS TO MANIPULATE THE LIST OF DIGITS
let lastfour (mylist : int list) = mylist.[8] + mylist.[7]*10 + mylist.[6]*100 + mylist.[5]*1000
let firsttwo (mylist : int list) = mylist.[1] + mylist.[0]*10
let firstone (mylist : int list) = mylist.[0]
let nextthree (mylist : int list) = mylist.[4] + mylist.[3]*10 + mylist.[2]*100
let nextfour (mylist : int list) = mylist.[4] + mylist.[3]*10 + mylist.[2]*100 + mylist.[1]*1000
let onexfour listofdigits = (firstone listofdigits) * (nextfour listofdigits) = (lastfour listofdigits)
let twoxthree listofdigits = (firsttwo listofdigits) * (nextthree listofdigits) = (lastfour listofdigits)

let answer =
    digits 123456789
    |> permute
    |> List.filter (fun x-> twoxthree x || onexfour x)
    |> List.map lastfour
    |> Seq.distinct
    |> Seq.sum


// BELOW HERE IS SCRATCH

let cd a b = 
    let first = digits a |> List.length
    let second = digits b |> List.length
    let third = digits (a*b) |> List.length
    first+second+third
// low low
cd 2 1345
// low high
cd 2 4987
// high low = high high
cd 8 1234
// ^ if first number is 1 digit, it is between 2 & 8 and leads 4&4 
cd 12 798
cd 81 123
// first is two digits, followed by 3&3, constrained first digit between 1-8
cd 123 79
cd 798 12
    // first is three digits, followed by 2&3, constained first digit between 1-7
cd 1234 8
// high low
cd 4987 2
// high high

    // first is four digits, followed by 1&5, constrained first digit between 1-4
cd 12345

1x4=4
2x3=4
3x2=4
4x1=4
// Notice that the bottom two cases yield the same product as the upper two cases, we only need to check two cases then: 1x4 & 2x3




let test = digits 391867254
firsttwo test
nextthree test
lastthree test
let test = digits 294358716
firstone test
nextfour test
lastfour test
