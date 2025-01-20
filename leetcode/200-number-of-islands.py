# Given an m x n 2d grid map of '1's (land) and '0's (water), return the number of islands.

# An island is surrounded by water and is formed by connecting adjacent lands horizontally 
# or vertically. You may assume all four edges of the grid are all surrounded by water.

# 192 ms - 13.27%
# 21.2MB - 5.53%

class Solution:
    def _is_valid_coord(self,r,c,m,n):
        return (0 <= r < m and 0 <= c < n)
    def _gen_neighbors(self,r,c,m,n, exclude = set(), include = set() ):
        # up, right, down, left
        return (coord for coord in [(r-1,c),(r,c+1),(r+1,c),(r,c-1)] if self._is_valid_coord(*coord,m,n) and coord not in exclude and coord in include) 
    def numIslands(self, grid: list[list[str]]) -> int:
        m = len(grid)
        n = len(grid[0])

        unchecked = set(((r,c) for r in range(m) for c in range(n)))
        count = 0
        while unchecked:
            r,c = coord = unchecked.pop()
            if grid[r][c] == '1':
                count += 1
                to_check = set(self._gen_neighbors(r,c,m,n,include = unchecked))
                # search for island
                while to_check:
                    r,c = neighbor_coord = to_check.pop()
                    unchecked.remove(neighbor_coord)
                    if grid[r][c] == '1':
                        for coord in self._gen_neighbors(r,c,m,n, exclude = to_check, include = unchecked):
                            to_check.add(coord)
        return count

# 152 ms - 39.38%
# 15.4 MB - 54.83%

class Solution:
    def sinkIsland(self, grid, r,c,m,n):
        seen = set()
        queue = set([(r,c)])
        while queue:
            r,c = queue.pop()
            seen.add((r,c))
            grid[r][c] = '0'
            for r,c in [(r-1,c),(r,c+1),(r+1,c),(r,c-1)]:
                if 0 <= r < m and 0<= c <n and (r,c) not in seen:
                    seen.add((r,c)) 
                    if grid[r][c] == '1':
                        queue.add((r,c))

    def numIslands(self,grid):
        count = 0
        m = len(grid)
        n = len(grid[0])
        for r in range(m):
            for c in range(n):
                if grid[r][c] == '1':
                    count += 1
                    self.sinkIsland(grid, r,c,m,n)

        return count


# 132 ms  - 86.16%
# 15.2 MB - 95.31%
class Solution:
    def sinkIsland(self, grid, r,c,m,n):
        queue = [(r,c)]
        while queue:
            r,c = queue.pop()
            for r,c in [(r-1,c),(r,c+1),(r+1,c),(r,c-1)]:
                if 0 <= r < m and 0<= c <n and grid[r][c] == '1':
                    grid[r][c] = '0'
                    queue.append((r,c))

    def numIslands(self,grid):
        count = 0
        m = len(grid)
        n = len(grid[0])
        for r in range(m):
            for c in range(n):
                if grid[r][c] == '1':
                    count += 1
                    grid[r][c] = '0'
                    self.sinkIsland(grid, r,c,m,n)

        return count



s1 = [
  ["1","1","1","1","0"],
  ["1","1","0","1","0"],
  ["1","1","0","0","0"],
  ["0","0","0","0","0"]
]

s2 = [
  ["1","1","0","0","0"],
  ["1","1","0","0","0"],
  ["0","0","1","0","0"],
  ["0","0","0","1","1"]
]

s3 = [
    ['1','1','1','1','0'],
    ['1','1','0','1','0'],
    ['1','1','0','0','1'],
    ['0','0','0','1','1']
]

[Solution().numIslands(grid) for grid in [s1,s2,s3]]

