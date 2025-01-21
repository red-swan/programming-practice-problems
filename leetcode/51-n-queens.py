
# 64 ms - 75%
# 14.7MB - 54%

class Solution:
    def solveNQueens(self, n: int) -> list[list[str]]:
        def toStrings(queens):
            output = []
            for _,c in sorted(queens):
                output.append('.' * c + 'Q' + '.' * (n-c-1))
            return output

        def canPlace(r,c):
            return not cols[c] and not rows[r] and not forwardDiags[r+c] and not backDiags[n-1 + c-r]
        def backtrack(r):
            if r == n:
                output.append(toStrings(queens))
            else:
                for c in range(n):
                    if canPlace(r,c):
                        queens.add((r,c))
                        cols[c] = True
                        rows[r] = True
                        forwardDiags[r+c] = True
                        backDiags[n-1 + c-r] = True
                        
                        backtrack(r+1)

                        queens.remove((r,c))
                        cols[c] = False
                        rows[r] = False
                        forwardDiags[r+c] = False
                        backDiags[n-1 + c-r] = False
                        
                    

        cols = [False] * n
        rows = [False] * n
        forwardDiags = [False] * (2 * n - 1)
        backDiags = [False] * (2 * n - 1) 
        queens = set()
        output = []
        backtrack(0)
        return output
        


answer = Solution()

answer.solveNQueens(5)
answer.solveNQueens(4)
answer.solveNQueens(8)




