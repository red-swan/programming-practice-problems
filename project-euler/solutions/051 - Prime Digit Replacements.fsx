(*
By replacing the 1st digit of the 2-digit number *3, it turns out that six of the 
nine possible values: 13, 23, 43, 53, 73, and 83, are all prime.

By replacing the 3rd and 4th digits of 56**3 with the same digit, this 5-digit 
number is the first example having seven primes among the ten generated numbers, 
yielding the family: 56003, 56113, 56333, 56443, 56663, 56773, and 56993. 
Consequently 56003, being the first member of this family, is the smallest prime 
with this property.

Find the smallest prime which, by replacing part of the number (not necessarily 
adjacent digits) with the same digit, is part of an eight prime value family.
*)


#load "Tools.fsx"

open Tools

// this solution is ugly but gets the job done
// the approach is a brute force search
// we skip primes already checked from another head-of-family
// as well as primes that don't have at least n digits in them

// we could make the isPrime function faster by making it a lookup
// rather than a computation

// another improvement would be to check a number for different digit sizes up
// to its digit length; you would lose the initial prime filter on digit length
// but would be more thorough and general if you wanted to find prime families
// of size 9 or higher



// convert int to list of digits
let intToDigArr number = 
    (string number).ToCharArray() 
    |> Array.map (fun x -> int x - 48)

let intToDigList number = 
    number |> intToDigArr |> Array.toList

// tally the occurrences of each digit
let digitTallies number = 
    number 
    |> intToDigList
    |> List.countBy id

// number of digits in a number
let numOfDigits (number:int) = 
    number.ToString().Length

let replaceDigWith number (digit:int) (posList:int list) = 
    let digArr = intToDigArr number
    for idx in posList do
        digArr.[idx] <- digit
    digArr 
    |> Array.map (fun x -> char (x+48) ) 
    |> System.String.Concat
    |> System.Int32.Parse

let replaceDig number posList = 
    [0 .. 9]
    |> List.map (fun x -> replaceDigWith number x posList)


// take an integer and return it's digits in a list
// as well as a list of the digits of max occurrence
let gatherInfo threshold number = 
    let listOfDigits = intToDigList number
    let numLgth = List.length listOfDigits
    let digitsOfInterest = 
        listOfDigits 
        |> List.countBy id
        |> List.filter (fun (dig,occ) -> occ >= threshold)
        |> List.map fst

    if digitsOfInterest.IsEmpty then [] else 

    let familyList = 
        listOfDigits
        |> List.indexed
        |> List.filter (fun (idx,dig) -> List.contains dig digitsOfInterest)
        |> List.groupBy snd
        |> List.collect (fun (grp,idxList) -> idxList |> List.map fst |> combinations threshold)
        |> List.map (replaceDig number)
        |> List.map (fun candidatesList -> List.filter isPrimeInt candidatesList)
        |> List.map (fun candidatesList -> List.filter (fun x -> (numOfDigits x) = numLgth) candidatesList)
    
    familyList



// does a number have at least n occurrences of any digit
let atLeastNDigits n number = 
    number |> digitTallies |> List.map snd |> List.max |> ((<=) n)

//let checkedPrimes = new System.Collections.Generic.HashSet<int>()
let checkedPrimes = Set.empty<int>

let candidatePrimes = 
    (sieveOfAtkin 1000000)
    
let findAnswer familySize digToReplace checkedPrimes =
    let rec loop checkedSet candidates =
        match candidates with
        | [] -> 0
        | head::tail when Set.contains head checkedSet 
             -> loop (Set.add head checkedSet) tail
        | head::tail 
             -> let families = gatherInfo digToReplace head
                let answer = List.filter (fun family -> List.length family >= familySize) families
                if answer.IsEmpty 
                then (loop (List.fold (fun acc x -> Set.add x acc) checkedSet (List.collect id families)) tail)
                else printf "%A" answer 
                     answer |> List.collect id |> List.min
    
    candidatePrimes 
    |> List.filter (atLeastNDigits digToReplace)
    |> loop Set.empty<int>

#time
let answer = findAnswer 8 3 checkedPrimes
#time //624ms




