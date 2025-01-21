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

// // // // // // // // // // Working on optimized version

// // // Imperative

let mp2 n odd = 
    let mutable mn,res = n,n
    if odd then do mn <- (mn >>> 1)
    while mn > 0 do
        res <- (res <<< 1) + (mn &&& 1)
        mn <- (mn >>> 1)
    res

let isPalindrome n bse = 
    let mutable reversed,k = 0,n
    while k > 0 do
        reversed <- bse * reversed + k % bse
        k <- k / bse
    n=reversed


// Calculation
let countp limit = 
    let mutable i,sum = 1,0
    let mutable p = (mp2  i true)
    while p < limit do
        if isPalindrome p 10 then do sum <- sum + p
        i <- i+1
        p <- mp2 i true

    i <- 1
    p <- (mp2  i false)
    while p < limit do
        if isPalindrome p 10 then do sum <- sum + p
        i <- i+1
        p <- mp2 i false
    sum

// // // Functional

let mp2 n odd = 
    let mutable mn,res = n,n
    if odd then do mn <- (mn >>> 1)
    while mn > 0 do
        res <- (res <<< 1) + (mn &&& 1)
        mn <- (mn >>> 1)
    res

let mp2 n odd = 0
open System
Convert.ToInt32("101",10)

let algo res mn = res <<< 1 + mn &&& 1
let next mn = mn >>> 1

