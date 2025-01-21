# Given a binary tree, return the level order traversal of its nodes' values. 
# (ie, from left to right, level by level).

# 32ms   - 83.23%
# 14.6MB - 48.00%

from collections import deque

# Definition for a binary tree node.
class TreeNode:
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right


class Solution:
    def levelOrder(self, root: TreeNode) -> list[list[int]]:
        if root is None:
            return []
        curr_level = [root]
        prev_levels = []
        output = []
        while curr_level:
            prev_levels.append(curr_level)
            curr_level = []
            for node in prev_levels[-1]:
                if node.left:
                    curr_level.append(node.left)
                if node.right:
                    curr_level.append(node.right)
        for i in prev_levels:
            output.append([node.val for node in i])

        return output
