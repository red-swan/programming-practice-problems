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
	West  complex64 = -1i
	South complex64 = 1
	East  complex64 = 1i
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

// Functions ---------------------------------------------------------
// pretty printing of Path type
func (p Path) String() string {
	return fmt.Sprintf("Position: %v  Direction: %v  Score: %v PathLength: %v", p.heading.position, p.heading.direction, p.score, len(p.path))
}

// heap.Interface ---
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
var Directions = [4]complex64{North, South, West, East}

const dataPath = "16.txt"

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

// Main --------------------------------
func main() {
	grid, start, end := parseData(dataPath)
	var bestScore = ^uint(0)
	onPath := make(map[Position]bool)
	scores := make(map[Heading]uint)
	initialSearch := &Path{
		heading: Heading{position: start, direction: East},
		score:   0,
		path:    []complex64{start},
	}
	todo := make(PathHeap, 0)
	heap.Push(&todo, initialSearch)

	for {
		if todo.Len() == 0 {
			break
		}

		currPath := heap.Pop(&todo).(*Path)

		// if we've seen this heading before and we're higher scoring now, skip it
		previousHeadingScore, previouslyScored := scores[currPath.heading]
		if (previouslyScored && previousHeadingScore < currPath.score) ||
			(bestScore < currPath.score) {
			continue
		} else {
			scores[currPath.heading] = currPath.score
		}
		// handle the case of being at the end
		if currPath.heading.position == end && currPath.score <= bestScore {
			bestScore = currPath.score
			for _, pos := range currPath.path {
				onPath[pos] = true
			}
		}

		// build the next search nodes
		type posScoreTuple struct {
			dir   complex64
			delta uint
		}

		for _, next := range [3]posScoreTuple{
			{dir: 1, delta: 1},
			{dir: 1i, delta: 1001},
			{dir: -1i, delta: 1001}} {

			newPath := Path{}
			newPath.score = currPath.score + next.delta
			newPath.heading.direction = currPath.heading.direction * next.dir
			newPath.heading.position = currPath.heading.position + newPath.heading.direction
			if grid[newPath.heading.position] {
				n := len(currPath.path)
				newPositionList := make([]Position, n+1)
				copy(newPositionList, currPath.path)
				newPositionList[n] = newPath.heading.position
				newPath.path = newPositionList
				heap.Push(&todo, &newPath)
			}
		}
	}

	// Part 1
	fmt.Println("Part 1: ", bestScore)

	// Part 2
	fmt.Println("Part 2: ", len(onPath))

}
