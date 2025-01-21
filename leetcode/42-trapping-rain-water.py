#array
#shifting-pointer

# Given n non-negative integers representing an elevation map
# where the width of each bar is 1, compute how much water it 
# can trap after raining.

from itertools import accumulate

sample0 = [0,1,0,2,1,0,3,1,0,1,2]
sample1 = [0,1,0,2,1,0,1,3,2,1,2,1]
sample2 = [4,2,0,3,2,5]
sample3 = [3,4,3]
sample4 = []
sample5 = [6]
sample6 = [5,0,3,0,0,0,2,3,4,2,1]
all_samples = [sample0, sample1, sample2,sample3, sample4, sample5,sample6]

def cumComp(arr,f): 
    return list(accumulate(arr, f))
def cum_min(arr):
    return cumComp(arr, min)
def cum_max(arr):
    return cumComp(arr, max)

# this method
# first 
#       calculates the max left and max right for each value
#       and creating arrays of these values
# second
#       calculates if we're in a pit and adds the current water
#       height to the area
# think integration
# Complexity:
# Time: O(n) - loops through once
# Space: O(n) - creates new arrays of maxes
def trap(height):
    area = 0
    left_maxes = cum_max(height)
    right_maxes = list(reversed(cum_max(reversed(height))))
    for i,h in enumerate(height):
        area += max(0, min(left_maxes[i], right_maxes[i]) - h)

    return area

list(map(trap,all_samples))

# sample0 = [0,1,0,2,1,0,3,1,0,1,2]
# let's make it faster
# Time: O(n)
# Space: O(1)
def trap(height):
    i_left = 0
    i_right = len(height) - 1
    left_max = 0
    right_max = 0
    area = 0
    while i_left < i_right:
        if height[i_left] < height[i_right]:
            if left_max <= height[i_left]:
                left_max = height[i_left]
            else:
                area += left_max - height[i_left]
            i_left += 1
        else:
            if right_max <= height[i_right]:
                right_max = height[i_right]
            else:
                area += right_max - height[i_right]
            i_right -= 1
    return area
# list(map(trap,all_samples))
# trap(sample0)
trap([8,5,9,3,6])





