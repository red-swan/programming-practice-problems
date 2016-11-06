(*Find the unique positive integer whose square has the form 1_2_3_4_5_6_7_8_9_0,
where each “_” is a single digit.*)

//#load "Tools.fs"

open System

let matchAnswer (bigInt:Numerics.BigInteger) = 
    let chArr = (Convert.ToString bigInt).ToCharArray()
    chArr.[0] = '1' && chArr.[2] = '2' && chArr.[4] = '3' && chArr.[6] = '4' && 
    chArr.[8] = '5' && chArr.[10] = '6' && chArr.[12] = '7' && chArr.[14] = '8' &&
    chArr.[16] = '9' && chArr.[18] = '0'

let getSecondDigit (bigInt:Numerics.BigInteger) = 
    let charArr = (Convert.ToString bigInt).ToCharArray()
    let length = Array.length charArr
    Array.item (length-2) charArr
    

let rec loop value = 
    let nextValue = 
        match getSecondDigit value with
        | '3' -> value + 40I
        | '7' -> value + 60I
        | _ -> failwith "something really broke for you to see this"
    match matchAnswer (value*value) with
    | true -> value
    | false -> loop nextValue



let stopWatch = System.Diagnostics.Stopwatch.StartNew()

let answer = 
    loop 1010101030I

stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalSeconds // 7.129504
