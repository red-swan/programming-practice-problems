// Environment -------------------------
const fs = require('fs')
const readline = require('readline')

// Globals -----------------------------
const dataPath = '01.txt'

// Functions ---------------------------
async function parseData(path) {
  const fileStream = fs.createReadStream(dataPath);

  const rl = readline.createInterface({
    input: fileStream,
    crlfDelay: Infinity
  });

  let lhs = new Array();
  let rhs = new Array();
  let digitStr = /\d+/g

  for await (const line of rl) {
    // Each line in input.txt will be successively available here as `line`.
    let [l,r] = [...line.matchAll(digitStr)]
    lhs.push(Number(l))
    rhs.push(Number(r))
  }

  return [lhs,rhs]
}


async function main() {
  let [lhs,rhs] = await parseData();
  lhs.sort();
  rhs.sort();
  
  // Part 1
  let difference = 0
  for(let i = 0; i < lhs.length; i++ ){
    // console.log(lhs[i], )
    difference += Math.abs(lhs[i] - rhs[i])
  }
  
  console.log("Part 1: ", difference) 


  // Part 2
  let rhsCounts = new Map();
  for(let n of rhs){
    if(rhsCounts.has(n)){
      rhsCounts.set(n, rhsCounts.get(n) + 1);
    } else {
      rhsCounts.set(n,1);
    }
  }

  let similarity = 0;
  for(let n of lhs){
    if(rhsCounts.has(n)){
      similarity += n * rhsCounts.get(n)
    }
  }
  console.log("Part 2: ", similarity)
}


main()
