(*
Surprisingly there are only three numbers that can be written as the sum of fourth powers of their digits:

1634 = 1**4 + 6**4 + 3**4 + 4**4
8208 = 8**4 + 2**4 + 0**4 + 8**4
9474 = 9**4 + 4**4 + 7**4 + 4**4
As 1 = 1**4 is not a sum it is not included.

The sum of these numbers is 1634 + 8208 + 9474 = 19316.

Find the sum of all the numbers that can be written as the sum of fifth powers of their digits.
*)



let getDigits (n:bigint) = 
    n.ToString().ToCharArray() |> Array.map (fun c -> bigint.Parse(c.ToString())) |> Array.toList

let testDigits (input : System.Numerics.BigInteger list) = 
    input
    |> List.map (fun x-> x ** 5)
    |> List.sum

let powertest (input : System.Numerics.BigInteger)=
    if(( getDigits >> testDigits) input = input) then true else false


// This limit is arbitrary, at a certain point, though, it can be expected that the digits will always sum to greater than the number
[2I .. 1000000I]
    |> List.filter powertest
    |> List.sum
