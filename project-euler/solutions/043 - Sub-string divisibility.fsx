(*The number, 1406357289, is a 0 to 9 pandigital number because it is made up of each of the 
digits 0 to 9 in some order, but it also has a rather interesting sub-string divisibility property.

Let d_1 be the 1st digit, d_2 be the 2nd digit, and so on. In this way, we note the following:

d_2d_3d_4=406 is divisible by 2
d_3d_4d_5=063 is divisible by 3
d_4d_5d_6=635 is divisible by 5
d_5d_6d_7=357 is divisible by 7
d_6d_7d_8=572 is divisible by 11
d_7d_8d_9=728 is divisible by 13
d_8d_9d_10=289 is divisible by 17
Find the sum of all 0 to 9 pandigital numbers with this property.*)


let rec distribute e = function
    | [] -> [[e]]
    | x::xs' as xs -> (e::xs)::[for xs in distribute e xs' -> x::xs]

let rec permute = function
    | [] -> [[]]
    | e::xs -> List.collect (distribute e) (permute xs)

let digits number =  [for c in (string number) -> c]

let rec digitstonumber (listofdigits : char list) = 
    match listofdigits with
        | [] -> 0UL
        | head::tail -> uint64 (10. ** (float (List.length listofdigits) - 1.) * float (int (head) - 48)) + (digitstonumber tail)

let hasproperty input = 
    let rec getpieces = function
    | a::b::c::tail -> (digitstonumber [a;b;c])::(getpieces (b::c::tail))
    | a::b::[] -> []
    | _ ->[]
    List.map2 (fun x y -> x % y = 0UL) (getpieces input).Tail [2UL;3UL;5UL;7UL;11UL;13UL;17UL] |> Seq.exists ((=)false) |> not 
    

let answer = 
    digits 1234567890
    |> permute
    |> List.filter hasproperty
    |> List.map digitstonumber
    |> List.sum
