# You are given an m x n grid where each cell can have one of three values:

#     0 representing an empty cell,
#     1 representing a fresh orange, or
#     2 representing a rotten orange.

# Every minute, any fresh orange that is 4-directionally adjacent to a rotten orange becomes rotten.

# Return the minimum number of minutes that must elapse until no cell has a fresh orange. If this is impossible, return -1.

# 52ms - 70.34%
# 14.1MB - 97.25%

class Solution:

    def orangesRotting(self, grid: list[list[int]]) -> int:
        if not grid:
            return 0
        elif not grid[0]:
            return 0
        else:
            m = len(grid)
            n = len(grid[0])
        to_rot = [(r,c) for r in range(m) for c in range(n) if grid[r][c] == 2]
        i = 0

        while to_rot:
            while to_rot:
                next_rot = []
                while to_rot:
                    r,c = to_rot.pop()
                    for r,c in [(r-1,c),(r,c+1),(r+1,c),(r,c-1)]:
                        if 0 <= r < m and 0<= c <n and grid[r][c] == 1:
                            grid[r][c] = 0
                            next_rot.append((r,c))
            if next_rot:
                to_rot = next_rot
                i += 1

        for r in range(m):
            for c in range(n):
                if grid[r][c] == 1:
                    return -1
        return i



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