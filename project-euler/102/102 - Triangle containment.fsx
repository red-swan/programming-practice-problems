(*
Three distinct points are plotted at random on a Cartesian plane, 
for which -1000 ≤ x, y ≤ 1000, such that a triangle is formed.

Consider the following two triangles:
A(-340,495), B(-153,-910), C(835,-947)
X(-175,41), Y(-421,-714), Z(574,-645)

It can be verified that triangle ABC contains the origin, 
whereas triangle XYZ does not.

Using triangles.txt a 27K text file containing the co-ordinates of
 one thousand "random" triangles, find the number of triangles for 
 which the interior contains the origin.

NOTE: The first two examples in the file represent the triangles in 
the example given above.
*)


// http://blackpawn.com/texts/pointinpoly/
// https://en.wikipedia.org/wiki/Cross_product

open System.IO

type point = float*float
type triangle = Cartesian of point*point*point

let cross ((x1,y1) : point) ((x2,y2) : point) = 
    x1*y2 - x2*y1

let textToTriangles (coordinateStr : string)= 
    coordinateStr.Split(',')
    |> Array.map float
    |> (fun x -> Cartesian ( (x.[0],x.[1]) , (x.[2],x.[3]) , (x.[4], x.[5]) ) )

let containsOrigin (Cartesian (a,b,c) : triangle) = 
    [cross a b; cross b c; cross c a]
    |> List.map sign
    |> List.sum
    |> (fun x -> abs(x) = 3 )

#time
let answer = 
    "p102_triangles.txt"
    |> File.ReadAllLines
    |> Array.map textToTriangles
    |> Array.filter containsOrigin
    |> Array.length
#time // .003