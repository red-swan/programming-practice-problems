type Stack<'a> = 'a list
module Stack = 
    let empty = []
    let isEmpty s = List.isEmpty s 
    let push stack item = item :: stack
    let pop stack = 
        match stack with 
        | [] -> failwith "Nothing on stack"
        | b::bs -> b,bs
    let peek stack = List.tryItem 0 stack

type MaxStack<'a> = {Stack : Stack<'a>; Maxes : Stack<'a>}
module MaxStack = 
    let empty = {Stack = Stack.empty; Maxes = Stack.empty}
    let push m item = 
        match Stack.peek m.Maxes with
        | None ->
            { Stack = Stack.push m.Stack item; Maxes = Stack.push m.Maxes item }
        | Some b when b <= item -> 
            { Stack = Stack.push m.Stack item; Maxes = Stack.push m.Maxes item }
        | _ ->
            { m with Stack = Stack.push m.Stack item}
    let pushMany m items = 
        List.fold (fun m item -> push m item) m items
    let pop m =
        let top, newStack = Stack.pop m.Stack
        let mx, newMax = Stack.pop m.Maxes
        if mx = top then 
            { Stack = newStack; Maxes = newMax }
        else
            { Stack = newStack; Maxes = m.Maxes}
    let peek m = Stack.peek m.Stack
    let getMax m = Stack.peek m.Maxes


let step1 = MaxStack.push MaxStack.empty 5
let step2 = MaxStack.pushMany step1 [4;7;7;8]
MaxStack.getMax step2





