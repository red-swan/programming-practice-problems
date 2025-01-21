
// if we're talking about tree-like structures (e.g. doubly-linked lists with children)
type BinaryTree<'T> = 
    | Node of 'T * BinaryTree<'T> * BinaryTree<'T>
    | Nil

let rec flattenLeft tree =
    seq {
        match tree with
        | Nil -> ()
        | Node(x,left,right) ->
            yield x
            yield! flattenLeft left
            yield! flattenLeft right
    }

let sample1 = Node(1, Nil, Node(2, Nil, Node(3, Node(7,Nil,Node(8,Node(11,Nil,Node(12,Nil,Nil)),Node(9,Nil,Node(10,Nil,Nil)))), Node(4,Nil,Node(5,Nil,Node(6,Nil,Nil))))))
flattenLeft sample1 |> Seq.toList
