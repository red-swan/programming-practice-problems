(*A palindromic number reads the same both ways. The largest palindrome made from the product of two 2-digit numbers is 9009 = 91 × 99.

Find the largest palindrome made from the product of two 3-digit numbers.*)

let cartesian xs ys = xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))
let digits number =  [for c in (string number) -> c] |> List.map (fun x -> (int x) - (int '0'))
let ispalindrome number = digits number = (List.rev (digits number))

let answer = 
    cartesian [100 .. 999] [100 .. 999]
    |> List.map (fun (a,b) -> a*b)
    |> List.filter ispalindrome
    |> List.max

// THINGS TO IMPROVE
// This doesn't run fast (epitome of brute force method)
