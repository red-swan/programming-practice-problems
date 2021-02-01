# You are given an m x n grid where each cell can have one of three values:

#     0 representing an empty cell,
#     1 representing a fresh orange, or
#     2 representing a rotten orange.

# Every minute, any fresh orange that is 4-directionally adjacent to a rotten orange becomes rotten.

# Return the minimum number of minutes that must elapse until no cell has a fresh orange. If this is impossible, return -1.

# 52ms - 70.34%
# 14.3MB - 39.99%

class Solution:
    def orangesRotting(self, grid: list[list[int]]) -> int:
        if not grid or not grid[0]:
            return 0
        else:
            m = len(grid)
            n = len(grid[0])
        i = 0
        ones_count = 0
        queue = []
        
        for r in range(m):
            for c in range(n):
                v = grid[r][c]
                if v == 2:
                    queue.append((r,c))
                elif v == 1:
                    ones_count += 1
                else:
                    continue

        coords_to_step_through = len(queue)
        while queue:
            if coords_to_step_through == 0:
                coords_to_step_through = len(queue)
                i += 1
            while 0 < coords_to_step_through:
                r,c = queue.pop(0)
                coords_to_step_through -= 1
                for r1,c1 in [(r-1,c),(r,c+1),(r+1,c),(r,c-1)]:
                    if 0 <= r1 < m and 0<= c1 <n and grid[r1][c1] == 1:
                        grid[r1][c1] = 2
                        ones_count -= 1
                        queue.append((r1,c1))
        if ones_count == 0:
            return i
        else:
            return -1
      
s1 = [
    [2,1,1,0,0],
    [1,1,1,0,0],
    [0,1,1,1,1],
    [0,1,0,0,1]
] # 7

s2 = [
    [1,1,0,0,0],
    [2,1,0,0,0],
    [0,0,0,1,2],
    [0,1,0,0,1]
] # -1

s3 = [

] # 0

s4 = [
    [],
    []
] # 0

s5 = [
    [2,1,1],
    [0,1,1],
    [1,0,1]
] # -1

s6 = [
    [0,2]
] # 0

s7 = [
    [2,1,1],
    [1,1,0],
    [0,1,1]
] # 4

s8 = [
    [0]
]

[Solution().orangesRotting(grid) for grid in [s1,s2,s3,s4,s5,s6,s7,s8]]