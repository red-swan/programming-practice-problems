# Reverse a linked list from position m to n. Do it in one-pass.

# 48 ms   -  7.53%
# 14.5 MB - 45.03%

class ListNode:
    def __init__(self, val=0, next=None):
        if type(val) is list and val:
            self._from_list(val)
        else:
            self.val = val
            self.next = next
    def _from_list(self,arr):
        self.val = arr[0]
        self.next = None
        current_node = self
        for item in arr[1:]:
            new_node = ListNode(item)
            current_node.next = new_node
            current_node = new_node
        return self
    def __repr__(self):
        output = []
        current_node = self
        while current_node is not None:
            output.append(str(current_node.val) + " -> ")
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

class Solution:
    def reverseBetween(self, head: ListNode, m: int, n: int) -> ListNode:
        if head is None or head.next is None or m == n:
            return head

        left_bound = None
        right_bound = None
        left_node = None
        current_node = head
        i = 0
        m = m - 1
        n = n - 1

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
            head = right_most_node
        else:
            left_bound.next = right_most_node
        return head


answer = Solution()
s1 = ListNode([1,2,3,4,5])
s1.val
s1.next
s1
s2 = ListNode([3,5])
answer.reverseBetween(s2,m = 1,n = 1)