// euler 55 -----------------------------------------------
let isPalindrome bigInt = 
    let ourString = bigInt.ToString().ToCharArray()
    let stringLength = ourString.Length
    let half = stringLength / 2
    ourString.[0 .. (half-1)] = (Array.rev (ourString.[(stringLength-half) .. (stringLength - 1)]))

let toPalindrome (bigInt:Numerics.BigInteger) = 
    bigInt
    |> (fun x -> x.ToString().ToCharArray())
    |> Array.rev
    |> (fun x -> System.String x)
    |> (fun x -> Numerics.BigInteger.TryParse(x))
    |> snd

let addPalindrome bigInt = 
    bigInt + (toPalindrome bigInt)


let lychrelSeq bigInt = 
    Seq.unfold (fun (i,num) -> 
                  if i > 50 || (isPalindrome num) then None 
                  else Some((i,num),(i+1,addPalindrome num))) (1,addPalindrome bigInt)


[1I .. 10000I]
|> Seq.map lychrelSeq
|> Seq.filter (fun x -> Seq.length x > 49)
|> Seq.length
