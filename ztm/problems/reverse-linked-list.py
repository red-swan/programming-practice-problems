# linked-list
# Given a linked list, return it in reverse

class LinkedListNode(object):
    def __init__(self, value):
        self.value = value
        self.next  = None
    def __repr__(self):
        output = []
        current_node = self
        while current_node is not None:
            output.append(str(current_node.value) + " -> ")
            current_node = current_node.next
        output.append("None")
        return ''.join(output)
    def __len__(self):
        i = 0
        current_node = self
        while current_node is not None:
            current_node = current_node.next
            i += 1
        return i

def make_linked_list(arr):
    first_node = LinkedListNode(arr[0])
    current_node = first_node

    for item in arr[1:]:
        new_node = LinkedListNode(item)
        current_node.next = new_node
        current_node = new_node
    return first_node

def reverse1(first_node):
    if first_node is None or first_node.next is None:
        return first_node

    left_node = None
    current_node = first_node
    
    while current_node is not None:
        right_node = current_node.next
        current_node.next = left_node
        
        left_node = current_node
        current_node = right_node

    return left_node

def reverse2(first_node,m,n):
    if first_node is None or first_node.next is None:
        return first_node

    left_bound = None
    right_bound = None
    left_node = None
    current_node = first_node
    i = 0

    while current_node is not None and i <= n+1:
        right_node = current_node.next
        if i < m - 1 or n + 1 < i:
            pass
        elif i == m - 1:
            left_bound = current_node
        elif i == m:
            left_most_node = current_node
        elif m < i < n:
            current_node.next = left_node
        elif n == i:
            current_node.next = left_node
            right_most_node = current_node
        elif n + 1 == i:
            right_bound = current_node
        else:
            pass
        left_node = current_node
        current_node = right_node
        i += 1

    left_most_node.next = right_bound
    if m == 0:
        first_node = right_most_node
    else:
        left_bound.next = right_most_node
    return first_node

sample_list = make_linked_list(['a','b','c','d','e','f','g','h','i','j','k','l','m'])
sample_list

len(sample_list)

last = reverse2(sample_list, 3, 5) # flip d through f
last = reverse2(sample_list, 0, 5) # flip a through f
last = reverse2(sample_list, 5, 12) # flip f through m
last
