// Environment -------------------------
const fs = require('fs')
const readline = require('readline')

// Functions ---------------------------
function identityOrObjectString(x) {
  return typeof x === 'object' ? JSON.stringify(x) : x;
}

function heappush(heap, newKey, f){
  // push the new key 
  heap.push(newKey);

  // get the current index of pushed key
  let curr = heap.length-1;

 // keep comparing till root is reached or we terminate in middle
  while(curr > 0){
    let parent = Math.floor((curr-1)/2)
    if( f(heap[curr]) < f(heap[parent]) ){
      // quick swap
      [ heap[curr], heap[parent] ] = [ heap[parent], heap[curr] ]
      // update the index of newKey
      curr = parent
    } else{
      // if no swap, break, since we heap is stable now
      break
    }
  } 
}

function heappop(heap, f){
  // swap root with last node
  const n = heap.length;
  [heap[0], heap[n-1]] = [ heap[n-1], heap[0]]

  // remove the root i.e. the last item (because of swap)
  const removedKey = heap.pop();

  let curr = 0;

  // keep going till atleast left child is possible for current node
  while(2*curr + 1 < heap.length){
    const leftIndex = 2*curr+1; 
    const rightIndex = 2*curr+2;
    const minChildIndex = (rightIndex < heap.length && heap[rightIndex] < heap[leftIndex] ) ? rightIndex :leftIndex;
    if(f(heap[minChildIndex]) < f(heap[curr])){
     // quick swap, if smaller of two children is smaller than the parent (min-heap)
      [heap[minChildIndex], heap[curr]] = [heap[curr], heap[minChildIndex]]
      curr = minChildIndex
    } else {
      break
    }
  }

  // finally return the removed key
  return removedKey;
}

function move(heading, direction) {
  let output = new Object();
  if((heading.dir == "North" && direction == "Left") ||
     (heading.dir == "South" && direction == "Right") ||
     (heading.dir == "West" && direction == "Forward")){
    output.dir = "West"
    output.pos = {x: heading.pos.x - 1, y: heading.pos.y}
  } else if ((heading.dir == "East" && direction == "Left") ||
             (heading.dir == "West" && direction == "Right") ||
             (heading.dir == "North" && direction == "Forward")){
      output.dir = "North"
      output.pos = {x: heading.pos.x, y: heading.pos.y - 1}
  } else if ((heading.dir == "South" && direction == "Left") ||
             (heading.dir == "North" && direction == "Right") ||
             (heading.dir == "East" && direction == "Forward")){
      output.dir = "East"
      output.pos = {x: heading.pos.x + 1, y: heading.pos.y}
  } else if ((heading.dir == "West" && direction == "Left") ||
             (heading.dir == "East" && direction == "Right") ||
             (heading.dir == "South" && direction == "Forward")){
      output.dir = "South"
      output.pos = {x: heading.pos.x, y: heading.pos.y + 1}
  } else {
    console.log("Case Not Found: ", heading, direction)
  }


  return output
}


// Types -------------------------------
class ObjectSet extends Set{
  add(elem){
    return super.add(identityOrObjectString(elem));
  }
  has(elem){
    return super.has(identityOrObjectString(elem));
  }
}

class ObjectDefaultMap extends Map{
  constructor(def) {
    super();
    this.def = def
  }
  set(key, value){
    let keyStr = identityOrObjectString(key);
    let valueStr = identityOrObjectString(value);
    return super.set(keyStr, valueStr);
  }
  has(key){
    return super.has(identityOrObjectString(key));
  }
  get(key){
    let keyStr = identityOrObjectString(key);
    if(this.has(keyStr)){
      return super.get(keyStr);
    } else {
      return this.def;
    }
  }
}




// Globals -----------------------------
const dataPath = '16.txt'
let grid = new ObjectDefaultMap(".");
let start;
let seen = new ObjectSet();
let best = Number.MAX_SAFE_INTEGER;
// headings
let scores = new ObjectDefaultMap(best);
let t = 0;


// Functions ---------------------------
async function parseData(path) {
  const fileStream = fs.createReadStream(dataPath);
  const rl = readline.createInterface({
    input: fileStream,
    crlfDelay: Infinity
  });

  let x;
  let y = 0;

  for await (const line of rl) {
    x = 0;
    for (const c of line){
      switch(c) {
        case "#":
          break;
        case "S":
          start = {x: x, y: y}
        default:
            grid.set({x: x, y: y}, c);
      }
      x++;
    }
    y++;
  }
}



// Main --------------------------------
async function main() {
  await parseData(dataPath);
  let todo =[{score: 0, t: t, pos: start, dir: "East", path: [start]}]
  let f = function(x) {return x.score}

  while(todo.length){
    
    let data = heappop(todo, f);
    
    let heading = {pos: data.pos, dir:data.dir};
    if(scores.get(heading) < data.score) {
      continue;
    }
    scores.set(heading,data.score);

    if(grid.get(heading.pos) === "E" && data.score <= best){
      
      for(const position of data.path){
        seen.add(position);
      }
      best = data.score;
    }

    for(const dirScore of [{dir: "Left", deltaScore: 1001},
                           {dir: "Right", deltaScore: 1001},
                           {dir: "Forward", deltaScore: 1}]){
      let newHeading = move(heading, dirScore.dir);
      if(!grid.has(newHeading.pos)){
        continue;
      }
      let newData = new Object();
      newData.score = data.score + dirScore.deltaScore;
      newData.t = ++t;
      newData.pos = newHeading.pos;
      newData.dir = newHeading.dir;
      newData.path = [...data.path, newHeading.pos];
      heappush(todo, newData, f);
    }
  }

  console.log(best)
  console.log(seen.size)
  

}

main()