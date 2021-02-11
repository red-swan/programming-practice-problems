# Write a program to solve a Sudoku puzzle by filling the empty cells.

# A sudoku solution must satisfy all of the following rules:

#     Each of the digits 1-9 must occur exactly once in each row.
#     Each of the digits 1-9 must occur exactly once in each column.
#     Each of the digits 1-9 must occur exactly once in each of the 
#         9 3x3 sub-boxes of the grid.

# The '.' character indicates empty cells.

import itertools as it

def board_to_string(board):
    def addNth(line, new_char, N):
        return list(it.chain(*[line[i : i+N] + [new_char]  
                    if len(line[i : i+N]) == N  
                    else line[i : i+N]  
                    for i in range(0, len(line), N)])) 

    def addVerts(line):
        return addNth(line,"|",3)
    def addHorzs(line):
        return addNth(line,"------------",3)

    rows = list(map(lambda line: ''.join(addVerts(line)),board))
    columns = addHorzs(rows)

    return '\n'.join(columns)

def print_board(board):
    print(board_to_string(board))

# 1728 ms - 5.01%
# 14.6 MB - 27.26%
class Solution:
    def __init__(self):
        self.allOptions = set(map(str,range(1,10)))
        self.boxCoords = {(i,j): list(it.product(self.boxNums(i),self.boxNums(j))) for i in [0,3,6] for j in [0,3,6]}
    def boxNums(self,i):
        m = 3*(i // 3)
        return [m, m + 1, m + 2]

    def genBoxCoords(self,board,r,c):
        return self.boxCoords[(3*(r//3), 3*(c//3))]

    def genRowCoords(self,board,r,c):
        return ((r,i) for i in range(9))
    
    def genColCoords(self,board,r,c):
        return ((i,c) for i in range(9))

    def genRelatedCoords(self,board,r,c):
        return it.chain(self.genRowCoords(board,r,c), self.genColCoords(board,r,c), self.genBoxCoords(board,r,c))

    def genRelatedValues(self,board,r,c):
        return (board[r][c] for r,c in self.genRelatedCoords(board,r,c))

    def possible(self,board,row,col, num):
        return num not in self.genRelatedValues(board, row, col)

    def solveSudoku(self, board: list[list[str]]) -> None:
        """
        Do not return anything, modify board in-place instead.
        """
        
        for row in range(9):
            for col in range(9):
                if board[row][col] == '.':
                    for num in self.allOptions:
                        if self.possible(board,row,col, num):
                            board[row][col] = num
                            if self.solveSudoku(board):
                                return True
                            else:
                                board[row][col] = '.'
                    return False
        return True
        

s1 = [["5","3",".",".","7",".",".",".","."],["6",".",".","1","9","5",".",".","."],[".","9","8",".",".",".",".","6","."],["8",".",".",".","6",".",".",".","3"],["4",".",".","8",".","3",".",".","1"],["7",".",".",".","2",".",".",".","6"],[".","6",".",".",".",".","2","8","."],[".",".",".","4","1","9",".",".","5"],[".",".",".",".","8",".",".","7","9"]]   
s2 = [[".",".","9","7","4","8",".",".","."],["7",".",".",".",".",".",".",".","."],[".","2",".","1",".","9",".",".","."],[".",".","7",".",".",".","2","4","."],[".","6","4",".","1",".","5","9","."],[".","9","8",".",".",".","3",".","."],[".",".",".","8",".","3",".","2","."],[".",".",".",".",".",".",".",".","6"],[".",".",".","2","7","5","9",".","."]]
s2Correct = [["5","1","9","7","4","8","6","3","2"],["7","8","3","6","5","2","4","1","9"],["4","2","6","1","3","9","8","7","5"],["3","5","7","9","8","6","2","4","1"],["2","6","4","3","1","7","5","9","8"],["1","9","8","5","2","4","3","6","7"],["9","7","5","8","6","3","1","2","4"],["8","3","2","4","9","1","7","5","6"],["6","4","1","2","7","5","9","8","3"]]
board = s2

answer = Solution()
answer.solveSudoku(board)
print_board(board)

