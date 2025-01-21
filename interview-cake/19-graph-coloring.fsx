
type Graph<'a  when 'a : comparison> = Map<'a,Set<'a>>
let getColorings graph = Set.ofList [0 .. Map.count graph ]
let setOptionToSet = function | None -> Set.empty | Some s -> s

let colorNodes graph : Map<'a,int> = 
    let colors = getColorings graph

    (Map.empty, graph)
    ||> Map.fold (fun (colorMapping : Map<'a,int>) (label : 'a) (neighbors : Set<'a>) ->
        let neighborColors = 
            (Set.empty, Set.remove label neighbors) // just don't even process loops 
            ||> Set.fold (fun adjColors neighbor -> 
                match Map.tryFind neighbor colorMapping with
                | None -> adjColors
                | Some color -> Set.add color adjColors )
        let newColor = Seq.find (fun color -> neighborColors |> Set.contains color |> not) colors 
        (Map.add label newColor colorMapping)  
        ) 


let sample1 : Graph<char> = 
    Map.ofList ['a',['b']; 'b',['a';'c']; 'c', ['b']] |> Map.map (fun k t -> Set.ofList t)
let sample2 : Graph<int> = 
    [1, [2;8]; 2, [1;9]; 3, [2;9;4]; 4, [10;5;3]
     5, [4;11;6]; 6, [7;11;5]; 7, [1;8;6]; 8, [1;7;12];
     9, [2;3;12]; 10, [12;11;4]; 11, [10;6;5]; 12, [8;9;10]]
    |> Map.ofList |> Map.map (fun k v -> Set.ofList v)
    

colorNodes sample1
sample2 |> colorNodes |> Map.toList








