
import itertools as it
import time

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

def grouper(iterable, n, fillvalue=None):
    "Collect data into fixed-length chunks or blocks"
    # grouper('ABCDEFG', 3, 'x') --> ABC DEF Gxx"
    args = [iter(iterable)] * n
    return it.zip_longest(*args, fillvalue=fillvalue)

def grid_from_string(str):
    grouped = grouper(str,9)
    return list(map(list,grouped))

def getBoxId(row,col):
    return 3*(row // 3) + (col // 3 )
def isValid(box:set[str],row:set[str],col:set[str],num):
    return num not in box and num not in row and num not in col
def solvBacktrack(board,boxes:list[set[str]],rows:list[set[str]],cols:list[set[str]],r,c):
    if r == len(board) or c == len(board[0]):
        return True
    else:
        if board[r][c] == '.':
            for num in map(str,range(1,10)):
                board[r][c] = num
                box = boxes[getBoxId(r,c)]
                row = rows[r]
                col = cols[c]

                if isValid(box,row,col,num):
                    box.add(num)
                    row.add(num)
                    col.add(num)

                    if c == len(board[0]) - 1:
                        if solvBacktrack(board,boxes, rows, cols, r + 1, 0):
                            return True
                    else:
                        if solvBacktrack(board, boxes, rows, cols,r, c + 1):
                            return True
                    box.remove(num)
                    row.remove(num)
                    col.remove(num)
                
                board[r][c] = '.'

        else:
            if c == len(board[0]) - 1:
                if solvBacktrack(board,boxes, rows, cols, r + 1, 0):
                    return True
            else:
                if solvBacktrack(board, boxes, rows, cols,r, c + 1):
                    return True
        return False


def solveSudoku(board_str) -> None:
    board = grid_from_string(board_str)
    n = len(board)
    boxes = [set() for _ in range(n)]
    rows  = [set() for _ in range(n)]
    cols  = [set() for _ in range(n)]

    for r in range(n):
        for c in range(n):
            if board[r][c] != '.':
                boxId = getBoxId(r,c)
                num = board[r][c]
                boxes[boxId].add(num)
                rows[r].add(num)
                cols[c].add(num)

    solvBacktrack(board,boxes,rows,cols,0,0)
    return board

        
        

s1 = "53..7....6..195....98....6.8...6...34..8.3..17...2...6.6....28....419..5....8..79" #   0.06 seconds
s2 = "..9748...7.........2.1.9.....7...24..64.1.59..98...3.....8.3.2.........6...2759.." #   0.12 seconds
s3 = ".......1.4.........2...........5.4.7..8...3....1.9....3..4..2...5.1........8.6..." # 213    seconds
all_samples = [s1,s2,s3]

solveSudoku(s1)


for sample in all_samples:
    start_time = time.time()
    solved = solveSudoku(sample)
    print("--- %s seconds ---" % (time.time() - start_time))
    print(board_to_string(solved))
