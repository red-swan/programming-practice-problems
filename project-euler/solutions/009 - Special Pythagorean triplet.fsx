(*A Pythagorean triplet is a set of three natural numbers, a < b < c, for which,

a^2 + b^2 = c^2
For example, 3^2 + 4^2 = 9 + 16 = 25 = 5^2.

There exists exactly one Pythagorean triplet for which a + b + c = 1000.
Find the product abc.*)

#load "../fsharp-lib/NumberTheory.fsx"


let gettriples s = 
    let s2 = s / 2
    let mlimit = int (sqrt (float s2 ))
    let mutable (sm,k,d,n,a,b,c) = (0,0,0,0,0,0,0)
    let mutable answer = []
    for m in [2 .. mlimit] do
        if s2 % m = 0 then
            sm <- s2 / m
            while sm % 2 = 0 do // reduce the search space by
                sm <- sm / 2 // removing all factors 2
            if m % 2 = 1 then k <- m+2 else k <- m+1
            while k < 2*m && k <= sm do
                if sm % k = 0 && gcd k m = 1 then
                    d <- s2 / (k*m)
                    n <- k-m
                    a <- d*(m*m-n*n)
                    b <- 2*d*m*n
                    c <- d*(m*m+n*n)
                    answer <- answer @ [(a,b,c)]
                k <- k+2
    answer


// let gettriples s = 
//     let mapitout kormore m  =
//         let rec loop = function
//         | [] -> []
//         | k::tail -> [m;k-m;s/(2*m*k)] :: loop tail
//         loop kormore
//     let getabc [m;n;d] =
//         let m2 = m*m
//         let n2 = n*n
//         ((m2-n2)*d,2*m*n*d,(m2+n2)*d)
//     let ms = [1 .. int (sqrt (float s))] |> List.filter (fun x-> s/2 % x = 0)
//     let findks m = [m+1 .. 2*m-1] |> List.filter (fun k -> k%2=1 && (s/(2*m))%k=0 && (gcd k m) = 1)
//     let ks = ms |> List.map (fun m-> findks m)
//     let mnds = List.map2 (fun m k-> mapitout k m) ms ks |> List.collect (fun x->x)
//     List.map getabc mnds

let answer = gettriples 1000 |> (fun [(a,b,c)]->a*b*c)


