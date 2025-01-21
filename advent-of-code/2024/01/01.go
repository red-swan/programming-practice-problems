package main

// Sources:
// https://stackoverflow.com/questions/8757389/reading-a-file-line-by-line-in-go

import (
	"bufio"
	"fmt"
	"io"
	"log"
	"os"
	"regexp"
	"slices"
	"strconv"
)

func main() {
	dataFile := "01.txt"
	lhs, rhs := ReadInput(dataFile)

	f := func(a, b int) int { return a - b }

	slices.SortFunc(lhs, f)
	slices.SortFunc(rhs, f)

	fmt.Println("Part 1: ", MeasureDifference(lhs, rhs))
	fmt.Println("Part 2: ", MeasureSimilarity(lhs, rhs))

}

func MeasureDifference(arr1, arr2 []int) uint {
	var output uint = 0
	if len(arr1) != len(arr2) {
		panic("arrays of differing lengths")
	}

	for i := range arr1 {
		x1 := arr1[i]
		x2 := arr2[i]
		if x1 < x2 {
			output += uint(x2 - x1)
		} else {
			output += uint(x1 - x2)
		}
	}
	return output
}

func MeasureSimilarity(arr1, arr2 []int) int {
	similarity := 0
	rhsCounts := make(map[int]int)

	for _, v := range arr2 {
		rhsCounts[v] += 1
	}

	for _, v := range arr1 {
		similarity += v * rhsCounts[v]
	}

	return similarity
}

func ReadInput(path string) ([]int, []int) {
	var lhs []int
	var rhs []int

	f, err := os.OpenFile(path, os.O_RDONLY, os.ModePerm)
	if err != nil {
		log.Fatalf("open file error: %v", err)
		return lhs, rhs
	}
	defer f.Close()

	reader := bufio.NewReader(f)
	for {
		line, err := reader.ReadString('\n')
		if err != nil {
			if err == io.EOF {
				break
			}
			log.Fatalf("read file line error: %v", err)
			return lhs, rhs
		}
		re := regexp.MustCompile(`\d+`)
		numbers := re.FindAllString(line, -1)
		if len(numbers) != 2 {
			log.Fatalf("Did not find two numbers: %v", line)
		}

		leftNum, err := strconv.Atoi(numbers[0])
		if err != nil {
			log.Fatalf("Could not parse lhs: %v", err)
		}
		rightNum, err := strconv.Atoi(numbers[1])
		if err != nil {
			log.Fatalf("Could not parse rhs: %v", err)
		}
		lhs = append(lhs, leftNum)
		rhs = append(rhs, rightNum)

	}
	return lhs, rhs
}
