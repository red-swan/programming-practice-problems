let stopWatch = System.Diagnostics.Stopwatch.StartNew()
//...
stopWatch.Stop()
printfn "%f" stopWatch.Elapsed.TotalMilliseconds
