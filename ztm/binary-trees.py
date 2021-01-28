# 1. Given a binary tree, find its maximum depth.
# 2. Given a binary tree, return the level order traversal of the nodes' values as an array
# 3. Given a binary tree, imagine you're standing to the right of the tree.
#    Return an array of the values of the nodes you can see ordered from 
#    top to bottom
# 4. Given a complete binary tree, count the number of nodes
# 5. Given a tree, determine if it is a valid binary search tree

from collections import deque
from math import ceil
from itertools import chain

class BinaryTreeNode(object):

    def __init__(self, input):
        if type(input) is tuple:
            self._from_tuple(input)
        elif type(input) is list:
            self._from_list(input)
        else:
            self._from_value(input)
    def _from_value(self,value):
        self.value = value
        self.left = None
        self.right = None
    def _from_tuple(self, nested_tuple):
        self.value = nested_tuple[0]
        if nested_tuple[1] is not None:
            self.left = BinaryTreeNode(nested_tuple[1])
        else:
            self.left = None
        if nested_tuple[2] is not None:
            self.right = BinaryTreeNode(nested_tuple[2])
        else:
            self.right = None
    def _from_list(self, input_list):
        if len(input_list) == 0:
            self._from_value(None)
        else:
            self.value = input_list[0]
            self.left = None
            self.right = None
            queue = deque([self])
            for item in input_list[1:]:
                new_node = BinaryTreeNode(item)
                node = queue[0]
                if node.left:
                    node.right = new_node
                    queue.popleft()
                    queue.append(node.left)
                    queue.append(node.right)
                else:
                    node.left = new_node

                

    def _max_depth_loop(self,acc):
        if self.left is None and self.right is None:
            return (1 + acc)
        elif self.left is not None and self.right is None:
            return self.left._max_depth_loop(1+acc)
        elif self.left is None and self.right is not None:
            return self.right._max_depth_loop(1+acc)
        else:
            return max(self.left._max_depth_loop(1+acc), self.right._max_depth_loop(1+acc))
    def get_max_depth(self):
        return self._max_depth_loop(0)

    def get_levels(self):
        queue = deque([self])
        node_count_on_level = len(queue)
        level = 1
        current_level_items = []
        levels = []
        while queue:
            node_count_on_level = len(queue)
            for _ in range(node_count_on_level):
                node = queue.popleft()
                if node:
                    current_level_items.append(node.value)
                    if node.left:
                        queue.append(node.left)
                    if node.right:
                        queue.append(node.right)
                    # we're in a new level
            level += 1
            levels.append(current_level_items)
            current_level_items = []
        return levels

    def get_value(self,height,idx):
        bottom_count = 2 ** (height - 1)
        if idx < 1 or bottom_count < idx:
            return None
        def loop(tree, current_level, left, right):
            if current_level == height:
                return tree.value
            else:
                mid = ceil((left + right) / 2)
                if mid <= idx and tree.right:
                    return loop(tree.right,current_level + 1, mid, right)
                elif idx < mid and tree.left:
                    return loop(tree.left, current_level + 1, left, mid - 1)
                else:
                    return None

        return loop(self,1,1, bottom_count)

    def exists(self,height,idx):
        if self.get_value(height,idx):
            return True
        else:
            return False

    def is_search_tree(self):
        def check(tree,min,max):
            if min:
                above = min <= tree.value
            else:
                above = True
            if max:
                below = tree.value <= max
            else:
                below = True
            return above and below
        def loop(tree,min,max):
            if tree is None:
                return True
            elif check(tree,min,max):
                return loop(tree.left, min, tree.value) and loop(tree.right,tree.value,max)
            else:
                return False
        return loop(self,None,None)
            

sample1 = BinaryTreeNode(('a',('b', ('d',None,('f',None,('g',None, None))), ('e', None, None) ), ('c', None, None)))
        #         a
        #        / \
        #       b   c
        #      / \  
        #     d   e
        #      \ 
        #       f
        #        \
        #         g


# 1.
sample1.get_max_depth()

# 2.
sample1.get_levels()

# 3.
# easy
list(map(lambda x: x[-1], sample1.get_levels()))

# theoretical less space using depth first search
# python isn't tail recursive, so this is larger on the stack
def rightMost(tree):
    acc = []
    def loop(tree, level, acc):
        n = len(acc)
        if n < level:
            acc.append(tree.value)
        if tree.right:
            loop(tree.right, level + 1, acc)
        if tree.left:
            loop(tree.left, level + 1, acc)
    loop(tree, 1, acc)
    return acc

rightMost(sample1)

# 4.
sample2 = BinaryTreeNode(list(chain(range(1,13),[None]*3)))
sample2.exists(4,6)
def complete_tree_height(tree):
    height = 0
    node = tree
    while node:
        height += 1
        node = node.left
    return height
def complete_tree_count(tree):
    height = complete_tree_height(tree)
    left = 1
    right = 2 ** (height - 1)
    while left < right:
        mid = ceil((left + right) / 2)
        if tree.exists(height,mid):
            left = mid
        else:
            right = mid - 1
    return (2 ** (height - 1) - 1) + left
complete_tree_count(sample2)



# 5.
sample3 = BinaryTreeNode([8,6,22,9,None,19,25])
sample3.is_search_tree()
sample4 = BinaryTreeNode([12,8,18,5,10,14,25])
sample4.is_search_tree()


# attempt at continuation passing      
# doesn't matter. python doesn't optimize tail calls
def node_count(tree):
    def loop(tree,cont):
        if tree.left is None and tree.right is None:
            return cont(1)
        elif tree.left is None and tree.right is not None:
            return loop(tree.right, lambda size: cont(size + 1))
        elif tree.left is not None and tree.right is None:
            return loop(tree.left, lambda size: cont(size + 1))
        else:
            return loop(tree.left, lambda leftsize: loop(tree.right, lambda rightsize: cont(1+leftsize + rightsize)))
    return loop(tree, lambda x: x)