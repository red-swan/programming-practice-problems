import functools as ft

def find_unique_delivery_id(delivery_ids):

    return ft.reduce(lambda x,y: x ^ y, delivery_ids )


  







