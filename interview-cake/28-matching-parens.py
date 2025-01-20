




def get_closing_paren(string, opening_paren_index):
    count = 0

    for idx in range(opening_paren_index,len(string)):
        char = string[idx]
        if char == '(':
            count += 1
        elif char == ')':
            count -= 1
        else:
            pass
        if count == 0:
            return idx

    raise ValueError("Input string has no matching closing parenthesis")




sample1 = "Sometimes (when I nest them (my parentheticals) too much (like this (and this))) they get confusing."


sample1[10]



get_closing_paren(sample1,10)