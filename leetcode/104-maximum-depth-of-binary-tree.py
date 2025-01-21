# Given the root of a binary tree, return its maximum depth.

# A binary tree's maximum depth is the number of nodes along 
# the longest path from the root node down to the farthest leaf node.

# 40 ms - 74.06%
# 16.3MB - 15.60%

# Definition for a binary tree node.
class TreeNode:
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right


class Solution:
    def maxDepth(self, root: TreeNode) -> int:
        if root is None:
            return 0
        def loop(node, acc):
            if node.left is None and node.right is None:
                return (1 + acc)
            elif node.left is not None and node.right is None:
                return loop(node.left,1+acc)
            elif node.left is None and node.right is not None:
                return loop(node.right, 1+acc)
            else:
                return max(loop(node.left, 1+acc), loop(node.right,1+acc))
        return loop(root,0)