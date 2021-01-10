

def find_rotation_point(word_list):
    low_index = 0
    high_index = len(word_list) - 1
    found = False
    while not found:
        halfway_index = (high_index + low_index + 1) // 2
        if word_list[halfway_index] < word_list[halfway_index - 1] :
            found = True
        elif word_list[low_index] < word_list[halfway_index]:
                # [k ... |w| ... a ... j]
                low_index = halfway_index
        elif word_list[halfway_index] < word_list[low_index]:
                # [k ... a ... |h| ... j]
                high_index = halfway_index
        else:
                raise ValueError("Something went wrong")
    return halfway_index

words = [
    'ptolemaic',
    'retrograde',
    'supplant',
    'undulate',
    'xenoepist',
    'asymptote',  # <-- rotates here!
    'babka',
    'banoffee',
    'engender',
    'karpatka',
    'othellolagkage',
]

find_rotation_point(words)