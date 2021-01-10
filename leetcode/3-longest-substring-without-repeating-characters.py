#string
#sliding-window

# Given a string s, find the length of the longest substring 
# without repeating characters.

from collections import deque


# this method uses memory for convenience
# the set is to quickly look up what's in our
# deque and the deque is for easily maintaining
# the substring
# Space: O(n) ??
# Time: O(n)
def lengthOfLongestSubstring(s):
    seen_deque = deque()
    seen_set = set()
    longest_seen = 0
    for c in s:
        while c in seen_set:
            seen_set.remove(seen_deque.popleft())
        seen_set.add(c)
        seen_deque.append(c)
        longest_seen = max(longest_seen, len(seen_set))
    return longest_seen

# instead let's use a map
def lengthOfLongestSubstring(s):
    char_positions = dict()
    sub_start_index = 0
    max_sub_length = 0
    for sub_end_index, char in enumerate(s):
        if char in char_positions:
            index_after_char = char_positions[char] + 1
            sub_start_index = max(index_after_char, sub_start_index)

        max_sub_length = max(max_sub_length, sub_end_index - sub_start_index + 1)
        char_positions[char] = sub_end_index
        
    return max_sub_length



sample1 = "abcabcbb"   # 3
sample2 = "bbbbb"      # 1
sample3 = "pwwkew"     # 3
sample4 = ""           # 0
sample5 = "dvdf"       # 3
sample6 = "abba"       # 2
all_samples = [sample1, sample2, sample3, sample4, sample5, sample6]

list(map(lengthOfLongestSubstring,all_samples))
lengthOfLongestSubstring(sample6)














