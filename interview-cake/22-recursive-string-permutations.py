
def flatten_list(top_list):
    return [ item for list in top_list for item in list]

def distribute(a,bs):
    n = len(bs)
    output = [ (bs[0:x] + a + bs[x:n]) for x in range(n+1)]
    return output

def loop(input_string, permutations ):
    if input_string == '':
        return permutations
    else:
        distributions = flatten_list([ distribute(input_string[0], x) for x in permutations ])
        return loop(input_string[1:], distributions)

def permute_string(input):
    return loop(input,[''])

distribute('a','bcdef')
loop('hello')
permute_string("ab")
permute_string("abc")
permute_string("abcd")




