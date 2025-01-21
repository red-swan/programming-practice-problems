
let findHigh3 (intList : int list) : int = 
    let init = intList.[0..2]
    let initProd2 = init.[0] * init.[1]
    let initProd3 = List.reduce (*) init
    let initHigh = max init.[0] init.[1]
    let initLow = min init.[0] init.[1]
    let rec loop prod3 lowProd2 highProd2 low high intList =
        match intList with
        | [] -> prod3
        | x::xs ->
            let newProd3 = List.max [prod3; lowProd2 * x; highProd2 * x]
            let newLowProd2 = min lowProd2 (low * x)
            let newHighProd2 = max highProd2 (high * x)
            let newLow = min low x
            let newHigh = max high x
            loop newProd3 newLowProd2 newHighProd2 newLow newHigh xs

    loop initProd3 initProd2 initProd2 initLow initHigh intList.[2..]

let sample1 = [-10; -10; 1; 3;2]
let sample2 = [1; 10; -5; 1; -100]

findHigh3 sample1 
findHigh3 sample2













