let indexList indices (s : 'a seq) = 
    seq { let indices = ref indices
          let i = ref 0
          use e = s.GetEnumerator()
          while not (List.isEmpty !indices) && e.MoveNext() do
              match !indices with
              | index :: rest when !i = index ->
                  i := !i + 1
                  indices := rest
                  yield e.Current
              | _ -> i := !i + 1 }
