

class LinkedListNode(object):

    def __init__(self, value):
        self.value = value
        self.next  = None

a = LinkedListNode('A')
b = LinkedListNode('B')
c = LinkedListNode('C')

a.next = b
b.next = c



def delete_node_unsafe(node_to_delete):
    next_node = node_to_delete.next
    if next_node:
        node_to_delete.next = next_node.next
        node_to_delete.value = next_node.value
    else:
        raise Exception("Cannot delete last value in place")


delete_node_unsafe(b)

a.next.value

















    