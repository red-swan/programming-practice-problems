def fifo_check(take_out_order, dine_in_orders, served_orders):
    take_out_idx = dine_in_idx = 0
    for i in range(len(served_orders)):
        if take_out_idx + 1 <= len(take_out_order) and served_orders[i] == take_out_order[take_out_idx]:
            take_out_idx += 1
        elif dine_in_idx + 1 <= len(dine_in_orders) and served_orders[i] == dine_in_orders[dine_in_idx]:
            dine_in_idx += 1
        else:
            return False 
    
    if take_out_idx != len(take_out_order) & dine_in_idx != len(dine_in_orders):
        return False

    return True


take_out = [1, 3, 5]
dine_in =  [2, 4, 6]
served = [1, 2, 4, 6, 5, 3]
fifo_check(take_out,dine_in,served)

fifo_check([17, 8, 24],[12, 19, 2],[17, 8, 12, 19, 24, 2])

