package main

// Environment -------------------------------------------------------
import (
	"bufio"
	"fmt"
	"log"
	"os"
)

// Types -------------------------------------------------------------
const (
	North int = iota
	South
	West
	East
)

type Position struct {
	x int
	y int
}
type Heading struct {
	position  Position
	direction int
}

type GameSetup struct {
	start Position
	end   Position
	walls map[Position]bool
}

type PathNode struct {
	heading  Heading
	moves    uint
	turns    uint
	children []*PathNode
}

type Path = []Position

// Globals -----------------------------------------------------------
var Directions = [4]int{North, South, West, East}

const dataPath = "16.txt"

// Functions ---------------------------------------------------------
func parseData(path string) GameSetup {
	var output GameSetup
	output.walls = make(map[Position]bool)

	file, err := os.OpenFile(path, os.O_RDONLY, os.ModePerm)
	if err != nil {
		log.Fatalf("Error opening file: %v", err)
	}
	defer file.Close()
	scanner := bufio.NewScanner(file)
	y := 0
	for scanner.Scan() {
		for x, char := range []rune(scanner.Text()) {
			switch char {
			case '#':
				output.walls[Position{x: x, y: y}] = true
			case 'E':
				output.end.x = x
				output.end.y = y
			case 'S':
				output.start.x = x
				output.start.y = y
			}
			x++
		}
		y++
	}
	return output
}

func Move(p Position, d int) Position {
	switch d {
	case North:
		return Position{x: p.x, y: p.y - 1}
	case South:
		return Position{x: p.x, y: p.y + 1}
	case West:
		return Position{x: p.x - 1, y: p.y}
	case East:
		return Position{x: p.x + 1, y: p.y}
	default:
		panic("Bad direction specified")
	}
}

func turns(from, to int) uint {
	if from == to {
		return 0
	} else if (from == North && to == South) ||
		(from == South && to == North) ||
		(from == West && to == East) ||
		(from == East && to == West) {
		return 2
	} else {
		return 1
	}
}

func score(p PathNode) uint {
	return p.moves + 1000*p.turns
}

func getMinScoreAt(p Position, s map[Heading]uint) (uint, bool) {
	atLeastOneScoreRegistered := false
	var outScore uint = 0

	for dir := range Directions {
		score, found := s[Heading{position: p, direction: dir}]
		switch {
		case found && !atLeastOneScoreRegistered:
			outScore = score
			atLeastOneScoreRegistered = true
		case found:
			outScore = min(score, outScore)
		}
	}
	return outScore, atLeastOneScoreRegistered
}

func getPathChildren(p PathNode, walls map[Position]bool) []*PathNode {
	var nextPaths []*PathNode
	for _, dirNext := range Directions {
		posNext := Move(p.heading.position, dirNext)
		if !walls[posNext] {
			nextPath := PathNode{}
			nextPath.heading.position = posNext
			nextPath.heading.direction = dirNext
			nextPath.moves = p.moves + 1
			nextPath.turns = p.turns + turns(p.heading.direction, dirNext)
			nextPaths = append(nextPaths, &nextPath)
		}
	}
	return nextPaths
}

func collectPaths(node *PathNode, path *Path, paths *[]Path, endPos Position, endScore uint) {
	currPos := node.heading.position
	*path = append(*path, currPos)
	if currPos == endPos && score(*node) == endScore {
		correctPath := make([]Position, len(*path))
		copy(correctPath, *path)
		*paths = append(*paths, correctPath)
	} else {
		for _, child := range node.children {
			collectPaths(child, path, paths, endPos, endScore)
		}
	}
	*path = (*path)[:len(*path)-1]
}

func getBestPathPositions(p *PathNode, endPos Position, endScore uint) uint {
	var pathsToEnd [][]Position
	var seenPositions = make(map[Position]bool)
	var positionCount uint = 0
	var tempPath Path = Path{}

	collectPaths(p, &tempPath, &pathsToEnd, endPos, endScore)

	for _, path := range pathsToEnd {
		for _, pos := range path {
			_, posSeen := seenPositions[pos]
			if !posSeen {
				positionCount++
				seenPositions[pos] = true
			}
		}
	}
	return positionCount
}

// Main --------------------------------
func main() {
	gameSetup := parseData(dataPath)
	var pathTree PathNode
	pathTree.heading.direction = East
	pathTree.heading.position = gameSetup.start
	currentSearchNodes := []*PathNode{&pathTree}
	nextSearchNodes := []*PathNode{}
	scores := make(map[Heading]uint)
	scores[pathTree.heading] = 0
	var endScore uint = 0
	endScoreFound := false

	// build the tree
	for {
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
