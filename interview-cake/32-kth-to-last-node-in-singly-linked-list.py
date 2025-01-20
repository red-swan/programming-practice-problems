class LinkedListNode:

    def __init__(self, value):
        self.value = value
        self.next  = None


a = LinkedListNode("Angel Food")
b = LinkedListNode("Bundt")
c = LinkedListNode("Cheese")
d = LinkedListNode("Devil's Food")
e = LinkedListNode("Eccles")

a.next = b
b.next = c
c.next = d
d.next = e


def kth_to_last_node(k,first_node):
    if k <= 0:
        raise ValueError("k must be larger than zero")
     
    current_node = first_node
    kth_back_node = current_node
    i = 0
    while current_node is not None:
        if k < i+1: kth_back_node = kth_back_node.next

        current_node = current_node.next
        i += 1

    if i < k:
        raise ValueError("k is larger than the list length")
    else:
        return kth_back_node


# Returns the node with value "Devil's Food" (the 2nd to last node)
result = kth_to_last_node(6, a)
result.value