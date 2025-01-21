
let productOthers (intList : int list) = 
    let ascProds = Seq.scan (*) 1 intList.[..^1]
    let descProds = Seq.scanBack (*) (intList.[1..]) 1
    [for a,b in Seq.zip ascProds descProds -> a*b]

let sample1 = [1; 7; 3; 4]

productOthers sample1

