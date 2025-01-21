# Given a binary tree, imagine yourself standing on the right side of it, 
# return the values of the nodes you can see ordered from top to bottom.

# 44ms - 9.37%
# 14.4MB - 20.04%

from collections import deque
from collections import OrderedDict

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
        queue = deque()
        output = OrderedDict()
        queue.append((root, 1))
        while queue:
            node,level = queue.popleft()
            if node.left:
                queue.append((node.left, level + 1))
            if node.right:
                queue.append((node.right,level + 1))
            output[level] = node #
        return [node.val for _,node in output.items()]



