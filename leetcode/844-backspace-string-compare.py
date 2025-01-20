#string
#generator

# Given two strings S and T, return if they are equal when 
# both are typed into empty text editors. # means a 
# backspace character.

# Note that after backspacing an empty text, 
# the text will continue empty.

def backspaceCompare(S, T):
    si = len(S) - 1
    ti = len(T) - 1
    s_hashes = 0
    t_hashes = 0
    while True:
        if si < 0: sChar = ""
        else: sChar = S[si]
        if ti < 0: tChar = ""
        else: tChar = T[ti]

        if sChar == "" and sChar == tChar : return True
        s_is_backspace = sChar == "#"
        t_is_backspace = tChar == "#"

        if s_is_backspace or t_is_backspace:
            if s_is_backspace:
                s_hashes += 1
                si -= 1
            if t_is_backspace:
                t_hashes += 1
                ti -= 1
            continue

        if 0 < s_hashes or 0 < t_hashes:
            if 0 < s_hashes:
                si -= 1
                s_hashes -= 1
            if 0 < t_hashes:
                ti -= 1
                t_hashes -= 1
            continue

        if sChar != tChar:
            return False
        else:
            si -= 1
            ti -= 1

from itertools import zip_longest
def backspaceCompare(S, T):
    def F(S):
        skip = 0
        for x in reversed(S):
            if x == '#':
                skip += 1
            elif skip:
                skip -= 1
            else:
                yield x

    return all(x == y for x, y in zip_longest(F(S), F(T)))



s1,t1 = "ab#c", "ad#c"
s2,t2 = "ab##", "c#d#"
s3,t3 = "a##c", "#a#c"
s4,t4 = "a#c", "b"
s5,t5 = "a######", "a"
s6,t6 = "a######", "a#"
s7,t7 = "###", "#"
s8,t8 = "bxj##tw", "bxo#j##tw"
s9,t9 = "xywrrmp", "xywrrmu#p"

backspaceCompare(s1,t1) # True
backspaceCompare(s2,t2) # True
backspaceCompare(s3,t3) # True
backspaceCompare(s4,t4) # False
backspaceCompare(s5,t5) # False
backspaceCompare(s6,t6) # True
backspaceCompare(s7,t7) # True
backspaceCompare(s8,t8) # True
backspaceCompare(s9,t9) # True



