

let sortScores unsortedScores highestVal = 
    let scoreCounts = Array.create highestVal 0

    for score in unsortedScores do
        scoreCounts.[score-1] <- scoreCounts.[score-1] + 1

    let sortedScores = Array.create (Seq.length unsortedScores) 0
    let mutable idx = 0

    for i,count in Seq.indexed scoreCounts do
        for _ in List.replicate (count) i do    
            sortedScores.[idx] <- i + 1
            idx <- idx + 1
        
    sortedScores


sortScores [37; 89; 41; 65; 91; 53; 100] 100







