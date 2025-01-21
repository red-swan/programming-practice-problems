# Given the root of a complete binary tree, return the number of the nodes in the tree.

# According to Wikipedia, every level, except possibly the last, is completely filled 
# in a complete binary tree, and all nodes in the last level are as far left as possible. 
# It can have between 1 and 2h nodes inclusive at the last level h.

# 64ms - 98.20%
# 21.4MB - 79.40%

from math import ceil

# Definition for a binary tree node.
class TreeNode:
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right
class Solution:
    def exists(self, root: TreeNode, height, idx) -> bool:
        right = 2 ** (height - 1)
        if idx < 1 or right < idx:
            return False
        current_level = 1
        left = 1
        node = root
        while current_level < height:
            mid = ceil((left + right) / 2)
            if mid <= idx and node.right:
                node = node.right
                current_level += 1
                left = mid
            elif idx < mid and node.left:
                node = node.left
                current_level += 1
                right = mid - 1
            else:
                return False

        return True
        
    def countNodes(self, root: TreeNode) -> int:
        if root is None:
            return 0
        # compute height
        height = 0
        node = root
        while node:
            height += 1
            node = node.left
        # compute count
        left = 1
        right = 2 ** (height - 1)
        while left < right:
            mid = ceil((left + right) / 2)
            if self.exists(root,height,mid):
                left = mid
            else:
                right = mid - 1
        return (2 ** (height - 1) - 1) + left


