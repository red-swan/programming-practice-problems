# On an NxN chessboard, a knight starts at the r-th row and c-th column 
# and attempts to make exactly K moves. The rows and columns are 0 
# indexed, so the top-left square is (0, 0), and the bottom-right 
# square is (N-1, N-1).

# A chess knight has 8 possible moves it can make, as illustrated below. 
# Each move is two squares in a cardinal direction, then one square in 
# an orthogonal direction.

# 220 ms  - 59.29%
# 14.5 MB - 59.70%

class Solution:
    def getNewPositions(self,n,r,c):
        return [(r,c) for r,c in [(r-2,c+1),(r-2,c-1),(r-1,c+2),(r-1,c-2),(r+2,c+1),(r+2,c-1),(r+1,c-2),(r+1,c+2)] if 0 <= c < n and 0 <= r < n]
    def createEmptyBoard(self,n):
        return [[0 for _ in range(n)] for _ in range(n)]
    def createNewProbBoard(self,n,r,c):
        board = self.createEmptyBoard(n)
        board[r][c] = 1
        return board
    def knightProbability(self, N: int, K: int, r: int, c: int) -> float:
        previous_board_probs = self.createNewProbBoard(N,r,c)
        current_board_probs = self.createEmptyBoard(N)
        previous_positions = set([(r,c)])
        current_positions = set()
        
        for _ in range(K):
            for prev_r, prev_c in previous_positions:
                neighbors = self.getNewPositions(N,prev_r, prev_c)
                for curr_r,curr_c in neighbors:
                    current_positions.add((curr_r,curr_c))
                    current_board_probs[curr_r][curr_c] += previous_board_probs[prev_r][prev_c] / 8

            
            previous_board_probs = current_board_probs
            current_board_probs = self.createEmptyBoard(N)
            previous_positions = current_positions
            current_positions = set()

        return sum(map(sum,previous_board_probs))



# Solution().knightProbability(3,0,0,0)
# Solution().knightProbability(3,1,0,0)
# Solution().knightProbability(3,2,0,0)
Solution().knightProbability(3,3,0,0)
