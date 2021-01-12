def overlap(endpoints1, endpoints2):
    min1,max1 = endpoints1
    min2,max2 = endpoints2
    if max1 <= min2:
        return None
    return max(min1,min2), min(max1,max2)

def get_xs(rectangle):
    return rectangle['left_x'], rectangle['left_x'] + rectangle['width']
def get_ys(rectangle):
    return rectangle['bottom_y'], rectangle['bottom_y'] + rectangle['height']

# 0,1,2,4 corners inside
# check upper right and lower left to see if it's inside

def find_rectangular_overlap(rect1, rect2):
    
    x_overlap = overlap(get_xs(rect1), get_xs(rect2))
    y_overlap = overlap(get_ys(rect1), get_ys(rect2))

    if x_overlap and y_overlap:
        return {
            'left_x'   : x_overlap[0],
            'bottom_y' : y_overlap[0],            
            'width'    : x_overlap[1] - x_overlap[0],
            'height'   : y_overlap[1] - y_overlap[0]
        }
    else:
        return {
            'left_x'   : None,
            'bottom_y' : None,            
            'width'    : None,
            'height'   : None
        }

rect1 = {
    'left_x': 1,
    'bottom_y': 1,
    'width': 2,
    'height': 2,
}
rect2 = {
    'left_x': 3,
    'bottom_y': 3,
    'width': 2,
    'height': 2,
}

find_rectangular_overlap(rect1,rect2)