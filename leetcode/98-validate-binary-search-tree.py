# Given the root of a binary tree, determine if it is a valid binary search tree (BST).
# A valid BST is defined as follows:
#     The left subtree of a node contains only nodes with keys less than the node's key.
#     The right subtree of a node contains only nodes with keys greater than the node's key.
#     Both the left and right subtrees must also be binary search trees.

# 44ms   - 73.33%
# 16.4MB - 55.96%

from collections import deque

# Definition for a binary tree node.
class TreeNode:
    def __init__(self, val=0, left=None, right=None):
        self.val = val
        self.left = left
        self.right = right

class Solution:
    def check(self,node,low,high):
            if low is not None and node.val <= low:
                return False
            if high is not None and high <= node.val:
                return False
            return True    
            
    def isValidBST(self, root: TreeNode) -> bool:
        queue = deque([(root,None,None)])
        while queue:
            node,low,high = queue.popleft()
            if not self.check(node,low,high):
                return False
            else:
                if node.left:
                    queue.append((node.left,low,node.val))
                if node.right:
                    queue.append((node.right,node.val, high))
        return True
















