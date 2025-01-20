

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

type Queue<'a> = Stack<'a> * Stack<'a>
module Queue = 
    let empty = [], []
    let enqueue q item = (Stack.push (fst q) item, snd q)
    let dequeue q = 
        match q with
        | [], [] -> failwith "Empty queue"
        | fs, b :: bs -> b, (fs, bs)
        | fs, [] -> 
            let bs = List.rev fs
            List.head bs, ([], List.tail bs)


















