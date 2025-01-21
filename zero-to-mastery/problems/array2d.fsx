let flip f x y = f y x
type Queue<'T> = | Queue of 'T list * 'T list
module Queue = 
    let empty = Queue([],[])
    let isEmpty = function | Queue([],[]) -> true | _ -> false
    let enqueue e q = 
        match q with 
        | Queue(xs,ys) -> Queue( e :: xs, ys)
    let enqueueMany es q = Seq.fold (fun q s -> enqueue s q) q es
    let peek q = 
        match q with 
        | Queue([],[]) -> failwith "Empty queue!"
        | Queue(_,y::ys) -> y
        | Queue(xs,[]) -> List.last xs
    let dequeue q =
        match q with 
        | Queue([],[]) -> failwith "Empty queue!"
        | Queue(xs,y::ys) -> y, Queue(xs,ys)
        | Queue(xs,[]) ->
            let ys = List.rev xs
            List.head ys, Queue([],List.tail ys)
    let ofSeq s = enqueueMany s empty

let up (r,c) = (r - 1, c)
let down (r, c) = (r + 1, c)
let left (r,c) = (r,c - 1)
let right (r,c)= (r,c + 1)
let adjCoords coord = Seq.map (fun f -> f coord) [up; right; down; left]
let isValidCoord m n (r,c) = 0 <= r && 0 <= c && r < m && c < n
let validCoords m n exclusions coords = 
    coords |> Seq.filter (fun coord -> not(Set.contains coord exclusions) && isValidCoord m n coord )  
let validAdjCoords m n exclusions coord =  coord |> adjCoords |> validCoords m n exclusions

let dfs (a : 'T [,]) = 
    let m = Array2D.length1 a
    let n = Array2D.length2 a
    let findNext seen coord =
        [up;right;down;left]
        |> Seq.map (fun f -> f coord)
        |> Seq.tryFind (fun coord -> not(Set.contains coord seen) && isValidCoord m n coord ) 
    let rec loop seen ((r,c) as coord) =
        seq {
                yield a.[r,c]
                let newSeen = Set.add coord seen
                match findNext newSeen coord with
                | Some newCoord -> yield! loop newSeen newCoord
                | None -> ()
        }
    loop Set.empty (0,0)
   
let bfs a start = 
    let m,n = Array2D.length1 a, Array2D.length2 a
    (Queue.ofSeq [start],Set.ofList [start]) 
    |> Seq.unfold ( fun (queue,exclusions) -> 
        if Queue.isEmpty queue then None else
            let ((r,c) as coord, queue') = Queue.dequeue queue
            let output = a.[r,c]
            let neighbors = validAdjCoords m n exclusions coord
            let nextQueue = Queue.enqueueMany neighbors queue'
            let nextExclusions = Seq.fold (flip Set.add) exclusions neighbors
            Some(output,(nextQueue,nextExclusions))
        )


let sample1 = [1 .. 20] |> List.chunkBySize 5 |> array2D
dfs sample1 |> Seq.toList
bfs sample1 (2,2) |> Seq.toList 
