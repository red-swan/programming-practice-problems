type Tree<'a> = 
    | Branch of 'a * Tree<'a> * Tree<'a>
    | Leaf of 'a
let getDepth tree = 
    let rec loop (lvl : int) t = 
        match t with
        | Leaf _ -> seq { lvl }
        | Branch (a,b,c) -> 
            Seq.collect (fun x -> loop (lvl + 1) x) (seq { b; c })
    loop 0 tree
let isSuperBalanced tree : bool = 
    tree
    |> getDepth
    |> Seq.fold (fun (mn,mx) x -> (min x mn, max x mx)) (0,0)
    |> (fun (min,max) -> 1 <= (max - min))

let sample1 = Branch (1, Branch (2, Leaf 4, Leaf 5), Leaf 3)
let sample2 = Branch (1, Branch (2,Branch (3, Leaf 4, Leaf 5), Leaf 6), Leaf 7)

getDepth sample1
getDepth sample2
isSuperBalanced sample1


