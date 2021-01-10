

let sample1 = [(0., 1.); (3., 5.); (4., 8.); (10., 12.); (9., 10.)]
let sample2 = [(1., 5.); (2., 3.)]
let sample3 = [(1., 10.); (2., 6.); (3., 5.); (7., 9.)]
let sample4 = [(1., 3.); (2., 4.)]
let sample5 = [(1., 2.); (2., 3.)]

let mergeAll xs = 
    // assumption that all xs are sorted by fst
    let rec loop acc xs = 
        match xs with
        | [] -> List.rev acc
        | [x] -> List.rev (x :: acc)
        | ((s1,e1) as x1) :: ((s2,e2) as x2) :: tail ->
            if e2  <= e1
                then loop acc (x1 :: tail)
            elif s2 <= e1
                then loop acc ((s1, e2) :: tail)
            else 
                loop (x1 :: acc) (x2 :: tail)
    xs 
    |> List.sortBy fst
    |> loop []

mergeAll sample1
mergeAll sample2
mergeAll sample3
mergeAll sample4
mergeAll sample5




// 1. What if we did have an upper bound on the input values? Could we improve our runtime? Would it cost us memory?
// 2. Could we do this "in place" on the input list and save some space? What are the pros and cons of doing this in place?


