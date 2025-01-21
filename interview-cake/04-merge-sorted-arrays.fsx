let mergeLists (l1  : 'a list) l2 : 'a list = 
    let rec loop acc l1 l2 = 
        match l1, l2 with
        | [], [] -> List.rev acc
        | [], y :: ys -> loop (y :: acc) [] ys
        | x :: xs, [] -> loop (x :: acc) xs []
        | x :: xs, y :: ys -> 
            if x < y
            then loop (x :: acc) xs l2
            else loop (y :: acc) l1 ys
    loop [] l1 l2

let myList     = [3; 4; 6; 10; 11; 15]
let alicesList = [1; 5; 8; 12; 14; 19]
let bobsList   = [3; 4; 5; 10; 15; 20]
let allLists = [myList; alicesList; bobsList]
mergeLists myList alicesList



// What if we wanted to merge several sorted lists? 
open System.Collections.Generic

let rec insert v i l =
    match i, l with
    | 0, xs -> v::xs
    | i, x::xs -> x::insert v (i - 1) xs
    | i, [] -> failwith "index out of range"

let rec remove i l =
    match i, l with
    | 0, x::xs -> xs
    | i, x::xs -> x::remove (i - 1) xs
    | i, [] -> failwith "index out of range"
let getMinIdx l =
    l
    |> Seq.indexed
    |> Seq.minBy snd
    |> fst


// Write a function that takes as an input a list of sorted lists and outputs a single 
// sorted list with all the items from each list.
let mergeAll (listOfLists : 'a list list) : 'a list when 'a : comparison = 
    // List.reduce mergeLists listOfLists // this works just fine
    let enums = 
        listOfLists 
        |> List.filter (not << List.isEmpty) 
        |> List.map (Seq.ofList >> (fun (x : seq<'a>) -> x.GetEnumerator()))
    for e in enums do e.MoveNext() |> ignore
    let rec loop acc (enums : list<IEnumerator<'a>>) = 
        match enums with
        | [] -> List.rev acc
        | [enumLast] -> 
            let nextVal = enumLast.Current
            if enumLast.MoveNext() 
            then loop (nextVal :: acc) [enumLast]
            else loop (nextVal :: acc) []
        | _ ->
            let minIdx = enums |> List.map (fun x -> x.Current) |> getMinIdx
            let minVal = enums.[minIdx].Current
            if enums.[minIdx].MoveNext()
            then loop (minVal :: acc) enums
            else loop (minVal :: acc) (remove minIdx enums)

    loop [] enums

mergeAll allLists


// Do we absolutely have to allocate a new list to use for the merged output?
// Where else could we store our merged list? How would our function need to change?
