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
  let [lhs,rhs] = await parseData()
  lhs.sort()
  rhs.sort()
  
  
  // Part 1
  let difference = 0
  for(let i = 0; i < lhs.length; i++ ){
    // console.log(lhs[i], )
    difference += Math.abs(lhs[i] - rhs[i])
  }
  
  console.log("Part 1: ", difference) 


  // Part 2
  let similarity = 0;
  let i = 0;
  let j = 0;
  while((i !== lhs.length) && (j !== rhs.length)){
    if(lhs[i] < rhs[j]){
      i++;
    } else if(rhs[j] < lhs[i]){
      j++;
    } else {
      similarity += lhs[i];
      j++;
    }
  }
  console.log("Part 2: ", similarity)
}


main()
