






let rec quickselect k (input : int []) =
    match input with
    | [||] -> failwith "empty array"
    | [|x|] -> x
    | arr ->
        let x = Array.head arr
        let (lowers, highers) = Array.partition ( fun a -> a < x) arr.[1..]
        let l = Array.length lowers
        if k < l then quickselect k lowers
        elif l < k then quickselect (k - l - 1) highers
        else x



let sample1 = [| 100 .. -1 .. 91|]

quickselect 1 sample1











