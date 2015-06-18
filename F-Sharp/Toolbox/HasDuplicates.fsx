let hasduplicates (inputlist : 'a list)=
    let dictionary = Set.empty
    let rec loop (cache : Set<'a>) (something : 'a list)=
        if (List.isEmpty something) then false
        elif Set.contains (List.head something) cache then true
        else loop (Set.add (List.head something) cache) (List.tail something)
    loop dictionary inputlist
