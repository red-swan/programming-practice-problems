(*The series, 1^1 + 2^2 + 3^3 + ... + 10^10 = 10405071317.

Find the last ten digits of the series, 1^1 + 2^2 + 3^3 + ... + 1000^1000.*)

#r "nuget: FSPowerPack.Community, 3.0.0"
open Microsoft.FSharp.Math

let mutable sum = 0N
for j in [1N .. 1000N] do
    sum <- sum + pown j (int j)
let answer =  
    sum.ToString().[(sum.ToString().Length - 10) ..]

