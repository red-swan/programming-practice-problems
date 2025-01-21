package main

import (
	"fmt"
	"math"
)

func digits(x uint64) uint64 {
	return 1 + uint64(math.Floor(math.Log10(float64(x))))
}

func evenDigits(x uint64) bool {
	return digits(x)%2 == 0
}

func add(x uint64) func(uint64) uint64 {
	f := func(y uint64) uint64 {
		return x + y
	}
	return f
}

func findDivisor(x, b, d uint64) uint64 {
	if d < x/d {
		return findDivisor(x, b, (b * d))
	} else {
		return d
	}
}

func splitDigits(x uint64) (uint64, uint64) {
	d := findDivisor(x, 10, 10)
	return x / d, x % d
}

func blink(x uint64) (uint64, uint64, uint64) {
	if x == 0 {
		return 1, 1, 0
	} else if evenDigits(x) {
		a, b := splitDigits(x)
		return 2, a, b
	} else {
		return 1, x * 2024, 0
	}
}

func stonesToMap(stones []uint64) map[uint64]uint64 {
	output := make(map[uint64]uint64)
	for _, stone := range stones {
		output[stone] = 1
	}
	return output
}

func blinkStoneMap(stonemap map[uint64]uint64) map[uint64]uint64 {
	output := make(map[uint64]uint64)
	for stone, count := range stonemap {
		i, a, b := blink(stone)
		output[a] += count
		if i == 2 {
			output[b] += count
		}
	}
	return output
}

func blinkStoneMapN(stonemap map[uint64]uint64, n uint64) map[uint64]uint64 {
	output := stonemap
	for range n {
		output = blinkStoneMap(output)
	}
	return output
}

func count(stonemap map[uint64]uint64) uint64 {
	var output uint64 = 0
	for _, v := range stonemap {
		output += v
	}
	return output
}

func main() {

	stones := []uint64{9694820, 93, 54276, 1304, 314, 664481, 0, 4}
	stonemap := stonesToMap(stones)

	// Part 1
	fmt.Printf("Part 1: %v\n", count(blinkStoneMapN(stonemap, 25)))

	// Part 2
	fmt.Printf("Part 2: %v\n", count(blinkStoneMapN(stonemap, 75)))

}
