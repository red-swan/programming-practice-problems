(*The nth term of the sequence of triangle numbers is given by, t_n = ½n(n+1); so the first ten triangle numbers are:

1, 3, 6, 10, 15, 21, 28, 36, 45, 55, ...

By converting each letter in a word to a number corresponding to its alphabetical position and adding these values we 
form a word value. For example, the word value for SKY is 19 + 11 + 25 = 55 = t_10. If the word value is a triangle number 
then we shall call the word a triangle word.

Using words.txt (right click and 'Save Link/Target As...'), a 16K text file containing nearly two-thousand common 
English words, how many are triangle words?*)

open System.IO

let wordsfromfile = 
    File.ReadAllLines(@"C:\Users\Jared\Documents\GitHub\Project-Euler-Solutions\F-Sharp\Text Files\p042_words.txt")
    |> Seq.toList 
    |> List.head
    |> (fun x-> x.Split(','))
    |> Array.toList
    |> List.map (fun x-> x.Replace("\"",""))

// This is made a list to save making the list everytime we check it
let trianglenumbers = Seq.unfold (fun x-> Some ((x*(x+1))/2,x+1)) 1 |> Seq.take 10000 |> Seq.toList

let wordconversion (word : string) =
    [for i in word.ToLower() -> (int i) - 96] |> List.reduce (+)

let istriangle x = Seq.exists ((=)x) trianglenumbers


let answer = 
    wordsfromfile
    |> List.map wordconversion
    |> List.filter istriangle
    |> List.length







/////////////////////////////////////////////////////////////////////////////////////////////////////////////
//                      Scratch         Scratch         Scratch         Scratch                            //
/////////////////////////////////////////////////////////////////////////////////////////////////////////////

let wordsfromtile2 =
    File.ReadAllLines(@"C:\Users\Jared\Documents\GitHub\Project-Euler-Solutions\F-Sharp\Text Files\p042_words.txt")
    |> Array.map (fun s -> s.Split(','))
    |> Array.toList

let stripChars text (chars:string) =
    Array.fold (
        fun (s:string) c -> s.Replace(c.ToString(),"")
    ) text (chars.ToCharArray())

let readLines (filePath:string) = seq {
    use sr = new StreamReader (filePath)
    while not sr.EndOfStream do
        yield sr.ReadLine ()
} 