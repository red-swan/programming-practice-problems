def reverse_in_place(char_list):
    n = len(char_list)
    for i in range(n // 2):
        char_list[i], char_list[-i-1] = char_list[-i-1], char_list[i]


sample = list("hello")

reverse_in_place(sample)
sample
