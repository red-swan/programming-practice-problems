
const quotient = (x, d) => Math.trunc(x / d)
const digits = n => 1 + Math.floor(Math.log10(n))
const hasEvenDigits = n => (digits(n) % 2 === 0)
const findDivisor = (x,b,d) => {
  if (d < quotient(x,d)) {
    return findDivisor(x, b, d*b)
  } else {
    return d
  }
}
const splitDigits = x => {
  let d = findDivisor(x,10,10)
  let q = quotient(x, d)
  let r = x % d
  return [q, r]
}
const blink = x => {
  if (x === 0){
    return [1]
  } else if (hasEvenDigits(x)) {
    return splitDigits(x)
  } else {
    return [x * 2024]
  }
}

const stonesToMap = stones => {
  let output = new Map()
  for (const stone of stones) {
    output.set(stone,1)
  }
  return output
}

const countNextBlink = stoneCounts => {
  let output = new Map()
  for (const [stone, count] of stoneCounts) {
    let nextStones = blink(stone)
    for (const nextStone of nextStones) {
      if (output.has(nextStone)) {
        output.set(nextStone,output.get(nextStone) + count)
      } else {
        output.set(nextStone, count)
      }
    }
  }
  return output
}

const countNBlinks = (stones,n) => {
  let stoneCounts = stonesToMap(stones)
  for (let i = 0; i < n; i++){
    stoneCounts = countNextBlink(stoneCounts)
  }
  let output = 0
  for(const [stone, count] of stoneCounts){
    output += count
  }
  return output
}

let stones = [9694820, 93, 54276, 1304, 314, 664481, 0, 4]


console.log("Part 1: ", countNBlinks(stones,25))
console.log("Part 2: ", countNBlinks(stones,75))
