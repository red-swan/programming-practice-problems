namespace ProjectEuler

[<AutoOpen>]
module Combinatorics =

    open HigherOrder

    // Factorial Function
    let rec factorial = memoize(fun n -> 
        if n = 0I then 1I
        else (n-1I) |> factorial |> ((*)n))



    // Cartesian Product -----------------------------------------------------------
    let cartesian xs ys = xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))

    let private nextPascalRow pascalRow = 
        [0I] @ pascalRow @ [0I]
        |> List.pairwise
        |> List.map (fun (a,b) -> a+b)

    let pascalRow n = 
        let rec loop iteration pascalRow =  
            match iteration with 
            | value when value = n -> pascalRow
            | _ -> pascalRow |> nextPascalRow |> loop (iteration + 1)
        loop 1 [1I]

    let choose n k = 
        (n+1)
        |> pascalRow
        |> List.item k


       


    let rec permutations n lst = 
      let rec selections = function
          | []      -> []
          | x::xs -> (x,xs) :: list { let! y,ys = selections xs 
                                      return y,x::ys }
      (n, lst) |> function
      | 0, _ -> [[]]
      | _, [] -> []
      | _, x::[] -> [[x]]
      | n, xs -> list { let! y,ys = selections xs
                        let! zs = permutations (n-1) ys 
                        return y::zs }

    let permute lst = 
        permutations (List.length lst) lst

    let rec combinations n lst = 
      let rec findChoices = function 
        | []    -> [] 
        | x::xs -> (x,xs) :: list { let! y,ys = findChoices xs 
                                    return y,ys } 
      list { if n = 0 then return! [[]]
             else
               let! z,r = findChoices lst
               let! zs = combinations (n-1) r 
               return z::zs }