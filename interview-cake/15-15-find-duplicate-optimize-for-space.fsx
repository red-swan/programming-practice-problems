


let findDuplicate ints = 
    let mutable floor = 1
    let mutable ceiling = Seq.length ints - 1

    let rec loop lower upper =
        if lower = upper then lower 
        else
            let midpoint = (lower + upper) / 2
            let lowerCount = 
                ints 
                |> Seq.filter (fun x -> lower <= x && x <= midpoint)
                |> Seq.length

            if (midpoint - lower + 1) < lowerCount
            then loop lower midpoint 
            else loop (midpoint + 1) upper 

    loop 1 (Seq.length ints)

let sample1 = [1;2;3;4;5;10;10;7;8;9]
findDuplicate sample1







