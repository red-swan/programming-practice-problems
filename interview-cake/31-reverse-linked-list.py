class LinkedListNode(object):

    def __init__(self, value):
        self.value = value
        self.next  = None



def reverse(first_node):
    if first_node is None or first_node.next is None:
        return first_node

    current_node = first_node
    previous_node = None
    
    while current_node is not None:
        next_node = current_node.next
        current_node.next = previous_node
        previous_node = current_node
        current_node = next_node
        

    return previous_node




a = LinkedListNode("A")
b = LinkedListNode("B")
c = LinkedListNode("C")

a.next = b
b.next = c

last = reverse(a)