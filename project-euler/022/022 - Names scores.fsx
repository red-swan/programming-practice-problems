(*
Using names.txt (right click and 'Save Link/Target As...'), a 46K text file 
containing over five-thousand first names, begin by sorting it into alphabetical 
order. Then working out the alphabetical value for each name, multiply this 
value by its alphabetical position in the list to obtain a name score.

For example, when the list is sorted into alphabetical order, COLIN, which is 
worth 3 + 15 + 12 + 9 + 14 = 53, is the 938th name in the list. So, COLIN would 
obtain a score of 938 × 53 = 49714.

What is the total of all the name scores in the file?
*)

// 'A' is 65
// 'Z' is 90

open System.IO

let answer = 
    File.ReadAllText("p022_names.txt")
    |> (fun x -> x.Replace("\"",""))
    |> (fun x -> x.Split [|','|])
    |> Array.sort
    |> Array.Parallel.map (fun str -> str.ToCharArray())
    |> Array.Parallel.map (fun chrArr -> Array.sumBy (fun chr -> int chr - 64) chrArr)
    |> Array.Parallel.mapi (fun i x -> (i+1)*x)
    |> Array.sum

