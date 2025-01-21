

// List monad
type ListBuilder() =
  member this.Bind (m, f) = List.collect f m 
  member this.Return (x) = [x]
  member this.ReturnFrom (x) = x
  member this.Zero () = []
  member this.Combine (a,b) = a@b
  member this.Delay f = f ()

let list = ListBuilder()




// memoize a function ----------------------------------------------------------
let memoize f =
    let dict = new System.Collections.Generic.Dictionary<_,_>()
    fun n ->
        match dict.TryGetValue(n) with
        | (true, v) -> v
        | _ ->
            let temp = f(n)
            dict.Add(n, temp)
            temp














