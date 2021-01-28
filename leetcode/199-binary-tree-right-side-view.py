# Given a binary tree, imagine yourself standing on the right side of it, 
# return the values of the nodes you can see ordered from top to bottom.

# 44ms - 9.37%
# 14.4MB - 20.04%

# Definition for a binary tree node.
class TreeNode:
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right

class Solution:
    def rightSideView(self, root: TreeNode) -> list[int]:
        if root is None:
            return []
        acc = []
        def loop(tree, level, acc):
            n = len(acc)
            if n < level:
                acc.append(tree.val)
            if tree.right:
                loop(tree.right, level + 1, acc)
            if tree.left:
                loop(tree.left, level + 1, acc)
        loop(root, 1, acc)
        return acc