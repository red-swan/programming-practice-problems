(*
If the numbers 1 to 5 are written out in words: one, two, three, four, five, 
then there are 3 + 3 + 5 + 4 + 4 = 19 letters used in total.

If all the numbers from 1 to 1000 (one thousand) inclusive were written out in 
words, how many letters would be used?


NOTE: Do not count spaces or hyphens. For example, 342 (three hundred and 
forty-two) contains 23 letters and 115 (one hundred and fifteen) contains 20 
letters. The use of "and" when writing out numbers is in compliance with British 
usage.
*)

let NumWrdLenBot20 number = 
    match number with 
    |  0                -> 0
    |  1 |  2 |  6 | 10 -> 3
    |  4 |  5 |  9      -> 4
    |  3 |  7 |  8      -> 5
    | 11 | 12           -> 6
    | 15 | 16           -> 7
    | 13 | 14 | 18 | 19 -> 8
    | 17                -> 9
    | _                 -> failwith "only valid for numbers 1-19"

let NumWrdLenTens number = 
    match number with
    | 40 | 50 | 60      -> 5
    | 20 | 30 | 80 | 90 -> 6
    | 70                -> 7
    | _                 -> failwith "Only handles numbers divisible by ten between twenty & ninety"

let GetNumberWordLength number = 
    let rec loop num = 
        match num with 
        | value when value > 1000 -> 
            failwith "GetNumberWordLength only handles 1-1000"
        | value when value > 999 ->
            11 // "onethousand".Length
        | value when value > 99 && value % 100 = 0 ->
            List.sum [ NumWrdLenBot20 (value / 100); 7] // "hundred".Length
        | value when value > 99 ->
            List.sum [ NumWrdLenBot20 (value / 100); 10; loop (value % 100)] // "hundredand".Length]
        | value when value > 19 ->
            List.sum [NumWrdLenTens ((value / 10) *10); NumWrdLenBot20 (value % 10)]
        | value when value > 0 ->
            NumWrdLenBot20 value
        | _ -> failwith "GetNumberWordLength only handles 1-1000"
    loop number

let answer = 
    [1 .. 1000]
    |> List.sumBy GetNumberWordLength

