



openers = "{[("
closers = "}])"
closer_dict = dict( (close,open) for (close,open) in zip(closers,openers))
openers_set = set(openers)

def is_valid(code):
    stack = []

    for char in code:
        if char in openers_set:
            stack.append(char)
        elif char in closer_dict:
            if len(stack) == 0: return False
            top = stack.pop()
            if top != closer_dict[char]:
                return False
    
    if len(stack) == 0: return True 
    else: return False



sample1 = "{ [ ] ( ) }"
sample2 = "{ [ ( ] ) }"
sample3 = "{ [ }"

is_valid(sample1)
is_valid(sample2)
is_valid(sample3)



