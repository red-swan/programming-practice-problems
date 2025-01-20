# Given a linked list, return the node where the cycle begins. 
# If there is no cycle, return null.

# There is a cycle in a linked list if there is some node in 
# the list that can be reached again by continuously following 
# the next pointer. Internally, pos is used to denote the index of 
# the node that tail's next pointer is connected to. Note that pos is 
# not passed as a parameter.

# Notice that you should not modify the linked list.

# 52 ms   - 56.90%
# 17.4 MB - 24.07%

# Definition for singly-linked list.
class ListNode:
    def __init__(self, x):
        if type(x) is tuple:
            self._from_tuple(x)
        else:
            self.val = x
            self.next = None
    def __getitem__(self,i):
        j = 0
        current_node = self
        while j < i:
            current_node = current_node.next
            j += 1
        return current_node
    def _from_tuple(self,t):
        head,pos = t
        self.val = head[0]
        self.next = None
        previous_node = self
        if 1 < len(head):
            for i in head[1:]:
                current_node = ListNode(i)
                previous_node.next = current_node
                previous_node = current_node
            if 0 <= pos:
                current_node.next = self[pos]
    def __repr__(self):
        if self.next is not None:
            next_string = self.next.val
        else:
            next_string = self.next
        return "{} -> {}".format(self.val, next_string)
    def __str__(self):
        return self.__repr__()
        


class Solution:
    def detectCycle(self, head : ListNode) -> ListNode:
        # run slow and fast pointers from start to find if cycle exists
        if head is None:
            return None
        slow_runner = head
        fast_runner = head
        if fast_runner.next is not None:
            fast_runner = fast_runner.next
        else:
            return None
        if fast_runner.next is not None:
            fast_runner = fast_runner.next
            slow_runner = slow_runner.next
        else:
            return None
        
        while fast_runner is not None and fast_runner is not slow_runner:
            if fast_runner.next is not None:
                fast_runner = fast_runner.next
            else:
                return None
            if fast_runner.next is not None:
                fast_runner = fast_runner.next
                slow_runner = slow_runner.next
            else:
                return None
            
        # run pointers from start and crossing point to find cycle start
        start = head
        while start is not slow_runner:
            start = start.next 
            slow_runner = slow_runner.next

        return start

answer = Solution()
sample1 = ListNode(([3,2,0,-4],1))
sample2 = ListNode(([1,2],0))
sample3 = ListNode(([1],-1))
answer.detectCycle(sample3)
solutions = [answer.detectCycle(x) for x in [sample1,sample2,sample3] ]
