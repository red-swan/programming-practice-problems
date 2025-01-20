from random import randint

def in_place_shuffle(input_list):
    n = len(input_list)
    for i in range(n):
        j = randint(i,n-1)
        temp = input_list[j]
        input_list[j] = input_list[i]
        input_list[i] = temp
    return input_list

in_place_shuffle(list(range(10)))


