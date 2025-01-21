

let reverseInPlace (charArr :  char []) = 
    let n = Array.length charArr
    for i in 1 .. n / 2 do
        let firstChar = charArr.[i-1]
        let secondChar = charArr.[^i-1]
        printfn "first: %c\tlast: %c" firstChar secondChar
        charArr.[i-1] <- secondChar
        charArr.[^i-1] <- firstChar
    ()

let sample = "hello" |> Seq.toArray
reverseInPlace sample
sample

