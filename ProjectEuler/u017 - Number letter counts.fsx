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

#load "Tools.fsx"


open Tools
open System
open MathNet.Numerics


let getLastDigits = function
    | 1 -> "one"
    | 2 -> "two"
    | 3 -> "three"
    | 4 -> "four"
    | 5 -> "five"
    | 6 -> "six"
    | 7 -> "seven"
    | 8 -> "eight"
    | 9 -> "nine"
    | 10 -> "ten"
    | 11 -> "eleven"
    | 12 -> "twelve"
    | 13 -> "thirteen"
    | 14 -> "fourteen"
    | 15 -> "fifteen"
    | 16 -> "sixteen"
    | 17 -> "seventeen"
    | 18 -> "eighteen"
    | 19 -> "nineteen"
    | _ -> failwith "getLastDigits attempted to get number larger than 19"
let getTensDigit = function
    | 20 -> "twenty"
    | 30 -> "thirty"
    | 40 -> "forty"
    | 50 -> "fifty"
    | 60 -> "sixty"
    | 70 -> "seventy"
    | 80 -> "eighty"
    | 90 -> "ninety"
    | _ -> failwith "getTensDigit attempted to get a number out of range"


let rec loop number str = 
    match number with
    | value when value > 999 -> "one thousand"
    | value when value > 99 -> 
        loop (value % 100) (str + (getLastDigits (value / 100)) + " hundred")
    | value when value > 19 ->
        loop (value / 10) (str + (getTensDigit ((value/10)*10) ))
    | value -> str + (getLastDigits value)