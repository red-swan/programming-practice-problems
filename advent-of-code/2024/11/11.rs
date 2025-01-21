use std::{collections::HashMap, u64};


fn digits(n: u64) -> u32 {
  1 + n.ilog10()
}

fn evenDigits(n: u64) -> bool {
  digits(n) % 2 == 0
}

fn splitDigits(n: u64) -> (u64,u64) {
  fn findDivisor(x: u64,b: u64,d: u64) -> u64 {
    if d < x / d {
      findDivisor(x, b, b*d)
    } else {
      d
    }
  }

  let d = findDivisor(n, 10, 10);  
  (n / d, n % d)
}

fn blink(n: u64) -> (u64,u64,u64) {
  if n == 0 {
    (1,1,0)
  } else if evenDigits(n) {
    let (a,b) = splitDigits(n);
    (2,a,b)
  } else {
    (1, n * 2024,0)
  }
}

fn countNextLevel(counts: HashMap<u64,u64>) -> HashMap<u64,u64> {
  let mut output = HashMap::new();
  for (stone,count) in counts.into_iter() {
    let (i,a,b) = blink(stone);
    let newCount = output.entry(a).or_insert(0);
    *newCount += count;
    if i == 2 {
      let newCount = output.entry(b).or_insert(0);
      *newCount += count;
    }
  }
  output
}

fn countLevel(stones: &Vec<u64>, n: u64) -> u64 {
  let initialMap: HashMap<u64,u64> = stones.iter().map(|x| (x.clone(),1)).collect();
  let finalLevel = (0..n).fold(initialMap,|acc,_| countNextLevel(acc));
  finalLevel.into_iter().fold(0,|acc, (_,v)| acc + v)
  
}

fn main() {

    let stones = vec![9694820, 93, 54276, 1304, 314, 664481, 0, 4];
    
    // Part 1
    let part1 = countLevel(&stones,25);
    println!("Part 1: {}", part1);

    // Part 2
    let part2 = countLevel(&stones,75);
    println!("Part 2: {}", part2);
}
