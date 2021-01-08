

def two_sum(nums, target):
    seen = {}
    for i,num in enumerate(nums):
        if target - num in seen:
            return [i, seen[target - num]]
        else:
            seen[num] = i










