#time
// 1. Given a binary tree, find its maximum depth.
// 2. Given a binary tree, return the level order traversal of the nodes' values as an array
// 3. Given a binary tree, imagine you're standing to the right of the tree.
//    Return an array of the values of the nodes you can see ordered from 
//    top to bottom
// 4. Given a complete binary tree, count the number of nodes
// 5. Given a tree, determine if it is a valid binary search tree

type BinaryTree<'T> = 
    | Node of 'T * BinaryTree<'T> * BinaryTree<'T>
    | Nil
module BinaryTree = 
    let empty = Nil
    let isEmpty t = t = empty
    let node x l r = Node(x,l,r)
    let stitch x l r = 
        match x with 
        | Nil -> failwith "not implemented"
        | Node(v,_,_) -> Node(v,l,r)
    let endNode x = node x Nil Nil
    let fromSome = function | None -> Nil | Some x -> endNode x
    let toSome = function | Nil -> None | Node(x,_,_) -> Some x
    let rec cata fNil fNode tree = 
        let recurse = cata fNil fNode
        match tree with 
        | Nil -> fNil ()
        | Node(x,l,r) -> 
            let listOfRs = List.map recurse [l;r]
            fNode x listOfRs
    let rec fold fNode acc node = 
        let recurse = fold fNode
        match node with
        | Nil -> acc
        | Node (v,l,r) -> 
            let newAcc = fNode acc v
            Seq.fold recurse newAcc [l;r] 
    let foldBack folder tree acc : 'State =
        let rec loop t cont =
            match t with
            | Nil         -> cont acc
            | Node(x,l,r) ->
                loop l (fun lacc ->
                loop r (fun racc ->
                    cont (folder x lacc racc)
                    ))
        loop tree id
    let getNode height idx tree = 
        if idx < 1 || (pown 2 (height - 1)) < idx then None else
        let idx = float(idx)
        let rec loop left right curLevel node =
            // printfn "%.0f %.0f %i %A" left right curLevel (toSome node)
            match node, curLevel with 
            | Nil, _ -> None
            | Node(x,_,_), _ when curLevel = height -> Some x
            | Node(_,l,r), _ ->
                    let mid = ((left + right) / 2.0) |> ceil
                    printfn "%.0f %.0f %.0f" left mid right
                    if mid <= idx then
                        printfn "Right"
                        loop mid right (curLevel + 1) r
                    else 
                        printfn "Left"
                        loop left (mid - 1.0) (curLevel + 1) l
        loop 1.0 (2.0 ** (float(height) - 1.0)) 1 tree
    let existsNode height idx tree = tree |> getNode height idx |> Option.isSome
    let getLevels tree = // bfs
        let rec loop output acc higherNodes lowerNodes = 
            match higherNodes, lowerNodes with
            | [], [] -> List.rev output
            | [], _ -> loop ((List.rev acc)::output) [] (List.rev lowerNodes) []
            | node::nodes, _ ->
                match node with
                | Nil -> loop output (None :: acc) nodes lowerNodes
                | Node(x,l,r) -> loop output ((Some x) :: acc) nodes (r::l::lowerNodes)
        loop [] [] [tree] []
    let getLevelValues tree = 
        [ 
            for level in getLevels tree ->
            [
                for item in level do
                match item with | None -> () | Some x -> yield x
            ]
            
        ]
    let fromList (values : 'T option list) = // assumes fully defined trees
        let level = System.Math.Log2(float(List.length values + 1)) |> int
        let values = values |> List.map fromSome |> List.rev
        let rec loop (acc : BinaryTree<'T> list) level (values : BinaryTree<'T> list) = 
            match acc with 
            | [] -> 
                let acc,values = List.splitAt (pown 2 (level - 1)) values
                loop acc (level - 1) values
            | [node] -> node
            | lowerNodes ->
                let higherNodes, evenHigherNodes = List.splitAt (pown 2 (level - 1)) values
                let lowerNodes = List.chunkBySize 2 lowerNodes
                let acc = 
                        
                    [ 
                        for higher,[r;l] in Seq.zip higherNodes lowerNodes -> 
                            match higher with
                            | Nil -> Nil
                            | Node(x,_,_) -> Node(x,l,r)
                    ]
                loop acc (level - 1) evenHigherNodes
        loop [] level values

// fsi.AddPrinter<BinaryTree<'T>>(fun tree ->
//     let levels = 
//         BinaryTree.getLevels tree
//         |> List.map (List.map (Option.fold (fun _ x -> string x) "_"))
//     let height = List.length levels
//     let bottomLevelN = pown 2 (height-1)
//     let rec makeIndices acc indices = 
//         match indices with
//         | [|x|] -> indices :: acc
//         | idcs -> 
//             let n = (Array.length idcs) / 2
//             let bottomIdx = (indices.[0] + indices.[1]) / 2
//             let upperIdx = (indices.[^1] + indices.[^0]) / 2
//             let by = (upperIdx - bottomIdx) / (n-1)
//             makeIndices (idcs :: acc)  [| bottomIdx .. by .. upperIdx|]
//     let indices = makeIndices [] [|0 .. 2 .. bottomLevelN - 2|]
//     let textBlock = List.init height (fun _ -> String.replicate bottomLevelN " ")
//     [
//         for s,is,values in Seq.zip3 textBlock indices levels do
//         for i,v in Seq.zip indices levels do
//             s.[i] <- v
//     ]

//     "Not implemented"
//     )

let sample1 = Node(4, Node(2, Node(1, Nil, Nil), Node(3, Nil, Nil)), Node(6, Node(5, Nil, Nil), Node(7, Nil, Nil)))
        //         4
        //      /     \
        //     2       6
        //    / \     / \
        //   1   3   5   7
let sample2 = Node(1, BinaryTree.endNode 2, Node(3, BinaryTree.endNode 4, Nil))
            //    1
            //   / \
            //  2   3
            //     /
            //    4
let sample3 = Node(3, Node(6, Node(9, Nil, Node(5,Node(8,Nil,Nil),Nil)), Node(2,Nil,Nil)), Node(1,Nil,Node(4, Nil,Nil)))
        //        3
        //      /   \
        //     6     1
        //    / \     \
        //   9   2     4
        //    \
        //     5
        //    /
        //   8
let sample4 = Node(1,Node(2,Node(4,Nil,Node(7,BinaryTree.endNode 8,Nil)), BinaryTree.endNode 5), Node(3,Nil, BinaryTree.endNode 6))
        //      1
        //    /   \
        //   2     3
        //  / \     \
        // 4   5     6
        //  \
        //   7
        //  /
        // 8
        // NLR [ 1; 2; 4; 7; 8; 5; 3; 6]
        // LNR [ 4; 8; 7; 2; 5; 1; 6; 3]
        // LRN [ 8; 7; 4; 5; 2; 6; 3; 1]
        // NRL [ 1; 3; 6; 2; 5; 4; 7; 8]
        // RNL [ 6; 3; 1; 5; 2; 7; 8; 4]
        // RLN [ 6; 3; 5; 8; 7; 4; 2; 1]
let sample5 = 
    [Some 1; Some 1; Some 1; Some 1; Some 1; Some 1; Some 1;Some 1; Some 1; Some 1;Some 1;Some 1;Some 1;None;None;None;]
    |> BinaryTree.fromList
        //              1
        //         /         \
        //        1           1
        //      /   \       /   \
        //     1     1     1     1
        //    / \   / \   / 
        //   1   1 1   1 1
let sample6 = [1 .. 15] |> List.map Some |> BinaryTree.fromList
        //              1
        //         /         \
        //        2           3
        //      /   \       /   \
        //     4     5     6     7
        //    / \   / \   / \   / \
        //   8   9 10 11 12 13 14 15

let reversed  tree = BinaryTree.foldBack (fun x l r -> r @ [x] @ l) tree [] 
let preOrder  tree = BinaryTree.foldBack (fun x l r -> [x] @ l @ r) tree [] 
let postOrder tree = BinaryTree.foldBack (fun x l r -> l @ r @ [x]) tree [] 
let preOrderR tree = BinaryTree.foldBack (fun x l r -> [x] @ r @ l) tree []
let rightMost tree = tree |> BinaryTree.getLevels |> List.map List.last

// 1.
sample1 |> BinaryTree.cata (fun () -> 1) (fun _ levels -> 1 + List.max levels)

// 2. 
sample2 |> BinaryTree.getLevelValues

// 3.
sample3 |> BinaryTree.getLevelValues |> List.map List.last 

// 4.
let ceilMid a b = ((float(a) + float(b)) / 2.0) |> ceil |> int
let countNodesTreeComplete tree = 
    let height = 
        let rec loop height = function
            | Nil -> height
            | Node(_,l,_) -> loop (height + 1) l
        loop 0 tree
    let rec loop minIdx maxIdx = 
        if minIdx + 1 = maxIdx then 
            minIdx 
        else
            let midIdx = ceilMid minIdx maxIdx
            printfn "%i" midIdx
            if BinaryTree.existsNode height midIdx tree then
                loop midIdx maxIdx
            else
                loop minIdx (midIdx - 1)
    
    loop 1 (pown 2 (height - 1))
    |> ( (+) (pown 2 (height - 1) - 1) ) // upper count

sample5 |> countNodesTreeComplete

// 5.
let sample7 = [12;8;18;5;10;14;25] |> List.map Some |> BinaryTree.fromList
let sample8 = [Some 16; Some 8; Some 22; Some 9; None; Some 19; Some 25] |> BinaryTree.fromList
let isBinaryTree tree =
    let rec loop lower upper tree =
        let between x a b = 
            Option.forall (fun b -> x <= b) b && Option.forall (fun a -> a <= x) a
        match lower, upper, tree with
        | _, _, Nil -> true
        | lower, upper, Node(x,l,r) -> 
            seq {
                    yield (between x lower upper)
                    yield loop lower (Some x) l
                    yield loop (Some x) upper r
                }
            |> Seq.contains false
            |> not
    loop None None tree

isBinaryTree sample7
isBinaryTree sample8



