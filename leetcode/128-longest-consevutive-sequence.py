# Given an unsorted array of integers nums, return the length of the 
# longest consecutive elements sequence.

# 56ms   - 67.06%
# 15.4MB - 22.78%

class Solution:
    def longestConsecutive(self, nums: list[int]) -> int:
        num_set = set(nums)
        longest_length = 0
        for num in num_set:
            if num - 1 not in num_set:
                current_length = 1
                while num + 1 in num_set:
                    num += 1
                    current_length += 1
                longest_length = max(longest_length, current_length)
        return longest_length



s1 = [100,4,200,1,3,2] # 4
s2 = [0,3,7,2,5,8,4,6,0,1] # 9

[Solution().longestConsecutive(s) for s in [s1,s2]]