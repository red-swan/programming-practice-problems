package main

import (
	"bufio"
	"fmt"
	"io"
	"log"
	"os"
	"strings"
	"unicode/utf8"
)

type position struct {
	x int
	y int
}

type gamestate struct {
	start     position
	bounds    position
	obstacles map[position]bool
}

const (
	Up = iota
	Down
	Left
	Right
)

func TurnRight(dir int) int {
	switch dir {
	case Up:
		return Right
	case Right:
		return Down
	case Down:
		return Left
	case Left:
		return Up
	default:
		panic("Input must be direction")
	}
}
func TurnAround(dir int) int {
	switch dir {
	case Up:
		return Down
	case Right:
		return Left
	case Down:
		return Up
	case Left:
		return Right
	default:
		panic("Input must be direction")
	}
}
func Move(p position, dir int) position {
	switch dir {
	case Up:
		return position{p.x, p.y - 1}
	case Down:
		return position{p.x, p.y + 1}
	case Left:
		return position{p.x - 1, p.y}
	case Right:
		return position{p.x + 1, p.y}
	default:
		panic("Bad direction passed")
	}
}
func PositionRange(p1, p2 position) []position {
	var output []position
	switch {
	case p1.x == p2.x && p1.y == p2.y:
		output = append(output, p1)
	case p1.x == p2.x:
		var delta int
		loopN := max(p1.y, p2.y) - min(p1.y, p2.y)
		if p1.y < p2.y {
			delta = 1
		} else {
			delta = -1
		}
		for i := 0; i < loopN; i++ {
			output = append(output, position{p1.x, p1.y + (delta * i)})
		}

	case p1.y == p2.y:
		var delta int
		loopN := max(p1.x, p2.x) - min(p1.x, p2.x)
		if p1.x < p2.x {
			delta = 1
		} else {
			delta = -1
		}
		for i := 0; i < loopN; i++ {
			output = append(output, position{p1.x + (delta * i), p1.y})
		}
	}
	return output
}

func ReadInput(path string) *gamestate {

	var output gamestate
	output.obstacles = make(map[position]bool)
	output.bounds.y = -1

	// open the file
	f, err := os.OpenFile(path, os.O_RDONLY, os.ModePerm)
	if err != nil {
		log.Fatalf("Failed to open file: %v", err)
	}
	defer f.Close()

	// read the file line by line
	reader := bufio.NewReader(f)
	for {
		// read a new line
		line, err := reader.ReadString('\n')
		if err != nil {
			if err == io.EOF {
				break
			}
			log.Fatalf("read file line error: %v", err)
		} else {
			output.bounds.y++
		}

		// process the line -- -- -- -- -- -- -- -- -- -- -- -- --
		// process the board max width
		if output.bounds.x == 0 {
			output.bounds.x = utf8.RuneCountInString(strings.TrimSpace(line)) - 1
		}
		// find the obstacles or starting point in the line
		x := 0
		y := output.bounds.y
		for _, char := range line {
			switch char {
			case '#':
				output.obstacles[position{x, y}] = true
			case '^':
				output.start.x = x
				output.start.y = y
			}
			x++
		}
	}
	return &output
}

func NextPathPoint(p position, dir int, state *gamestate) position {
	var nextPoint position
	switch dir {
	case Up:
		nextPoint = position{p.x, -1}
	case Down:
		nextPoint = position{p.x, state.bounds.y + 1}
	case Left:
		nextPoint = position{-1, p.y}
	case Right:
		nextPoint = position{state.bounds.x + 1, p.y}
	}

	// find an obstacle to hit
	f := func(obs position) bool {
		switch dir {
		case Up:
			return (p.x == obs.x) && (obs.y < p.y)
		case Down:
			return (p.x == obs.x) && (p.y < obs.y)
		case Left:
			return (p.y == obs.y) && (obs.x < p.x)
		case Right:
			return (p.y == obs.y) && (p.x < obs.x)
		default:
			panic("Bad direction provided")
		}
	}
	g := func(obs position) position {
		switch dir {
		case Up:
			if nextPoint.y < obs.y {
				return obs
			} else {
				return nextPoint
			}
		case Down:
			if nextPoint.y < obs.y {
				return nextPoint
			} else {
				return obs
			}
		case Left:
			if nextPoint.x < obs.x {
				return obs
			} else {
				return nextPoint
			}
		case Right:
			if nextPoint.x < obs.x {
				return nextPoint
			} else {
				return obs
			}
		default:
			panic("Bad direction provided")
		}
	}
	for obstacle := range state.obstacles {
		if f(obstacle) {
			nextPoint = g(obstacle)
		}
	}
	// we don't want the obstacle point, but rather the point just before it
	return Move(nextPoint, TurnAround(dir))
}

func OnBoundary(pos, bounds position) bool {
	return pos.x == 0 || pos.y == 0 || pos.x == bounds.x || pos.y == bounds.y
}

func BuildPathPoints(state *gamestate) ([]position, bool) {
	var path []position
	var loop bool = false
	var currentPosition position = state.start
	var currentDirection int = Up
	type posDir struct {
		p position
		d int
	}
	seenPositions := make(map[posDir]bool)
	for {
		if OnBoundary(currentPosition, state.bounds) {
			break
		}
		// find next path point
		nextPosition := NextPathPoint(currentPosition, currentDirection, state)

		// check if we're in a loop
		if seenPositions[posDir{nextPosition, currentDirection}] {
			loop = true
			break
		}
		// update state
		path = append(path, nextPosition)
		seenPositions[posDir{nextPosition, currentDirection}] = true
		currentPosition = nextPosition
		currentDirection = TurnRight(currentDirection)
	}

	return path, loop
}

func BuildPathFull(state *gamestate) ([]position, bool) {
	var output []position
	p1 := state.start
	keyPoints, loops := BuildPathPoints(state)
	for _, p2 := range keyPoints {
		for _, pathPoint := range PositionRange(p1, p2) {
			output = append(output, pathPoint)
		}
		p1 = p2
	}
	output = append(output, keyPoints[len(keyPoints)-1])
	return output, loops
}

func main() {
	initialState := ReadInput("06.txt")
	initialPath, _ := BuildPathFull(initialState)

	// part 1 ----------------------------------------------------------
	uniquePoints := make(map[position]bool)
	uniquePointCount := 0 // the last, boundary point

	for _, point := range initialPath {
		if !uniquePoints[point] {
			uniquePoints[point] = true
			uniquePointCount++
		}
	}
	fmt.Printf("Part 1: %v\n", uniquePointCount)

	// part 2 ----------------------------------------------------------

	var newObstacles []position
	for point := range uniquePoints {
		// add the new point to the obstacles
		initialState.obstacles[point] = true

		_, doesLoop := BuildPathPoints(initialState)
		if doesLoop {
			newObstacles = append(newObstacles, point)
		}
		// remove the new point from the obstacles
		delete(initialState.obstacles, point)
	}
	fmt.Printf("Part 2: %v\n", len(newObstacles))

}
