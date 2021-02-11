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


class Solution:
    def __init__(self):
        self.allOptions = set(map(str,range(1,10)))
        self.boxCoords = {(i,j): list(it.product(self.boxNums(i),self.boxNums(j))) for i in [0,3,6] for j in [0,3,6]}
    def boxNums(self,i):
        m = 3*(i // 3)
        return [m, m + 1, m + 2]
    def coordsToCheck(self,board,r,c):
        row = ((r,i) for i in range(9))
        col = ((i,c) for i in range(9))
        box = self.boxCoords[(3*(r//3), 3*(c//3))]
        return it.chain(row,col,box)

    def getCandidates(self,board,r,c):
        if board[r][c] == '.':
            found = { board[r][c] for r,c in self.coordsToCheck(board,r,c) if board[r][c] != '.'}
            candidates = self.allOptions.difference(found)
            return candidates
        else:
            return {}

    def getAllCandidates(self,board):
        return (((r,c),self.getCandidates(board,r,c)) for r,c in it.product(range(9),range(9)) if board[r][c] == '.')

    def findEasiestCoord(self,board):
        min = 10
        (a,b,cs) = None,None,None
        gen = self.getAllCandidates(board)
        for ((r,c),candidates) in gen:
            if len(candidates) == 0:
                return None
            else:
                if len(candidates) < min:
                    min = len(candidates)
                    a = r
                    b = c
                    cs = candidates
        return ((a,b),cs)

    def loop(self, board,r,c,v):
        board[r][c] = v
        ((rnext,cnext),candidates) = self.findEasiestCoord(board)
        if rnext is None:
            board[r][c] = '.'
        elif len(candidates) == 0:
            return
        else:
            for candidate in candidates:
                self.loop(board,rnext,cnext,candidate)        
        

    def solveSudoku(self, board: list[list[str]]) -> None:
        """
        Do not return anything, modify board in-place instead.
        """
        ((r,c),candidates) = self.findEasiestCoord(board)
        for candidate in candidates:
            return self.loop(board,r,c, candidate)
        

s1 = [["5","3",".",".","7",".",".",".","."],["6",".",".","1","9","5",".",".","."],[".","9","8",".",".",".",".","6","."],["8",".",".",".","6",".",".",".","3"],["4",".",".","8",".","3",".",".","1"],["7",".",".",".","2",".",".",".","6"],[".","6",".",".",".",".","2","8","."],[".",".",".","4","1","9",".",".","5"],[".",".",".",".","8",".",".","7","9"]]   
answer = Solution()
print_board(s1)
answer.findEasiestCoord(s1) # should give 5
answer.getCandidates(s1,4,4)

answer.solveSudoku(s1)
print_board(s1)


# def test(x,acc):
#     acc.append(x)
#     print(acc)
#     s = sum(acc)
#     if s == 7:
#         return
#     elif 7 < s:
#         acc.pop()
#     else:
#         #  sum(acc) < 7:
#         for i in [1000,100,1]:
#             test(i,acc)
                
# acc = []
# test(1,acc)
# acc

def f():
    for x in range(10):
        print(x)
        return x

f = (x for x in range(10))

for a in f:
    # a = next(f)
    print(a)
