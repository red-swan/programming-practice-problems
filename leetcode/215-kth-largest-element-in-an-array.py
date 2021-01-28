# Find the kth largest element in an unsorted array. Note that it is the kth largest 
# element in the sorted order, not the kth distinct element.

# 2720 ms - 5.00%
# 19.1MB  - 8.00%

class Solution:

    def swap(self,arr,i,j):
        temp = arr[i]
        arr[i] = arr[j]
        arr[j] = temp

    def sortStep(self,items, left, right):
        i = left
        for j in range(left,right+1):
            if items[j] <= items[right]:
                self.swap(items,i,j)
                i += 1
        return i - 1

    def _quickselect(self,items,left,right,k):
        partition_i = self.sortStep(items,left,right)
        if partition_i == k - 1:
            return items[k-1]
        elif k - 1 < partition_i:
            return self._quickselect(items, left, partition_i - 1, k)
        else:
            return self._quickselect(items, partition_i + 1, right, k)

    def findKthLargest(self, nums, k: int) -> int:
        return self._quickselect(nums, 0, len(nums) - 1, len(nums) - k+1)









s1 = [3,2,1,5,6,4] # k = 2 => output = 5
s2 =  [3,2,3,1,2,4,5,5,6] # k = 4 => output = 4

[Solution().findKthLargest(l,k) for l,k in [(s1,2),(s2,4)]]