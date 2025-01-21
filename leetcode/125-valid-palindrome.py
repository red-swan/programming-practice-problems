# Given a string, determine if it is a palindrome, 
# considering only alphanumeric characters and ignoring cases.

# 96 ms   -  6.44%
# 14.5 Mb - 81.92%
# see https://leetcode.xnerv.wang/valid-palindrome/

from string import ascii_lowercase
from string import digits
allowed = set(ascii_lowercase + digits)


def isPalindrome(s):

    i_left = 0
    i_right = len(s) - 1

    while i_left < i_right:
        left = s[i_left].lower()
        right = s[i_right].lower()
        left_in = left in allowed
        right_in = right in allowed
        if left_in and right_in:
            if left == right:
                i_left += 1
                i_right -= 1
            else:
                return False
        elif left_in:
            i_right -= 1
        elif right_in:
            i_left += 1
        else:
            i_left += 1
            i_right -= 1

    return True


s0 = ""
s1 = "A man, a plan, a canal: Panama"
s2 = "race a car"
s3 = 'abb'
s4 = 'racecar'
s5 = 'BbB'
all_samples = [s0,s1,s2,s3,s4,s5]
# isPalindrome(s1)
list(map(isPalindrome,all_samples))
