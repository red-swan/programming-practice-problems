# Given an array of integers nums sorted in ascending order, 
# find the starting and ending position of a given target value.

# If target is not found in the array, return [-1, -1].

# Follow up: Could you write an algorithm with O(log n) runtime complexity?



# Given an array of integers sorted in ascending order, return the 
# starting and ending index of a given target value in an array, i.e. 
# [x, y]

# 124 ms - 6.31%
# 15.5 MB - 52.45 %

class Solution:
    def _binary_search(self,nums, left, right, target):
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

    def binary_search(self,items, target):
        return self._binary_search(items, 0, len(items) - 1, target)

    def searchRange(self, nums, target: int) :
        if len(nums) < 1:
            return [-1,-1]
    
        first_pos = self._binary_search(nums, 0, len(nums) - 1, target)

        if first_pos is None:
            return [-1,-1]

        end_pos = first_pos
        start_pos = first_pos

        while start_pos is not None:
            temp1 = start_pos
            start_pos = self._binary_search(nums, 0, start_pos - 1, target)
        start_pos = temp1

        while end_pos is not None:
            temp2 = end_pos
            end_pos = self._binary_search(nums, end_pos + 1, len(nums) - 1, target)
        end_pos = temp2

        return [start_pos, end_pos]
    
        


[Solution().searchRange(l,t) for l,t in [([5,7,7,8,8,10],8),([5,7,7,8,8,10],6),([],0)]]




