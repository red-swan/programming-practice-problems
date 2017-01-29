(*
By starting at the top of the triangle below and moving to adjacent numbers on
the row below, the maximum total from top to bottom is 23.

3
7 4
2 4 6
8 5 9 3

That is, 3 + 7 + 4 + 9 = 23.

Find the maximum total from top to bottom in triangle.txt (right click and 
'Save Link/Target As...'), a 15K text file containing a triangle with 
one-hundred rows.

NOTE: This is a much more difficult version of Problem 18. It is not possible 
to try every route to solve this problem, as there are 299 altogether! If you 
could check one trillion (1012) routes every second it would take over twenty 
billion years to check them all. There is an efficient algorithm to solve it. 
;o)
*)

open System
open System.IO

let pyramid = 
    "C:\Users\JDKS\Applications\Github\Project-Euler-Solutions\p067_triangle.txt"
    |> File.ReadAllLines
    |> Array.map (fun x -> x.Split(' '))
    |> Array.map (fun strArr -> Array.map (fun str -> Int32.Parse(str)) strArr)
    |> Array.map (fun intArr -> Array.toList intArr)
    |> Array.toList

let maxPair intArr = 
    intArr
    |> List.pairwise 
    |> List.map (function (a,b) -> if a>b then a else b)

let combineRows (intArrTop:int list) (intArrBot:int list) = 
    intArrTop
    |> maxPair
    |> List.map2 (fun t b -> t+b) intArrBot

let traverseRows intListBinaryTree =
    let front::back = intListBinaryTree
    let rec loop acc lstOfIntLst = 
        match lstOfIntLst with
        | [] -> acc
        | head::tail -> loop (combineRows acc head) tail
    loop front back


let answer =
    pyramid
    |> List.rev
    |> traverseRows

