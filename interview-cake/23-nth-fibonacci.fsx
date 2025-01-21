


let fibNumbers = Seq.unfold (fun (a,b) -> Some(a,(b,a+b))) (0,1) |> Seq.cache
let fib n = Seq.item n fibNumbers
    
fibNumbers |> Seq.take 15 |> Seq.toList
























