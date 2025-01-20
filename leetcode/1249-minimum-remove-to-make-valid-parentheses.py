# Given a string s of '(' , ')' and lowercase English characters. 

# Your task is to remove the minimum number of parentheses 
# ( '(' or ')', in any positions ) so that the resulting 
# parentheses string is valid and return any valid string.

# Formally, a parentheses string is valid if and only if:
#     It is the empty string, contains only lowercase characters, or
#     It can be written as AB (A concatenated with B), where A and B are valid strings, or
#     It can be written as (A), where A is a valid string.

# 100ms  - 80.25%
# 15.9MB - 81.85%

class Solution:
    def minRemoveToMakeValid(self, s: str) -> str:
        s = list(s)
        open_stack = []
        for i in range(len(s)):
            if s[i] == '(':
                open_stack.append(i)
            elif s[i] == ')':
                if open_stack:
                    open_stack.pop()
                else:
                    s[i] = ''
        for i in open_stack:
            s[i] = ''
        return ''.join(s)

sample1 = "a)bc(d)" # 1
sample2 = "(a))(b(cde" # 3
sample3 = "(a)(b)((c)" # 1
sample4 = ")(a()))(bcd" # 3
sample5 = "abcd" # 0
sample6 = "(a)(b)cd" # 0
sample7 = "(ab(c)d" # 1
sample8 = "))((" # 4

all_samples = [sample1,sample2,sample3,sample4,sample5,sample6,sample7,sample8]
[Solution().minRemoveToMakeValid(x) for x in all_samples]

