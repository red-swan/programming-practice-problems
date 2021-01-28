#Given a non-empty string s, you may delete at most one character. Judge whether you can make it a palindrome. 

# 168 ms  - 55.99%
# 14.8 MB - 13.57%

def _isPalindrome(s,start,end):
    while start < end:
        if s[start] != s[end]:
            return False
        start += 1
        end -= 1
    return True
    
def isPalindrome(s):
    return _isPalindrome(s,0,0)

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