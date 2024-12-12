
struct Stone {
  number: u32
  blinked: Vec<Stone>
}




fn main() {

  let initialStones = [0, 1, 10, 99, 999];
  let stones: Vec<Stone> = initialStones.into_iter().map(|&x| Stone{number: x, blinked: Vec::new()}).collect();
  // let stones = [125, 17]
  // let stones = [9694820, 93, 54276, 1304, 314, 664481, 0, 4]


  println!("Hello World!");

}