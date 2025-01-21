# Given a string s containing just the characters '(', ')', '{', '}', 
# '[' and ']', determine if the input string is valid.
# An input string is valid if:
#     Open brackets must be closed by the same type of brackets.
#     Open brackets must be closed in the correct order.

# 32 ms - 61.04%
# 14.3 MB - 66.38%

class Solution:
    def isValid(self,s: str) -> bool:
        stack = []
        for c in s:
            if ord(c) == 123 or ord(c) == 40 or ord(c) == 91:
                # open brackets
                stack.append(ord(c))
            elif ord(c) == 125 or ord(c) == 41 or ord(c) == 93:
                # close brackets
                if stack:
                    check = (stack.pop(),ord(c))
                else:
                    return False
                if check == (123,125) or check == (40,41) or check == (91,93):
                    pass
                else:
                    print(check)
                    return False
            else:
                pass
        if stack:
            return False
        else:
            return True

[Solution().isValid(x) for x in ["[]","[]]","[[]"]]

















