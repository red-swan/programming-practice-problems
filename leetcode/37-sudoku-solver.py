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

# 276 ms - 56.52%
# 14.4 MB - 74.38%
class Solution:
    def __init__(self):
        self.nums = list(map(str,range(1,10)))
    def _getBoxId(self,row,col):
        return 3*(row // 3) + (col // 3 )
    def _isValid(self,box:set[str],row:set[str],col:set[str],num):
        return num not in box and num not in row and num not in col
    def solvBacktrack(self,board,boxes:list[set[str]],rows:list[set[str]],cols:list[set[str]],r,c):
        if r == len(board) or c == len(board[0]):
            return True
        ### ### ###
        if board[r][c] == '.':
            for num in self.nums:
                board[r][c] = num
                box = boxes[self._getBoxId(r,c)]
                row = rows[r]
                col = cols[c]

                if self._isValid(box,row,col,num):
                    box.add(num)
                    row.add(num)
                    col.add(num)

                    if c == len(board[0]) - 1:
                        if self.solvBacktrack(board,boxes, rows, cols, r + 1, 0):
                            return True
                    else:
                        if self.solvBacktrack(board, boxes, rows, cols,r, c + 1):
                            return True
                    box.remove(num)
                    row.remove(num)
                    col.remove(num)
                
                board[r][c] = '.'

        else:
            if c == len(board[0]) - 1:
                if self.solvBacktrack(board,boxes, rows, cols, r + 1, 0):
                    return True
            else:
                if self.solvBacktrack(board, boxes, rows, cols,r, c + 1):
                    return True
        return False


    def solveSudoku(self, board: list[list[str]]) -> None:
        """
        Do not return anything, modify board in-place instead.
        """
        n = len(board)
        boxes = [set() for _ in range(n)]
        rows  = [set() for _ in range(n)]
        cols  = [set() for _ in range(n)]

        for r in range(n):
            for c in range(n):
                if board[r][c] != '.':
                    boxId = self._getBoxId(r,c)
                    num = board[r][c]
                    boxes[boxId].add(num)
                    rows[r].add(num)
                    cols[c].add(num)

        self.solvBacktrack(board,boxes,rows,cols,0,0)

        
        

s1 = [["5","3",".",".","7",".",".",".","."],["6",".",".","1","9","5",".",".","."],[".","9","8",".",".",".",".","6","."],["8",".",".",".","6",".",".",".","3"],["4",".",".","8",".","3",".",".","1"],["7",".",".",".","2",".",".",".","6"],[".","6",".",".",".",".","2","8","."],[".",".",".","4","1","9",".",".","5"],[".",".",".",".","8",".",".","7","9"]]   
s2 = [[".",".","9","7","4","8",".",".","."],["7",".",".",".",".",".",".",".","."],[".","2",".","1",".","9",".",".","."],[".",".","7",".",".",".","2","4","."],[".","6","4",".","1",".","5","9","."],[".","9","8",".",".",".","3",".","."],[".",".",".","8",".","3",".","2","."],[".",".",".",".",".",".",".",".","6"],[".",".",".","2","7","5","9",".","."]]
s2Correct = [["5","1","9","7","4","8","6","3","2"],["7","8","3","6","5","2","4","1","9"],["4","2","6","1","3","9","8","7","5"],["3","5","7","9","8","6","2","4","1"],["2","6","4","3","1","7","5","9","8"],["1","9","8","5","2","4","3","6","7"],["9","7","5","8","6","3","1","2","4"],["8","3","2","4","9","1","7","5","6"],["6","4","1","2","7","5","9","8","3"]]
board = s2

answer = Solution()
answer.solveSudoku(board)
print_board(board)

def f(a,b,c,d):
    return a+b+c+d

f(1,*(2,3),4)

asd = dict()
asd = [dict() for _ in range(3)]

asd[0]["hello"] = "there"
asd[1]