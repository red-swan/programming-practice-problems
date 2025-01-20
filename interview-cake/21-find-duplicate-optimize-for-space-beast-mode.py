# the integers are in the range 1..n
# the list has a length of n+1

def walk_array(array, steps, start_position):
    position = array[start_position - 1]
    for i in range(steps):
        position = array[position - 1]
    return position

def find_cycle_length(array, start_position):
    position = array[start_position - 1]
    step = 1
    while position != start_position:
        position = array[position - 1]
        step += 1
    return step

def find_first_in_cycle(array,cycle_length):
    position1 = array[len(array) - 1]
    position2 = walk_array(array, cycle_length, len(array))
    while position1 != position2:
        position1 = array[position1 - 1]
        position2 = array[position2 - 1]
    return position1

def find_duplicate(array):
    position_in_cycle = walk_array(array,len(array) - 1, len(array))
    cycle_length = find_cycle_length(array, position_in_cycle)
    duplicate = find_first_in_cycle(array, cycle_length)
    return duplicate

sample1 = [1,4,7,2,4,9,8,3,5]
sample2 = [3, 4, 2, 3, 1, 5]
sample3 = [3, 1, 2, 2]
sample4 = [4, 3, 1, 1, 4]


[find_duplicate(x) for x in [sample1,sample2,sample3,sample4]]
























