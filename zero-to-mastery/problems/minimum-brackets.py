sample1 = "a)bc(d)" # 1
sample2 = "(a))(b(cde" # 3
sample3 = "(a)(b)((c)" # 1
sample4 = ")(a()))(bcd" # 3
sample5 = "abcd" # 0
sample6 = "(a)(b)cd" # 0
sample7 = "(ab(c)d" # 1
sample8 = "))((" # 4

# problem just wants number of brackets to remove
# so no stack necessary
def min_brackets(string):
    open_brackets = 0
    error_count = 0
    for char in string:
        if char == ")":
            if open_brackets == 0:
                error_count += 1 
            else:
               open_brackets -= 1
        elif char == "(":
            open_brackets += 1
    return error_count + open_brackets


all_samples = [sample1,sample2,sample3,sample4,sample5,sample6,sample7,sample8]

list(map(min_brackets, all_samples))