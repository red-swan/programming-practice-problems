
my_list     = [3, 4, 6, 10, 11, 15]
alices_list = [1, 5, 8, 12, 14, 19]



def merge_lists(list1,list2):
    n1 = len(list1)
    n2 = len(list2)
    if n1 == 0: return list2
    if n2 == 0: return list1

    output = [None] * (n1+n2)
    done = False
    i0 = i1 = i2 = 0
    
    while not done:
        if list1[i1] < list2[i2]:
            output[i0] = list1[i1]
            i1 += 1
        else:
            output[i0] = list2[i2]
            i2 += 1
        i0 +=1        
        if n1 <= i1:
            output[i0:] = list2[i2:]
            done = True
        if n2 <= i2:
            output[i0:] = list1[i1:]
            done = True
        if i0 == n1 + n2:
            done = True
    return output



merge_lists(my_list, alices_list)




# What if we wanted to merge several sorted lists? 
# Write a function that takes as an input a list of sorted lists and outputs a single 
# sorted list with all the items from each list.

# Do we absolutely have to allocate a new list to use for the merged output?
#  Where else could we store our merged list? How would our function need to change?
