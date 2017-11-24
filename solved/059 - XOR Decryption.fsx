(*
Each character on a computer is assigned a unique code and the 
preferred standard is ASCII (American Standard Code for Information 
Interchange). For example, uppercase A = 65, asterisk (*) = 42, 
and lowercase k = 107.

A modern encryption method is to take a text file, convert the bytes 
to ASCII, then XOR each byte with a given value, taken from a secret 
key. The advantage with the XOR function is that using the same 
encryption key on the cipher text, restores the plain text; for 
example, 65 XOR 42 = 107, then 107 XOR 42 = 65.

For unbreakable encryption, the key is the same length as the plain 
text message, and the key is made up of random bytes. The user would 
keep the encrypted message and the encryption key in different locations, 
and without both "halves", it is impossible to decrypt the message.

Unfortunately, this method is impractical for most users, so the 
modified method is to use a password as a key. If the password is 
shorter than the message, which is likely, the key is repeated 
cyclically throughout the message. The balance for this method is 
using a sufficiently long password key for security, but short enough 
to be memorable.
x   
Your task has been made easy, as the encryption key consists of three 
lower case characters. Using cipher.txt (right click and 'Save Link/Target As...'),
 a file containing the encrypted ASCII codes, and the knowledge that 
 the plain text must contain common English words, decrypt the message 
 and find the sum of the ASCII values in the original text.
*)

#load "AssemblyInfo.fs"
#load "Tools.fs"
open System
open System.IO
open System.Text
open Tools
open System.Text.RegularExpressions


// 97 to 122 is a to z
//[0 .. 800] 
//|> List.map (fun x -> (x,char x)) 
//|> List.iter (fun (x,charx) -> printfn "%i: %A" x charx)
module XORKey =
    type T = XORKey of int []
    let create (i: seq<int>) = 
        if Seq.length i = 3
        then XORKey (Seq.toArray i)
        else failwith "XORKey must be of length 3"
    let extractArray (i:T) = 
        let (XORKey i') = i
        i'
    let asString i = 
        i
        |> extractArray
        |> Array.map char
        |> (fun aChars -> System.String.Concat(aChars))

let secretMessage = 
    File.ReadAllText(@"C:\Users\JDKS\Desktop\ProjectEuler\p059_cipher.txt")
    |> (fun x-> x.Split [|','|])
    |> Array.Parallel.map int


let allPossibleKeys = 
    [97 .. 122]
    |> permutations 3 
    |> Seq.toArray
    |> Array.Parallel.map (fun lst -> XORKey.create lst)

//        |> (fun lChars -> System.String.Concat(Array.ofList(lChars))))
//    |> Array.Parallel.map (fun x -> String.replicate 401 x)
//    |> Array.Parallel.map (fun x -> x.[0..1200])

let decryptToBytes message (withKey:XORKey.T) = 
    withKey 
    |> XORKey.extractArray
    |> Seq.infiniteOf 
    |> Seq.map2 (fun secret key -> secret ^^^ key) message
    |> Seq.take (Seq.length message)
    |> Seq.toArray

let bytesToString input = 
    input
    |> Array.Parallel.map (fun x -> char x)
    |> (fun lChars -> System.String.Concat(lChars))

let decryptToString message (withKey:XORKey.T) = 
    withKey
    |> decryptToBytes message
    |> bytesToString


let candidates = 
    allPossibleKeys
    |> Array.Parallel.map (fun x -> (x,decryptToBytes secretMessage x))
    |> Array.Parallel.map (fun (key,decryptedBytes) -> 
            (XORKey.asString key,Array.sum decryptedBytes,bytesToString decryptedBytes))
    |> Array.filter (fun (key,bytes,str) -> Regex.Match(str," the ").Success)
