(*

In England the currency is made up of pound, £, and pence, p, and there are eight coins in general circulation:

1p, 2p, 5p, 10p, 20p, 50p, £1 (100p) and £2 (200p).
It is possible to make £2 in the following way:

1×£1 + 1×50p + 2×20p + 1×5p + 1×2p + 3×1p
How many different ways can £2 be made using any number of coins?

*)


// the total and available coins
let total, coins = 200, [1;2;5;10;20;50;100;200]
 
// implement the coin change algorithm
let rec count n m =
    if n = 0 then 1
    elif n < 0 then 0
    elif (m <= 0 && n >= 1) then 0
    else (count n (m-1)) + (count (n-coins.[m-1]) m)
 
let answer = count total coins.Length
