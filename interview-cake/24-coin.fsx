#time
let count amnt (coins : int []) = 
    let rec loop n m = 
        if n = 0 then 1
        elif n < 0 then 0
        elif (m <= 0 && n >= 1) then 0
        else (loop n (m-1)) + (loop (n-coins.[m-1]) m)
    loop amnt (Seq.length coins)

let count2 (amnt : int) (coins) = 
    let memoised = System.Collections.Generic.Dictionary<int * int, int>()
    let memoise n m c = do if memoised.ContainsKey(n,m) then () else memoised.Add((n,m), c)
    let coinsSorted = Array.sort coins

    let rec loop n m = 
        if n = 0 then 1
        elif (n < 0) || ( m = 0 && 1 <= n) then 0
        elif memoised.ContainsKey((n,m)) then memoised.[(n,m)]
        else 
            let q = n - coinsSorted.[m-1]
            let count1 = loop n (m-1) 
            let count2 = loop q m
            do 
                memoise n (m-1) count1
                memoise q m count2
            count1 + count2
    loop amnt (Array.length coins)
        


count 4 [|1;2;3|]
count 5 [|10;20;30|]

let countset1 = [1;5;10;25;50;100;200;500;1000;2000;5000;10000] |> List.toArray


count 2500 countset1
count2 2500 countset1
count 200 [|1;2;5;10;20;50;100;200|]
count2 200 [|1;2;5;10;20;50;100;200|]