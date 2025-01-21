
def hasPalindrome(input_string):
    char_set = set()
    for char in input_string:
        if char in char_set:
            char_set.remove(char)
        else:
            char_set.add(char)
    if len(char_set) <= 1:
        return True
    else:
        return False


hasPalindrome("civic")
hasPalindrome("ivicc")
hasPalindrome("civil")
hasPalindrome("livci")


