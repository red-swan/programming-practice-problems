(*The decimal number, 585 = 10010010012 (binary), is palindromic in both bases.

Find the sum of all numbers, less than one million, which are palindromic in base 10 and base 2.

(Please note that the palindromic number, in either base, may not include leading zeros.) *)

let ispalindrome number =
	let arr = number.ToString().ToCharArray()
	arr = Array.rev arr

let rec dectobin i =
    match i with
    | 0 | 1 -> string i
    | _ ->
        let bit = string (i % 2)
        (dectobin (i / 2)) + bit
	
let answer = 
	[1 .. 1000000]
	|> List.filter ispalindrome
	|> List.filter (fun x-> dectobin x |> ispalindrome)
	|> List.sum

