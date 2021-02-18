# Given an array of integers sorted in ascending order, return the 
# starting and ending index of a given target value in an array, i.e. 
# [x, y]

def _binary_search(nums, left, right, target):
  while left <= right:
    mid = (left + right) // 2
    foundVal = nums[mid]
    if (foundVal == target):
      return mid
    elif foundVal < target:
      left = mid + 1
    else:
      right = mid - 1

  return None

def binary_search(items, target):
    return _binary_search(items, 0, len(items) - 1, target)

def search_range(nums, target):
    if len(nums) < 1:
        return None
  
    first_pos = _binary_search(nums, 0, len(nums) - 1, target)

    if first_pos is None:
        return None

    end_pos = first_pos
    start_pos = first_pos

    while start_pos is not None:
        temp1 = start_pos
        start_pos = _binary_search(nums, 0, start_pos - 1, target)
    start_pos = temp1

    while end_pos is not None:
        temp2 = end_pos
        end_pos = _binary_search(nums, end_pos + 1, len(nums) - 1, target)
    end_pos = temp2

    return [start_pos, end_pos]

sample1 = [1,3,3,5,5,5,8,9]


list(map(lambda x: binary_search(sample1,x),range(1,10)))
binary_search(sample1,9)
search_range(sample1, 5)