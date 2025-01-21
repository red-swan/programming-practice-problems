




sample1 = [1, 7, 3, 4]


def get_products_of_all_ints_except_at_index(num_list):

    products_before = [1] * len(num_list)
    for i in range(1,len(num_list)):
        products_before[i] = num_list[i-1] * products_before[i-1]
    

    output_products = [1] * len(num_list)
    acc = 1
    for i in range(1,len(num_list)+1):
        output_products[-i] = acc * products_before[-i]
        acc *= num_list[-i]
        

    return output_products

get_products_of_all_ints_except_at_index(sample1)





