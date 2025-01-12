package main

// Environment -------------------------------------------------------
import (
	"bufio"
	"container/heap"
	"fmt"
	"log"
	"os"
)

// Types -------------------------------------------------------------
const (
	North complex64 = -1
	West  complex64 = -i
	South complex64 = 1
	East  complex64 = i
)

type Position = complex64
type Heading struct {
	position  Position
	direction complex64
}
type Path struct {
	heading Heading
	score   uint
	path    []complex64
}

type PathHeap []*Path

func (ph PathHeap) Len() int { return len(ph) }
func (ph PathHeap) Less(i, j int) bool {
	return ph[i].score < ph[j].score
}
func (ph PathHeap) Swap(i, j int) {
	ph[i], ph[j] = ph[j], ph[i]
}

// add x as element Len()
func (pq *PathHeap) Push(x any) {
	*pq = append(*pq, x.(*Path))
}

// remove and return element Len() - 1.
func (pq *PathHeap) Pop() any {
	old := *pq
	n := len(old)
	path := old[n-1]
	old[n-1] = nil // don't stop the GC from reclaiming the item eventually
	*pq = old[0 : n-1]
	return path
}

// Globals -----------------------------------------------------------
var Directions = [4]int{North, South, West, East}

const dataPath = "16-sample1.txt"

// Functions ---------------------------------------------------------
func parseData(path string) (map[Position]bool, Position, Position) {
	maze := make(map[Position]bool)
	var start Position
	var end Position

	file, err := os.OpenFile(path, os.O_RDONLY, os.ModePerm)
	if err != nil {
		log.Fatalf("Error opening file: %v", err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	rowIndex := 0
	for scanner.Scan() {
		for colIndex, char := range []rune(scanner.Text()) {
			if char == 'E' {
				end = complex(float32(rowIndex), float32(colIndex))
			} else if char == 'S' {
				start = complex(float32(rowIndex), float32(colIndex))
			}
			if char != '#' {
				maze[complex(float32(rowIndex), float32(colIndex))] = true
			}
			colIndex++
		}
		rowIndex++
	}
	return maze, start, end
}

// func Move(p Position, d int) Position {
// 	switch d {
// 	case North:
// 		return Position{x: p.x, y: p.y - 1}
// 	case South:
// 		return Position{x: p.x, y: p.y + 1}
// 	case West:
// 		return Position{x: p.x - 1, y: p.y}
// 	case East:
// 		return Position{x: p.x + 1, y: p.y}
// 	default:
// 		panic("Bad direction specified")
// 	}
// }

// func turns(from, to int) uint {
// 	if from == to {
// 		return 0
// 	} else if (from == North && to == South) ||
// 		(from == South && to == North) ||
// 		(from == West && to == East) ||
// 		(from == East && to == West) {
// 		return 2
// 	} else {
// 		return 1
// 	}
// }

// func getMinScoreAt(p Position, s map[Heading]uint) (uint, bool) {
// 	atLeastOneScoreRegistered := false
// 	var outScore uint = 0

// 	for dir := range Directions {
// 		score, found := s[Heading{position: p, direction: dir}]
// 		switch {
// 		case found && !atLeastOneScoreRegistered:
// 			outScore = score
// 			atLeastOneScoreRegistered = true
// 		case found:
// 			outScore = min(score, outScore)
// 		}
// 	}
// 	return outScore, atLeastOneScoreRegistered
// }

// func getPathChildren(p PathNode, walls map[Position]bool) []*PathNode {
// 	var nextPaths []*PathNode
// 	for _, dirNext := range Directions {
// 		posNext := Move(p.heading.position, dirNext)
// 		if !walls[posNext] {
// 			nextPath := PathNode{}
// 			nextPath.heading.position = posNext
// 			nextPath.heading.direction = dirNext
// 			nextPath.moves = p.moves + 1
// 			nextPath.turns = p.turns + turns(p.heading.direction, dirNext)
// 			nextPaths = append(nextPaths, &nextPath)
// 		}
// 	}
// 	return nextPaths
// }

// func collectPaths(node *PathNode, path *Path, paths *[]Path, endPos Position, endScore uint) {
// 	currPos := node.heading.position
// 	*path = append(*path, currPos)
// 	if currPos == endPos && score(*node) == endScore {
// 		correctPath := make([]Position, len(*path))
// 		copy(correctPath, *path)
// 		*paths = append(*paths, correctPath)
// 	} else {
// 		for _, child := range node.children {
// 			collectPaths(child, path, paths, endPos, endScore)
// 		}
// 	}
// 	*path = (*path)[:len(*path)-1]
// }

// func getBestPathPositions(p *PathNode, endPos Position, endScore uint) uint {
// 	var pathsToEnd [][]Position
// 	var seenPositions = make(map[Position]bool)
// 	var positionCount uint = 0
// 	var tempPath Path = Path{}

// 	collectPaths(p, &tempPath, &pathsToEnd, endPos, endScore)

// 	for _, path := range pathsToEnd {
// 		for _, pos := range path {
// 			_, posSeen := seenPositions[pos]
// 			if !posSeen {
// 				positionCount++
// 				seenPositions[pos] = true
// 			}
// 		}
// 	}
// 	return positionCount
// }

// Main --------------------------------
func main() {
	grid, start, end := parseData(dataPath)
	var bestScore = ^uint(0)
	onPath := make(map[Position]bool)
	scores := make(map[Heading]uint)
	initialSearch := &Path{
		pos:   start,
		dir:   East,
		score: 0,
		path:  []complex64{start},
	}
	todo := make(PathHeap, 0)
	heap.Push(&todo, initialSearch)

	for {
		if todo.Len() == 0 {
			break
		}

		currPath := heap.Pop(&todo).(*Path)

		if scores[currPath.heading] < currPath.score {
			continue
		} else {
			scores[currPath.heading] = currPath.score
		}

		if currPath.heading.position == end && currPath.score <= bestScore {
			bestScore = currPath.score
			for _, pos := range currPath.path {
				onPath[pos] = true
			}
		}
		type posScoreTuple struct {
			pos   Position
			delta uint
		}
		for r, v := range []posScoreTuple{{1, 1}, {1i, 1001}, {-1i, 1001}} {
			newPath := Path{}
			newPath.heading.position
		}

		endScore, endScoreFound = getMinScoreAt(gameSetup.end, scores)
		// if there's no new positions, we're done
		if len(currentSearchNodes) == 0 {
			break
		}
		// iterate through the positions we're at currently
		for _, currentNode := range currentSearchNodes {
			// get the score we're at currently
			// get the next possible headings
			currentNode.children = getPathChildren(*currentNode, gameSetup.walls)
			// fmt.Println(currentHeading, "\t", nextPossibleHeadings)
			// iterate through the next possible headings
			for _, nextPossibleNode := range currentNode.children {
				// score the heading
				scoreNext := score(*nextPossibleNode)

				previouslyRecordedScore, scoredAlready := scores[nextPossibleNode.heading]

				scoredHigherThanEnd := endScoreFound && endScore < scoreNext
				scoredHigherThanPrevious := scoredAlready && previouslyRecordedScore < scoreNext
				if !(scoredHigherThanEnd || scoredHigherThanPrevious) {
					scores[nextPossibleNode.heading] = scoreNext
					nextSearchNodes = append(nextSearchNodes, nextPossibleNode)
				}
			}

		}
		currentSearchNodes = nextSearchNodes
		nextSearchNodes = []*PathNode{}
	}

	// Part 1

	solution1, solution1Found := getMinScoreAt(gameSetup.end, scores)
	if solution1Found {
		fmt.Println("Part 1: ", solution1)
	} else {
		fmt.Println("Part1: No solution found")
	}

	// Part 2
	solution2 := getBestPathPositions(&pathTree, gameSetup.end, solution1)
	fmt.Println("Part 2: ", solution2)

}
