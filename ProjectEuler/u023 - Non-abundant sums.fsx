(*
A perfect number is a number for which the sum of its proper divisors is exactly 
equal to the number. For example, the sum of the proper divisors of 28 would be 
1 + 2 + 4 + 7 + 14 = 28, which means that 28 is a perfect number.

A number n is called deficient if the sum of its proper divisors is less than n 
and it is called abundant if this sum exceeds n.

As 12 is the smallest abundant number, 1 + 2 + 3 + 4 + 6 = 16, the smallest 
number that can be written as the sum of two abundant numbers is 24. By 
mathematical analysis, it can be shown that all integers greater than 28123 can 
be written as the sum of two abundant numbers. However, this upper limit cannot 
be reduced any further by analysis even though it is known that the greatest 
number that cannot be expressed as the sum of two abundant numbers is less than 
this limit.

Find the sum of all the positive integers which cannot be written as the sum of 
two abundant numbers.
*)

let properDivisors (myInt:int) = 
    let upper = myInt |> float |> sqrt |> int
    let rec loop i divList = 
        match i with
        | number when (number = upper) && 
                      (number * number = myInt) &&
                      (myInt % number = 0)
            -> number :: divList
        | number when number = upper && (myInt % number = 0)
            -> ([number] @ [myInt / number] @ divList)
        | number when (number < upper) && (myInt % number = 0)
            -> loop (i+1) ([number] @ [myInt / number] @ divList)
        | number when number < upper 
            -> loop (i+1) divList
        | _ -> divList
    loop 2 [1]
    
let isDeficient integer = 
    let integerSum = integer |> properDivisors |> List.sum
    integerSum < integer

let isAbundant integer = 
    let integerSum = integer |> properDivisors |> List.sum
    integer < integerSum

let isPerfect integer = 
    let integerSum = integer |> properDivisors |> List.sum
    integerSum = integer

let abundantNumbers = 
    [2 .. 28123]
    |> List.filter isAbundant

let hasAbundantSum int = 
//    let candidates = List.filter (fun x -> x <= int) abundantNumbers
    Seq.find (fun x -> )

    0


///////////////////////////////////////////////////////////////////////
let checknumber number divisor =
    let divided = (float)number / (float)divisor
    if divided = floor divided then
        divided
        |> int
        |> printfn "%i = %i * %i" number divisor

let rec finddivisor number divisor numbers = 
    match numbers with
    | head :: tail -> 
        checknumber number head
        finddivisor number head tail
    | [] -> printfn "finish"

let divisions number =
    let list = [1..number]
    finddivisor number list.Head list

let ``find my divisors`` = 12345

divisions ``find my divisors``
////////////////////////////////////////////////////////////////////////////////
let divisorsSum n =
  let mutable sum = 1
  let limit = (int (sqrt(float n)))
  for i in [2..limit] do
    if n%i=0 then sum <- sum+i+n/i
  if (limit*limit)=n then sum-limit else sum

let isAbundant x = (divisorsSum x)>x
let abundants  = [1..28123] |> List.filter isAbundant |> List.toArray
let domain = System.Collections.BitArray(28124)

let rec loopUntil i j =
    if i=abundants.Length then ()
    elif j=abundants.Length then loopUntil (i+1) (i+1)
    else
      let sum = abundants.[i]+abundants.[j] 
      if sum<28124 then 
        domain.Set(sum, true)
        loopUntil i (j+1)
      else 
        loopUntil (i+1) (i+1)

let solve  =    
    loopUntil 0 0
    [1..28123] |> List.filter (fun x -> domain.Get(x)=false) |> List.sum