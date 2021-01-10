


let fifoCheck takeOutOrders dineInOrders servedOrders = 
    let rec loop takeouts dineins serveds = 
        // printfn "takeouts: %A\tdineins: %A\tserveds: %A\n" takeouts dineins serveds
        match takeouts, dineins, serveds with
        | [],[],[] -> true
        | ts, ds, s::ss ->
            if not(List.isEmpty ds) && s = List.head ds
                then loop ts (List.tail ds) ss
            elif not(List.isEmpty ts) && s = List.head ts
                then loop (List.tail ts) ds ss
            else
                false
        | _ -> false
    loop takeOutOrders dineInOrders servedOrders


fifoCheck [1; 3; 5] [2; 4; 6] [1; 2; 4; 6; 5; 3]
fifoCheck [17; 8; 24] [12; 19; 2] [17; 8; 12; 19; 24; 2]



// This assumes each customer order in served_orders is unique. 
// How can we adapt this to handle lists of customer orders with potential repeats?

// Our implementation returns True when all the items in dine_in_orders and 
// take_out_orders are first-come first-served in served_orders and False otherwise. 
// That said, it'd be reasonable to raise an exception if some orders that went into 
// the kitchen were never served, or orders were served but not paid for at either register. 
// How could we check for those cases?

// Our solution iterates through the customer orders from front to back. 
// Would our algorithm work if we iterated from the back towards the front?
// Which approach is cleaner?

