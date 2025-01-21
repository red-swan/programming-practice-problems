(*
In the card game poker, a hand consists of five cards and are ranked, from lowest to highest, in the following way:

High Card: Highest value card.
One Pair: Two cards of the same value.
Two Pairs: Two different pairs.
Three of a Kind: Three cards of the same value.
Straight: All cards are consecutive values.
Flush: All cards of the same suit.
Full House: Three of a kind and a pair.
Four of a Kind: Four cards of the same value.
Straight Flush: All cards are consecutive values of same suit.
Royal Flush: Ten, Jack, Queen, King, Ace, in same suit.
The cards are valued in the order:
2, 3, 4, 5, 6, 7, 8, 9, 10, Jack, Queen, King, Ace.

If two players have the same ranked hands then the rank made up of the highest value wins; for example, a pair of eights beats a pair of fives (see example 1 below). But if two ranks tie, for example, both players have a pair of queens, then highest cards in each hand are compared (see example 4 below); if the highest cards tie then the next highest cards are compared, and so on.

Consider the following five hands dealt to two players:

Hand	 	Player 1	 	Player 2	 	Winner
1	 	5H 5C 6S 7S KD
Pair of Fives
 	2C 3S 8S 8D TD
Pair of Eights
 	Player 2
2	 	5D 8C 9S JS AC
Highest card Ace
 	2C 5C 7D 8S QH
Highest card Queen
 	Player 1
3	 	2D 9C AS AH AC
Three Aces
 	3D 6D 7D TD QD
Flush with Diamonds
 	Player 2
4	 	4D 6S 9H QH QC
Pair of Queens
Highest card Nine
 	3D 6D 7H QD QS
Pair of Queens
Highest card Seven
 	Player 1
5	 	2H 2D 4C 4D 4S
Full House
With Three Fours
 	3C 3D 3S 9S 9D
Full House
with Three Threes
 	Player 1
The file, poker.txt, contains one-thousand random hands dealt to two players. Each line of the file contains ten cards (separated by a single space): the first five are Player 1's cards and the last five are Player 2's cards. You can assume that all hands are valid (no invalid characters or repeated cards), each player's hand is in no specific order, and in each hand there is a clear winner.

How many hands does Player 1 win?
*)
open System.IO

// type declarations -----------------------------------------------------------
type Suit = | Clubs | Diamonds | Hearts | Spades
type Rank = | Two | Three | Four | Five | Six | Seven
            | Eight | Nine | Ten | Jack | Queen | King | Ace
type Card = {Rank:Rank; Suit:Suit}
type HandRank = 
| HighCard of Rank | Pair of Rank | TwoPair of Rank*Rank 
| ThreeOfAKind of Rank | Straight of Rank | Flush of Rank
| FullHouse of Rank*Rank | FourOfAKind of Rank | StraightFlush of Rank
type Hand = Card list
type ShowDown = Hand list

// parsing functions -----------------------------------------------------------
let parseSuit = function
|'C' -> Clubs  |'D' -> Diamonds 
|'H' -> Hearts |'S' -> Spades

let parseRank = function
|'2' -> Two   |'3' -> Three |'4' -> Four 
|'5' -> Five  |'6' -> Six   |'7' -> Seven
|'8' -> Eight |'9' -> Nine  |'T' -> Ten 
|'J' -> Jack  |'Q' -> Queen | 'K' -> King
| 'A' -> Ace 

let parseCard (cardStr : string) = 
    {Rank = parseRank cardStr.[0];
     Suit = parseSuit cardStr.[1]}

// supporting functions --------------------------------------------------------
let getHighCard (inHand : Hand) =
    inHand |> List.map (fun x -> x.Rank) |> List.max

// Hand Ranking Functions ------------------------------------------------------
let isStraight__OLD__ (myHand : Hand) = 
    if List.length myHand < 5 then false else
    let sortedHand = List.sort myHand
    if (List.map (fun card -> card.Rank) sortedHand) = [Two;Three;Four;Five;Ace]
    then true else
    sortedHand
    |> List.pairwise
    |> List.forall (fun (card1,card2) -> -1 = compare card1.Rank card2.Rank)

let isStraight (myHand : Hand) = 
    if List.length myHand < 5 then false else
    let sortedHand = List.sort myHand
    sortedHand
    |> List.pairwise
    |> List.forall (fun (card1,card2) -> -1 = compare card1.Rank card2.Rank)


isStraight [{Rank = Two;Suit = Clubs}; 
            {Rank = Four; Suit = Diamonds}; 
            {Rank = Five; Suit = Clubs}; 
            {Rank = Ace; Suit = Spades};
            {Rank = Three; Suit = Spades}]


let isFlush (myHand : Hand) = 
    let baseSuit = myHand.[0].Suit
    myHand 
    |> List.map (fun card -> card.Suit)
    |> List.exists (fun suit -> suit <> baseSuit)
    |> not



isFlush    [{Rank = Two;Suit = Clubs}; 
            {Rank = Four; Suit = Clubs}; 
            {Rank = Five; Suit = Clubs}; 
            {Rank = Ace; Suit = Clubs};
            {Rank = Three; Suit = Clubs}]


let (|IsStraight|_|) (inHand : Hand) = 
    if isStraight inHand then Some(Straight (getHighCard inHand)) else None
let (|IsFlush|_|) (inHand : Hand) = 
    if isFlush inHand then Some(Flush (getHighCard inHand)) else None
let (|IsStraightFlush|_|) (inHand : Hand) = 
    if isStraight inHand & isFlush inHand 
    then Some(StraightFlush (getHighCard inHand))
    else None
let (|IsHighCard|_|) (inHand : Hand) = 
    Some(HighCard (getHighCard inHand))

    

let cardOccurrences (myHand : Hand) = 
    myHand
    |> Seq.groupBy (fun card -> card.Rank)
    |> Seq.map (fun (rank,cardList) -> (rank, Seq.length cardList))
    |> Seq.filter (fun (rank,n) -> n > 1)
    |> Seq.sortByDescending snd
    |> Seq.toList 

let (|CardOccurrences|_|) (inHand : Hand) = 
    let outList = cardOccurrences inHand
    if outList.IsEmpty then None else Some(outList)

let rankHand (inHand : Hand) = 
    match inHand with
    | IsStraightFlush ranking -> ranking
    | CardOccurrences [(value,4)] -> FourOfAKind value
    | CardOccurrences [(value,3);(fullofs,2)] -> FullHouse (value,fullofs)
    | IsFlush ranking -> ranking
    | IsStraight ranking -> ranking
    | CardOccurrences [(value,3)] -> ThreeOfAKind value
    | CardOccurrences [(high,2);(low,2)] -> TwoPair (high,low)
    | CardOccurrences [(value,2)] -> Pair value
    | IsHighCard ranking -> ranking

let rankHands (player1:Hand) (player2:Hand) = 
    let p1Rank,p2Rank = (rankHand player1),(rankHand player2)
    if p1Rank > p2Rank then true
    elif p1Rank = p2Rank then
        let rec loop p1 p2 = 
            let p1High = (getHighCard p1) 
            let p2High = (getHighCard p2) 
            if p1High > p2High then true
            elif p1High = p2High then
                loop (List.filter (fun x -> x.Rank <> p1High) p1) 
                     (List.filter (fun x -> x.Rank <> p2High) p2)
            else false
        loop player1 player2
    else false
// computation of solution -----------------------------------------------------
let answer= 
    @"p054_poker.txt"
    |> File.ReadAllLines
    |> Array.map (fun (x:string) -> x.Split(' '))
    |> Array.map (fun x -> Array.toList <| Array.map parseCard x)
    |> Array.map (fun (x : Card list) -> List.splitAt 5 x)
    |> Array.toList
//    |> List.unzip
    |> List.filter (fun (a,b) -> rankHands a b)
    |> List.length



// sample data -----------------------------------------------------------------
let testHouse = 
    [{Rank = Two;Suit = Clubs}; 
     {Rank = Two; Suit = Hearts}; 
     {Rank = Two; Suit = Spades}; 
     {Rank = Three; Suit = Spades};
     {Rank = Three; Suit = Clubs}]
let testSet = 
    [{Rank = Two;Suit = Clubs}; 
     {Rank = Two; Suit = Hearts}; 
     {Rank = Two; Suit = Spades}; 
     {Rank = Nine; Suit = Spades};
     {Rank = Ten; Suit = Clubs}]
let testSet2 = 
    [{Rank = Two;Suit = Clubs}; 
     {Rank = Two; Suit = Hearts}; 
     {Rank = Two; Suit = Spades}; 
     {Rank = Ace; Suit = Spades};
     {Rank = Three; Suit = Clubs}]
let testQuads = 
    [{Rank = Two;Suit = Clubs}; 
     {Rank = Two; Suit = Hearts}; 
     {Rank = Two; Suit = Spades}; 
     {Rank = Two; Suit = Diamonds};
     {Rank = Three; Suit = Clubs}]
    