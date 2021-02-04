class Solution:
    # bfs
    def countSteps(self, grid: list[list[int]]) -> int:
        if not grid:
            return grid
        elif not grid[0]:
            return grid
        else:
            m = len(grid)
            n = len(grid[0])
        queue = [(r,c) for r in range(m) for c in range(n) if grid[r][c] == 0]
        i = 1
        while queue:
            while queue:
                next_queue = []
                while queue:
                    r,c = queue.pop()
                    for r,c in [(r-1,c),(r,c+1),(r+1,c),(r,c-1)]:
                        if 0 <= r < m and 0<= c <n and grid[r][c] is empty:
                            grid[r][c] = i
                            next_queue.append((r,c))
            if next_queue:
                queue = next_queue
                i += 1
        return grid
    
    def _dfs_loop(self,grid,r,c,i):
        if r < 0 or len(grid) <= r or c < 0 or len(grid[0]) <= c or grid[r][c] < i:
            return
        grid[r][c] = i
        for r,c in [(r-1,c), (r,c+1), (r+1,c), (r,c-1)]:
            self._dfs_loop(grid,r,c, i + 1)
        
    # dfs
    def countStepsDfs(self,grid):
        if not grid or not grid[0]:
            return grid
        for r in range(len(grid)):
            for c in range(len(grid[0])):
                if grid[r][c] == 0:
                    self._dfs_loop(grid,r,c,0)
        
        return grid


# testing ======================================================================
def grid_as_string(grid):
    def line_to_str(l):
        return ''.join(map(str,l))
    return '\n'.join(map(line_to_str, grid)).replace(str(empty),"*").replace('-1','█')

empty = 2147483647

s1 = [
    [empty,    -1,     0, empty],
    [empty, empty, empty,    -1],
    [empty,    -1, empty,    -1],
    [    0,    -1, empty, empty]
]
s2 = [
    [empty,    -1,     0, empty],
    [   -1, empty, empty,    -1],
    [empty,    -1, empty,    -1],
    [    0,    -1, empty, empty]
]


Solution().countStepsDfs(s1)
print(grid_as_string(s1))
Solution().countStepsDfs(s2)
print(grid_as_string(s2))
