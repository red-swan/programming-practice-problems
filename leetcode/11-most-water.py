#array
#shifting-pointer

# Given n non-negative integers a1, a2, ..., an , where each 
# represents a point at coordinate (i, ai). n vertical lines 
# are drawn such that the two endpoints of the line i is at 
# (i, ai) and (i, 0). Find two lines, which, together with 
# the x-axis forms a container, such that the container 
# contains the most water.

def brute(nums):
    n = len(nums)
    max_area = 0
    for i in range(n-1):
        for j in range(1,n):
            height = min(nums[i], nums[j])
            width = j - i
            area = height * width
            max_area = max(max_area, area)
    return max_area



def mostWater(height):
    max_area = 0
    a = 0
    b = len(height) - 1
    while a < b:
        max_area = max(max_area, min(height[a], height[b]) * (b-a) )
        if height[a] < height[b]:
            a += 1
        else:
            b -= 1

    return max_area

sample1 = [7,1,2,3,9]
sample2 = [6,9,3,4,5,8]
sample3 = []
sample4 = [5]
sample5 = [1,8,6,2,5,4,8,3,7]
sample6 = [4,8,1,2,3,9]
sample7 = [4,8,1,1000,1001,2,3,9]
all_samples = [sample1,sample2,sample3,sample4, sample5, sample6, sample7]


list(map(brute,all_samples))

list(map(mostWater,all_samples))


