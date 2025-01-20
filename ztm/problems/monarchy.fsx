#time
type Status = | Alive | Dead
type Monarchy = 
    {
        Name : string
        Status : Status
        Children : Monarchy list
    }


module Monarchy =
    let create name = {Name = name; Status = Alive; Children = []}

    let rec birth (child : string) (parent : string) monarchy : Monarchy = 
        if parent = monarchy.Name then
            {monarchy with Children = List.append monarchy.Children [create child]}
        else
            {monarchy with Children = List.map (birth child parent) monarchy.Children }
    
    let birthMany l m = 
        List.fold (fun m (c,p) -> birth c p m) m l

    let rec death name monarchy : Monarchy = 
        if name = monarchy.Name then
            {monarchy with Status = Dead}
        else
            {monarchy with Children = List.map (death name) monarchy.Children }
    let deathMany l m = 
        List.fold (fun m n -> death n m) m l


    let getOrderOfSuccession monarchy : string list = 
        let rec loop x = 
            seq {
                if x.Status = Alive then
                    yield x.Name
                yield! Seq.collect loop x.Children
            }
        loop monarchy
        |> Seq.toList



let monarchy = Monarchy.create "Jake"

let births = 
    [
        ("Catherine","Jake"); ("Jane","Catherine"); ("Farah","Jane")
        ("Tom","Jake"); ("Celine","Jake"); ("Mark","Catherine")
        ("Peter","Celine")
    ]

let monarchy2 = Monarchy.birthMany births monarchy

Monarchy.getOrderOfSuccession monarchy2

let deaths = ["Jake";"Jane"]

let monarchy3 = Monarchy.deathMany deaths monarchy2

Monarchy.getOrderOfSuccession monarchy3














