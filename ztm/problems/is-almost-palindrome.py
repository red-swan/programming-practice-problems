#string
#subproblem 

# Given a string, determine if it is almost a palindrome. A string is 
# almost a palindrome if it becomes a palindrome by removing 1 letter.
# Consider only alphanumeric characters and ignore case sensitivity.

def _isPalindrome(s,start,end):
    while start < end:
        if s[start] != s[end]:
            return False
        start += 1
        end -= 1
    return True
    
def isPalindrome(s):
    return _isPalindrome(s,0,0)

# almost a palindrome
def isAlmostPalindrome(s):
    start = 0
    end = len(s) - 1
    while start < end:
        if s[start] == s[end]:
            start += 1
            end -= 1
        elif start == end - 1:
                return True
        else:
            return (_isPalindrome(s,start+1,end) or _isPalindrome(s,start,end-1))


    return True

sample1 = 'raceacar'   # True
sample2 = 'abccdba'    # True
sample3 = 'abcdefdba'  # False
sample4 = ""           # True
sample5 = "a"          # True
sample6 = "ab"         # True
all_samples = [sample1,sample2,sample3,sample4,sample5,sample6]

isAlmostPalindrome("raceacar")

isPalindrome('racecar')
isPalindrome('raceecar')

list(map(isAlmostPalindrome, all_samples))