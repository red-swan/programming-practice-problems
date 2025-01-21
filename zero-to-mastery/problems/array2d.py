


class Array2D():
    def __init__(self,vals,m,n):
        self.vals = [[vals[r*n+c] for c in range(n)] for r in range(m)]
        self.length1 = m
        self.length2 = n
    def _is_valid_coord(self,r,c):
        return (0 <= r < self.length1 and 0 <= c < self.length2)
    def _gen_neighbors(self,r,c):
        # up, right, down, left
        return (coord for coord in [(r-1,c),(r,c+1),(r+1,c),(r,c-1)] if self._is_valid_coord(*coord)) 
    def _first_valid_neighbor(self,r,c, exclude = set()):
        gen = (neighbor for neighbor in self._gen_neighbors(r,c) if neighbor not in exclude)
        return next(gen, None)
    def dfs(self):
        output = [None] * (self.length1 * self.length2)
        i = 0
        seen = set()
        next_coord = (0,0)
        while next_coord is not None:
            seen.add(next_coord)
            r,c = next_coord
            output[i] = self.vals[r][c]
            next_coord = self._first_valid_neighbor(*next_coord, exclude = seen)
            i += 1
        return output


test = Array2D(range(1,7), 2,3)
test.vals
test.dfs()

sample1 = Array2D(range(1,21),4,5)
sample1.dfs()






