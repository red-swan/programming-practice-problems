
type Status = | Alive | Dead
type Monarchy = 
    {
        Name : string
        Status : Status
        Children : Monarchy list
    }


module Monarchy =
    let rec birth parent child monarchy : Monarchy = 
        if parent = monarchy.Name then
            {monarchy with Children = List.append monarchy.Children [child]}
        else
            {monarchy with Children = List.map (birth parent child) monarchy.Children }
    let death name monarchy : Monarchy = 
        failwith "not implemented"
    let getOrderOfSuccession monarchy : string list = 
        failwith "not implemented"


















