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

type SearchNode struct {
	heading Heading
	moves   uint
	turns   uint
}

// Globals -----------------------------------------------------------
var Directions = [4]int{North, South, West, East}

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

func scoreNode(s SearchNode) uint {
	return s.moves + 1000*s.turns
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
func MoveHeading(h Heading) Position {
	return Move(h.position, h.direction)
}

func getPossibleNextNodes(walls map[Position]bool, n SearchNode) []SearchNode {
	var output []SearchNode
	for _, dirNext := range Directions {
		posNext := Move(n.heading.position, dirNext)
		if !walls[posNext] {
			headingNext := Heading{position: posNext, direction: dirNext}
			nodeNext := SearchNode{heading: headingNext, moves: n.moves + 1, turns: n.turns + turns(n.heading.direction, dirNext)}
			output = append(output, nodeNext)
		}
	}
	return output
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

// Main --------------------------------
func main() {
	gameSetup := parseData("16.txt")
	searchNodes := []SearchNode{
		{heading: Heading{position: gameSetup.start, direction: East},
			moves: 0,
			turns: 0}}

	nextSearchNodes := []SearchNode{}
	scores := make(map[Position]uint)
	scores[gameSetup.start] = 0

	for {
		// if there's no new positions, we're done
		if len(searchNodes) == 0 {
			break
		}
		// iterate through the positions we're at currently
		for _, currentNode := range searchNodes {
			// get the score we're at currently
			// get the next possible headings
			nextPossibleNodes := getPossibleNextNodes(gameSetup.walls, currentNode)
			// fmt.Println(currentHeading, "\t", nextPossibleHeadings)
			// iterate through the next possible headings
			for _, nextPossibleNode := range nextPossibleNodes {
				// score the heading
				scoreNext := scoreNode(nextPossibleNode)

				scoreNextAlready, scoredAlreadyNext := scores[nextPossibleNode.heading.position]
				if !scoredAlreadyNext || scoreNext < scoreNextAlready {
					scores[nextPossibleNode.heading.position] = scoreNext
					nextSearchNodes = append(nextSearchNodes, nextPossibleNode)
				}
			}

		}
		searchNodes = nextSearchNodes
		nextSearchNodes = []SearchNode{}
	}

	// Part 1
	f := func(x, y int) {
		p := Position{x, y}
		fmt.Println(p, scores[p])
	}
	fmt.Println("Part 1: ", scores[gameSetup.end])

}
