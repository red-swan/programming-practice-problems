import unittest
    
class Stack(object):

    def __init__(self):
        """Initialize an empty stack"""
        self.items = []

    def push(self, item):
        """Push a new item onto the stack"""
        self.items.append(item)

    def pushMany(self, items):
        for item in items:
            self.push(item)

    def pop(self):
        """Remove and return the last item"""
        # If the stack is empty, return None
        # (it would also be reasonable to throw an exception)
        if not self.items:
            None
        return self.items.pop()

    def peek(self):
        """Return the last item without removing it"""
        if not self.items:
            return None
        return self.items[-1]

class QueueTwoStacks(object):

    def __init__(self):
        self.stack1 = Stack()
        self.stack2 = Stack()

    def enqueue(self, item):
        self.stack1.push(item)

    def dequeue(self):
        if len(self.stack2.items) == 0:
            if len(self.stack1.items) == 0:
                raise Exception("Queue is empty")
            while len(self.stack1.items) > 0:
                self.stack2.push(self.stack1.pop())

        return self.stack2.pop()



class Test(unittest.TestCase):

    def test_basic_queue_operations(self):
        queue = QueueTwoStacks()

        queue.enqueue(1)
        queue.enqueue(2)
        queue.enqueue(3)

        actual = queue.dequeue()
        expected = 1
        self.assertEqual(actual, expected)

        actual = queue.dequeue()
        expected = 2
        self.assertEqual(actual, expected)

        queue.enqueue(4)

        actual = queue.dequeue()
        expected = 3
        self.assertEqual(actual, expected)

        actual = queue.dequeue()
        expected = 4
        self.assertEqual(actual, expected)

    def test_error_when_dequeue_from_new_queue(self):
        queue = QueueTwoStacks()

        with self.assertRaises(Exception):
            queue.dequeue()

    def test_error_when_dequeue_from_empty_queue(self):
        queue = QueueTwoStacks()

        queue.enqueue(1)
        queue.enqueue(2)

        actual = queue.dequeue()
        expected = 1
        self.assertEqual(actual, expected)

        actual = queue.dequeue()
        expected = 2
        self.assertEqual(actual, expected)

        with self.assertRaises(Exception):
            queue.dequeue()


unittest.main(verbosity=2)