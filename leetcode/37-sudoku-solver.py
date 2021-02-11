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

    def genBoxCoords(self,board,r,c):
        return self.boxCoords[(3*(r//3), 3*(c//3))]

    def genRowCoords(self,board,r,c):
        return ((r,i) for i in range(9))
    
    def genColCoords(self,board,r,c):
        return ((i,c) for i in range(9))

    def genRelatedCoords(self,board,r,c):
        return it.chain(self.genRowCoords(board,r,c), self.genColCoords(board,r,c), self.genBoxCoords(board,r,c))

    def genAllEmptyCoords(self,board):
        for r in range(9):
            for c in range(9):
                if board[r][c] == '.':
                    yield (r,c)

    def getCandidates(self,board,r,c):
        if board[r][c] == '.':
            found = { board[r][c] for r,c in self.genRelatedCoords(board,r,c) if board[r][c] != '.'}
            candidates = self.allOptions.difference(found)
            return candidates
        else:
            return {}

    def genAllCandidates(self,board):
        for r,c in self.genAllEmptyCoords(board):
            yield ((r,c),self.getCandidates(board,r,c))


    def findCoordWithLeastCandidates(self,board):
        smallest_candidate_count = 10
        output = None
        for ((r,c),candidates) in self.genAllCandidates(board):
            if len(candidates) < smallest_candidate_count:
                smallest_candidate_count = len(candidates)
                output = ((r,c),candidates)
            if len(candidates) < 2:
                return output
        return output

    def loop(self, board,r,c,v):
        # somehow we're not backtracking correctly
        if (r,c) == (0,6):
            print(board_to_string(board))
        board[r][c] = v
        
        next_candidate = self.findCoordWithLeastCandidates(board)
        if next_candidate is None:
            return
        (rnext,cnext),candidates = next_candidate
        if len(candidates) == 0:
            board[r][c] = '.'
        else:
            for candidate in candidates:
                return self.loop(board,rnext,cnext,candidate)        
        

    def solveSudoku(self, board: list[list[str]]) -> None:
        """
        Do not return anything, modify board in-place instead.
        """
        print_board(board)
        print("##################")
        ((r,c),candidates) = self.findCoordWithLeastCandidates(board)
        for candidate in candidates:
            return self.loop(board,r,c, candidate)
        print_board(board)
        

s1 = [["5","3",".",".","7",".",".",".","."],["6",".",".","1","9","5",".",".","."],[".","9","8",".",".",".",".","6","."],["8",".",".",".","6",".",".",".","3"],["4",".",".","8",".","3",".",".","1"],["7",".",".",".","2",".",".",".","6"],[".","6",".",".",".",".","2","8","."],[".",".",".","4","1","9",".",".","5"],[".",".",".",".","8",".",".","7","9"]]   
s2 = [[".",".","9","7","4","8",".",".","."],["7",".",".",".",".",".",".",".","."],[".","2",".","1",".","9",".",".","."],[".",".","7",".",".",".","2","4","."],[".","6","4",".","1",".","5","9","."],[".","9","8",".",".",".","3",".","."],[".",".",".","8",".","3",".","2","."],[".",".",".",".",".",".",".",".","6"],[".",".",".","2","7","5","9",".","."]]
s2Correct = [["5","1","9","7","4","8","6","3","2"],["7","8","3","6","5","2","4","1","9"],["4","2","6","1","3","9","8","7","5"],["3","5","7","9","8","6","2","4","1"],["2","6","4","3","1","7","5","9","8"],["1","9","8","5","2","4","3","6","7"],["9","7","5","8","6","3","1","2","4"],["8","3","2","4","9","1","7","5","6"],["6","4","1","2","7","5","9","8","3"]]
board = s2

answer = Solution()
answer.solveSudoku(board)
answer.findCoordWithLeastCandidates(board)
answer.getCandidates(s2,6,2)

list((s2[r][c] for r,c in answer.genBoxCoords(s2,6,2)))
answer.genBoxCoords(s2,6,2)
print_board(s2)
print_board(s2Correct)

answer.findCoordWithLeastCandidates(board)


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


g = answer.genAllEmptyCoords(s1)
list(g)


def f():
    def f1():
        print("f1")
    def f2():
        print("f2")
    def f3():
        return 7
    def f4():
        print("f4")

    for g in [f1,f2,f3,f4]:
        return g()

f()