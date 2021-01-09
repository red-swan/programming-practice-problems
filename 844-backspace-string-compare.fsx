#time


let sample1 = "ab#c", "ad#c"
let sample2 = "ab##", "c#d#"
let sample3 = "a##c", "#a#c"
let sample4 = "a#c", "b"
let sample5 = "a######", "a"
let sample6 = "a######", "a#"
let sample7 = "###", "#"
let sample8 = "bxj##tw", "bxo#j##tw"
let sample9 = "xywrrmp", "xywrrmu#p"
let sample10 = "xp", "p"

let f (s : string) = 
    let rec loop skip str = 
        seq { if Seq.isEmpty str then do ()
              elif Seq.head str = '#' then yield! loop (skip + 1) (Seq.tail str)
              elif 0 < skip then yield! loop (skip - 1) (Seq.tail str)
              else 
                  yield Seq.head str
                  yield! loop skip (Seq.tail str)
        }
    loop 0 (Seq.rev s)


let backspaceCompare (s : string) (t : string) = 
    (f s, f t)
    ||> Seq.compareWith Operators.compare
    |> ((=)0)



sample1 ||> backspaceCompare  // true
sample2 ||> backspaceCompare  // true
sample3 ||> backspaceCompare  // true
sample4 ||> backspaceCompare  // false
sample5 ||> backspaceCompare  // false
sample6 ||> backspaceCompare  // true
sample7 ||> backspaceCompare  // true
sample8 ||> backspaceCompare  // true
sample9 ||> backspaceCompare  // true
sample10 ||> backspaceCompare // false
