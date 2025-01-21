
let findPath1 (network : Map<'a, 'a list>) (point1 : 'a) (point2 : 'a) = 
    let rec loop paths = // could make this 'a list * Set<'a> list if the paths are real big
        match List.tryFind (fun path -> List.head path = point2 && 1 < List.length path ) paths with
        | Some path -> path
        | None ->
            let getNewPath path = 
                let lastName = List.head path
                let newNames = 
                    match Map.tryFind lastName network with 
                    | None -> [] 
                    | Some names -> List.filter (fun name -> List.contains name path |> not) names
                List.map (fun name -> name :: path) newNames
            
            let newPaths = [for path in paths do yield! getNewPath path] |> List.filter (List.isEmpty >> not)
            if List.isEmpty newPaths then [] else loop newPaths
    
    loop [[point1]]
    |> List.rev

let findPath (network : Map<'a,'a list>) point1 point2 = 
    let getFinalPath (visited : Map<'a,'a>) : 'a list = 
        point2
        |> List.unfold (fun (last) -> 
             match visited.[last] with
             | name when name = last -> None
             | name -> Some(last, name) )
        |> List.rev |> List.append [point1]
    let rec loop visited (candidates : Set<'a>) = 
        if Map.containsKey point2 visited then getFinalPath visited
        elif Set.isEmpty candidates then []
        else 
            let nextVisited, nextCandidates = 
                candidates 
                |> Seq.fold (fun (v,c) (name : 'a) -> 
                        let nextCandidates = 
                            network.[name] 
                            |> Seq.filter (fun x -> Map.containsKey x network && not(Map.containsKey x visited)) 
                            |> set
                        let nextVisited = 
                            nextCandidates
                            |> Set.fold (fun visited cand -> Map.add cand name visited ) v 
                        (nextVisited, Set.union c nextCandidates) 
                    ) (visited, Set.empty) 
            loop nextVisited nextCandidates

    loop (Map.ofList [point1,point1]) (Set [point1])


let network = 
    [
        "Min"     , ["William"; "Jayden"; "Omar"];
        "William" , ["Min"; "Noam"];
        "Jayden"  , ["Min"; "Amelia"; "Ren"; "Noam"];
        "Ren"     , ["Jayden"; "Omar"];
        "Amelia"  , ["Jayden"; "Adam"; "Miguel"];
        "Adam"    , ["Amelia"; "Miguel"; "Sofia"; "Lucas"];
        "Miguel"  , ["Amelia"; "Adam"; "Liam"; "Nathan"];
        "Noam"    , ["Nathan"; "Jayden"; "William"];
        "Omar"    , ["Ren"; "Min"; "Scott"];
    ] |> Map.ofList //|> Map.map (fun k v -> List.toSeq v)


findPath network "Jayden" "Adam"
findPath network "Jayden" "Adam1"
findPath network "Jayden" "Jayden"
findPath network "Min" "Adam"









