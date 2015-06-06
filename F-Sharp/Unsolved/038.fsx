(*Take the number 192 and multiply it by each of 1, 2, and 3:

192 × 1 = 192
192 × 2 = 384
192 × 3 = 576
By concatenating each product we get the 1 to 9 pandigital, 192384576. We will call 192384576 the
concatenated product of 192 and (1,2,3)

The same can be achieved by starting with 9 and multiplying by 1, 2, 3, 4, and 5, giving the pandigital,
918273645, which is the concatenated product of 9 and (1,2,3,4,5).

What is the largest 1 to 9 pandigital 9-digit number that can be formed as the concatenated product of an
integer with (1,2, ... , n) where n > 1?*)









let rec distribute e = function
    | [] -> [[e]]
    | x::xs' as xs -> (e::xs)::[for xs in distribute e xs' -> x::xs]

let rec permute = function
    | [] -> [[]]
    | e::xs -> List.collect (distribute e) (permute xs)

let digits number = [for k in (string number) -> k]
let digitalproduct number integers = List.map (fun x -> (number *x).ToString()) integers |> Seq.reduce (+)
let ispandigital stringofdigits = (List.sort ([for j in (string stringofdigits) -> j])) = ['1';'2';'3';'4';'5';'6';'7';'8';'9']


let ispandigitalproduct number = 
    true

let answer = 
    digits 987654321
    |> permute
    |> List.sort
    |> List.rev
    |> List.find ispandigitalproduct

let rec digitstonumber (listofdigits : char list) = 
    match listofdigits with
    | [] -> 0
    | head::tail -> (int (10. ** (float (List.length listofdigits) - 1.) * float (int (head) - 48)) + (float ( digitstonumber tail))


