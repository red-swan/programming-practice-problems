(*
The fraction 49/98 is a curious fraction, as an inexperienced mathematician in attempting to simplify it may incorrectly believe that 49/98 = 4/8, which is correct, is obtained by cancelling the 9s.

We shall consider fractions like, 30/50 = 3/5, to be trivial examples.

There are exactly four non-trivial examples of this type of fraction, less than one in value, and containing two digits in the numerator and denominator.

If the product of these four fractions is given in its lowest common terms, find the value of the denominator.
*)
let numbers = [10..99]
let cartesian xs ys = 
    xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))

let contains x = Seq.exists ((=)x)
let characters something = [for c in (string something) -> c]
let characterst (a,b) = (characters a, characters b)
let digits number =  [for c in (string number) -> c] |> List.map (fun x -> (int x) - (int '0'))
(*
10A + X         =       A
10X + B         =       B
10AB + XB = 10AX + AB
10 = (AB - BX) / (AB - AX)
*)
let cancelcheck (num, den) = 
    let dnum = digits num
    let dden = digits den
    if(List.length dnum > 2 || List.length dden > 2) 
        then false
    else
        let [A;X;] = dnum
        let [Y;B;] = dden
        X = Y &&  10 * (A*B - A*X) = (A*B - B*X)

// ANSWER
cartesian numbers numbers 
    |> List.filter (fun (a,b) -> a < b && not ((a % 10 = 0) && (b % 10 = 0)))
    |> List.filter cancelcheck


// INTERESTING QUESTIONS
// Primality tests (for ints, BigInts)
// fastest way to get digits?

// ALL SCRATCH BELOW HERE
let getdigits number =  [for c in (string number) -> c] |> List.map (fun x -> (int x) - (int '0'))
let getshareddigit (num, den) = 
    let rec scanfordigits = function
    | head::tail -> if contains head (getdigits den) then head::(scanfordigits tail) else (scanfordigits tail)
    | [] -> []
    scanfordigits (getdigits num)

List.
let erasedigit n number = getdigits number |> List.filter (fun x -> x <> n)

let digitstoint = SOME SORT OF INCREMENTER ??

let doescancelingwork (num ,den) = 
    let n = getshareddigit (num den)
    let testnum = erasedigit n num |> digitstoint
    let testden = erasedigit n den |> digitstoint
    float num / float den = float testnum / float testden


// input is a (num, den)
// output is bool for whether it works or not
let sortout (num,den) = 
	let aschars = characterst (num,den)
	

// Two digits functions, which is faster??
let getdigits number =  [for c in (string number) -> c] |> List.map (fun x -> (int x) - (int '0'))
let digits number = 
    let rec sortthrough = function
        | 0 -> []
        | x -> [(x % 10)] @ (sortthrough (x / 10))
    sortthrough number |> List.rev

// Digits for BIG ??
let digits number = 
    let rec sortthrough = function
        | 0I -> []
        | x -> [(x % 10I)] @ (sortthrough (x / 10I))
    sortthrough number |> List.rev
