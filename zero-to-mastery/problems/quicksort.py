def swap(arr,i,j):
    temp = arr[i]
    arr[i] = arr[j]
    arr[j] = temp

def sortStep(items, left, right):
    i = left
    for j in range(left,right+1):
        if items[j] <= items[right]:
            swap(items,i,j)
            i += 1
    return i - 1

def _quicksort(items, left, right):
    if left < right:
        partition_index = sortStep(items, left, right)
        _quicksort(items, left, partition_index - 1)
        _quicksort(items, partition_index + 1, right)
def quicksort(items):
    _quicksort(items, 0, len(items) - 1)

def _quickselect(items,left,right,k):
    partition_i = sortStep(items,left,right)
    if partition_i == k - 1:
        return items[k-1]
    elif k - 1 < partition_i:
        return _quickselect(items, left, partition_i - 1, k)
    else:
        return _quickselect(items, partition_i + 1, right, k)
def quickselect(items,k):
    return _quickselect(items, 0, len(items) - 1, k)

sample1 = [5,3,1,6,4,2]

quickselect(sample1, 4)
quicksort(sample1)
sample1











