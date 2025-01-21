#time
open System.Collections.Generic

let isValidF s = 
    let openBrackets = Set.ofList ['{';'(';'[']
    let closeBrackets = Set.ofList ['}';')';']']
    let bracketLookup = (closeBrackets, openBrackets) ||> Seq.zip |> Map.ofSeq
    (Some(Stack<char>()),s)    
    ||> Seq.fold (fun stackOpt c ->
        match stackOpt with
        | None -> None
        | Some stack ->
            if Set.contains c openBrackets then
                do stack.Push(c)
                Some stack
            elif Set.contains c closeBrackets then
                let openingBracket = bracketLookup.[c]
                if stack.Peek() = openingBracket then
                    do stack.Pop() |> ignore
                    Some stack
                else 
                    None
            else
                Some stack
        )
    |> function | None -> false 
                | Some stack when fst(stack.TryPeek()) -> false 
                | _ -> true

let isValidI (s : string) = 
    let stack = Stack<char>()    
    let openBrackets = Set.ofList ['{';'(';'[']
    let closeBrackets = Set.ofList ['}';')';']']
    let bracketLookup = (closeBrackets, openBrackets) ||> Seq.zip |> Map.ofSeq
    let e = s.GetEnumerator()
    let mutable searching = true
    while e.MoveNext() && searching do
        let c = e.Current
        if Set.contains c openBrackets then
            do stack.Push(c)
        elif Set.contains c closeBrackets then
            let matchingOpenBracket = bracketLookup.[c]
            if stack.Peek() = matchingOpenBracket then
                do stack.Pop() |> ignore
            else
                searching <- false
        else
            ()
    searching && stack.TryPeek() |> fst |> not


let isValid = isValidI

isValid "{}"
isValid "{[()]}"
isValid "{[[]]}"
isValid "{[[]]"
isValid ""
isValid "{([)]}"

