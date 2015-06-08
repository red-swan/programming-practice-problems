










let pentagonalnumbers = Seq.unfold (fun x-> Some(((3UL*x-1UL)*x)/2UL,x+1UL)) 1UL
let pentagonaldifferences = Seq.unfold (fun x-> Some(3UL*x + 1UL,x+1UL)) 1UL
let ispentagonal (x : uint64) = ((sqrt (24.0* (float x)  + 1.0) + 1.0) / 6.0) % 1.0 = 0.0

Seq.find ispentagonal pentagonaldifferences