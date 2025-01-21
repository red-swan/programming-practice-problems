sample1 = [-10, -10, 1, 3,2]
sample2 = [1, 10, -5, 1, -100]

def get_highest_three(list_of_ints):

    high_pos = max(list_of_ints[0], list_of_ints[1])
    low_neg = min(list_of_ints[0], list_of_ints[1])
    high_prod2 = list_of_ints[0] * list_of_ints[1]
    low_prod2 = list_of_ints[0] * list_of_ints[1]
    largest_prod3 = list_of_ints[0] * list_of_ints[1] * list_of_ints[2]

    for i in range(2,len(list_of_ints)):
        num = list_of_ints[i]
        # does number create new high product
        largest_prod3 = max(high_prod2 * num, low_prod2 * num, largest_prod3)
        # update tracked products of high/low
        high_prod2 = max(high_prod2, high_pos * num)
        low_prod2  = min( low_prod2, low_neg * num)
        # update high and low
        high_pos = max(high_pos, num)
        low_neg = min(low_neg, num)
        
        
    
    return largest_prod3

get_highest_three(sample1)
get_highest_three(sample2)














