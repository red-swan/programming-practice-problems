// imperatively

let reverseArr (arr : 'a []) from until = 
    for i in 0 .. (until - from + 1)/2 - 1 do
        let firstChar = arr.[from+i]
        let secondChar = arr.[until - i]
        arr.[from + i] <- secondChar
        arr.[until - i] <- firstChar

let reverseWords (s : char []) =
    let n = Array.length s
    reverseArr s 0 (n-1)
    let mutable wordStartIdx = 0
    for i in 0 .. n do
        if i = n || s.[i] = ' ' then
            reverseArr s wordStartIdx (i-1)
            wordStartIdx <- i + 1

let sampleStr = "cake pound steal"
let sampleCharArr = Seq.toArray sampleStr

reverseWords sampleCharArr
sampleCharArr


// Functionally
let reverseWordsF (s : string) = 
    s.Split(' ')
    |> Seq.rev
    |> String.concat " "
    


reverseWordsF sampleStr