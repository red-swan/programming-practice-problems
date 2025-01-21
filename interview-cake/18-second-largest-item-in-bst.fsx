

type Node<'a> = 
    { Left : Node<'a> option; Value : 'a ; Right : Node<'a> option }
let empty x = { Left = None; Value = x; Right = None}

let getLargest bst = 
    let rec loop node = 
        match node.Right with 
        | None -> node.Value
        | Some rightNode -> loop rightNode
    loop bst

let get2ndLargest bst = 
    let rec loop largestSeen node =
        match node.Left, node.Right with
        | None, None -> largestSeen
        | Some leftNode, None -> getLargest leftNode
        | _, Some newNode -> loop node.Value newNode
    loop bst.Value bst

let sample2 =
    { 
      Value = 50
      Left  = Some {Value = 30; Left = Some (empty 20); Right = Some (empty 40)}
      Right = Some {Value = 80; Left = Some (empty 70); Right = Some (empty 90)} 
    }

let sample3 = 
    {
        Value = 50
        Left  = None
        Right = Some { Value = 60; Left = Some (empty 55); Right = None}
    }


get2ndLargest sample2
get2ndLargest sample3






