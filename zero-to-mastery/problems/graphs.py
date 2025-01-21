

s1Adj = [[1,3],[0],[3,8],[0,2,4,5],[3,6],[3],[4,7],[6],[8]]
s1Mat = [
    [0,1,0,1,0,0,0,0,0],
    [1,0,0,0,0,0,0,0,0],
    [0,0,1,0,0,0,0,0,1],
    [1,0,1,0,1,1,0,0,0],
    [0,0,0,1,0,0,1,0,0],
    [0,0,0,1,0,0,0,0,0],
    [0,0,0,0,1,0,0,1,0],
    [0,0,0,0,0,0,1,0,0],
    [0,0,1,0,0,0,0,0,0]
]


# bfs ==========================================================================
def bfsAdj(l, r):
    acc = []
    seen = set()
    queue = [r]
    while queue:
        r = queue.pop(0)
        acc.append(r)
        seen.add(r)
        for val in l[r]:
            if val not in seen:
                queue.append(val)
    return acc

def bfsMat(l,r):
    acc = []
    n = len(l[0])
    queue = [r]
    seen = set()
    while queue:
        r = queue.pop(0)
        acc.append(r)
        seen.add(r)
        for c in range(n):
            if l[r][c] == 1 and c not in seen:
                queue.append(c)
    return acc

bfsAdj(s1Adj,1)
bfsMat(s1Mat,1)


# dfs ==========================================================================
def dfsAdj(l,r):
    acc = []
    seen = set()
    def loop(r):
        acc.append(r)
        seen.add(r)
        for r_new in l[r]:
            if r_new not in seen:
                
                loop(r_new)
    loop(r)
    return acc

def dfsMat(l,r):
    n = len(l[0])
    acc = []
    seen = set()
    def loop(r):
        acc.append(r)
        seen.add(r)
        for c in range(n):
            if l[r][c] == 1 and c not in seen:
                loop(c)
    loop(r)
    return acc


dfsAdj(s1Adj,0)
dfsMat(s1Mat,0)















