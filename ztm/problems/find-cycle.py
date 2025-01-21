class LinkedListNode(object):
    def __init__(self, value):
        self.value = value
        self.next  = None
    def __repr__(self):
        if self.next is None:
            next_string = "None"
        else:
            next_string = str(self.next.value)
        return "%s -> %s" % (str(self.value), next_string)

class LinkedList(object):
    def __init__(self,values):
        self.head = LinkedListNode(values[0])
        current_node = self.head
        for value in values[1:]:
            new_node = LinkedListNode(value)
            current_node.next = new_node
            current_node = new_node
    def __getitem__(self,i):
        if type(i) is not int:
            raise TypeError("linked list indices must be integers")
        j = 0
        current_node = self.head
        while j != i:
            if current_node is None:
                raise IndexError("linked list index out of range")
            else:
                current_node = current_node.next
            j += 1
        return current_node
    def _found_cycle_index_(self):
        tortoise = self.head
        hare = self.head
        i = 0
        while hare is not None and hare.next is not None:
            hare = hare.next.next
            tortoise = tortoise.next
            i += 1
            if hare is tortoise: 
                return i
        return None
    def has_cycle(self):
        if self._found_cycle_index_ is None:
            return False
        else: 
            return True
    def find_cycle_start_index(self):
        n = self._found_cycle_index_()
        if n is None:
            return None
        else:
            start_node = self.head
            lead_node = self[n]
            i = 0
            while start_node is not lead_node:
                start_node = start_node.next
                lead_node = lead_node.next
                i += 1
        return i
    def find_first_in_cycle(self):
        return self[self.find_cycle_start_index()]


sample1 = LinkedList([1,2,3,4,5,6,7,8])
sample1[7].next = sample1[2]

sample1.has_cycle()
sample1._found_cycle_index_()
sample1.find_cycle_start_index()
sample1.find_first_in_cycle()



