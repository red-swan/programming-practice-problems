(*It is possible to show that the square root of two can be expressed as an infinite continued fraction.

√ 2 = 1 + 1/(2 + 1/(2 + 1/(2 + ... ))) = 1.414213...

By expanding this for the first four iterations, we get:

1 + 1/2 = 3/2 = 1.5
1 + 1/(2 + 1/2) = 7/5 = 1.4
1 + 1/(2 + 1/(2 + 1/2)) = 17/12 = 1.41666...
1 + 1/(2 + 1/(2 + 1/(2 + 1/2))) = 41/29 = 1.41379...

The next three expansions are 99/70, 239/169, and 577/408, but the eighth expansion, 1393/985,
is the first example where the number of digits in the numerator exceeds the number of digits in the denominator.

In the first one-thousand expansions, how many fractions contain a numerator with more digits than denominator?*)


#r "nuget: MathNet.Numerics, 3.0.0"


open MathNet.Numerics
open MathNet.Numerics.Random


let ourDigits n = 
    let rec loop() = seq { yield 2N; yield! loop() }
    seq { yield 1N; yield! loop()} |> Seq.take n |> Seq.toList

let approx n = 
    List.foldBack (fun elem acc -> elem + (1N / acc)) 
                  (ourDigits (n)) (2N)

[1 .. 1000]
|> List.map (fun n -> approx n)
|> List.filter (fun x -> Array.length (x.Numerator.ToString().ToCharArray()) > Array.length (x.Denominator.ToString().ToCharArray()))
|> List.length
