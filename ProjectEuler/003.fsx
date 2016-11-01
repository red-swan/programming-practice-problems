(*The prime factors of 13195 are 5, 7, 13 and 29.

What is the largest prime factor of the number 600851475143 ?*)

let answer =
    let factorsof (n:int64) =
        let upperBound = int64(System.Math.Sqrt(double(n)))
        [2L..upperBound] |> Seq.filter (fun x -> n%x=0L)
    let isprime m = 
        factorsof m |> Seq.length = 0
    factorsof 600851475143L 
        |> Seq.filter isprime
        |> Seq.last
