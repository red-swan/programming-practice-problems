
type Node<'a> = 
    { Left : Node<'a> option; Value : 'a ; Right : Node<'a> option }

let empty a = {Left = None; Value = a; Right = None}

let isBST lower upper bt = 
    let rec loop lower upper node = 
        match node with 
        | None -> Seq.empty
        | Some node when node.Value <= lower || upper <= node.Value ->
                  seq { false }
        | Some node ->
            seq {
                yield! loop lower node.Value node.Left
                yield! loop node.Value upper node.Right
            }
        
    loop lower upper (Some bt)
    |> Seq.isEmpty

let isBSTint = isBST System.Int32.MinValue System.Int32.MaxValue

let sample1 =
    { 
      Value = 50
      Left  = Some {Value = 30; Left = Some (empty 20); Right = Some (empty 60)}
      Right = Some {Value = 80; Left = Some (empty 70); Right = Some (empty 90)} 
    }

let sample2 =
    { 
      Value = 50
      Left  = Some {Value = 30; Left = Some (empty 20); Right = Some (empty 40)}
      Right = Some {Value = 80; Left = Some (empty 70); Right = Some (empty 90)} 
    }

List.map isBSTint [sample1; sample2]

