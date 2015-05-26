let cartesian xs ys = xs |> List.collect (fun x -> ys |> List.map (fun y -> x, y))
