class DoubleLinkedListNode(object):
    def __init__(self, value):
        self.value = value
        self.previous = None
        self.next  = None
        self.child = None
    def __str__(self):
        if self.previous is None:
            prevStr = " "
        else:
            prevStr = str(self.previous.value)
        if self.next is None:
            nextStr = " "
        else:
            nextStr = str(self.next.value)
        if self.child is None:
            childStr = "  "
        else:
            childStr = str(self.child.head)
        return prevStr + str(self.value) + nextStr + "\n" +  childStr
    def __repr__(self):
        return str(self)

class DoubleLinkedList(object):
    def __init__(self,arr):
        previous_node = None
        first_node = DoubleLinkedListNode(arr[0])
        self.head = first_node
        current_node = first_node

        for item in arr[1:]:
            new_node = DoubleLinkedListNode(item)
            current_node.next = new_node
            current_node.previous = previous_node

            previous_node = current_node
            current_node = new_node
    def __repr__(self):
        output = []
        current_node = self.head
        while current_node is not None:
            output.append(str(current_node.value) + " <-> ")
            current_node = current_node.next
        output.append("None")
        return ''.join(output)
    def __len__(self):
        i = 0
        current_node = self.head
        while current_node is not None:
            current_node = current_node.next
            i += 1
        return i
    def __getitem__(self,i):
        j = 0
        current_node = self.head
        while j != i:
            current_node = current_node.next
            j += 1
        return current_node

    def flatten(self):
        previous_node = None
        current_node = self.head
        next_nodes = []
        while current_node is not None or next_nodes:
            if current_node is None:
                previous_node.next = next_nodes.pop()
                current_node = previous_node.next
            elif current_node.child is None:
                previous_node = current_node
                current_node = current_node.next
            else:
                next_nodes.append(current_node.next)
                current_node.next = current_node.child.head
                current_node.child = None
                previous_node = current_node
                current_node = current_node.next

    def insert_child(self,dll, i):
        insertion_node = self[i] 
        insertion_node.child = dll



sample1 = DoubleLinkedList(['a','b','c','d','e','f','g','h','i','j','k','l','m'])
sample2 = DoubleLinkedList(['n','o','p','q','r','s'])
sample3 = DoubleLinkedList(['t','u','v'])
sample4 = DoubleLinkedList(['w','x','y','z'])
sample1.insert_child(sample2, 5)
sample2.insert_child(sample3,0)
sample3.insert_child(sample4,2)

sample1.head
sample1[5].child
sample1[5]
sample1.flatten()
sample1
