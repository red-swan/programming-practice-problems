from functools import reduce


def find_repeat(numbers_list):
    n = len(numbers_list) - 1    
    return abs(reduce(lambda sum, n: sum - n, numbers_list, (n*n+n)/2))