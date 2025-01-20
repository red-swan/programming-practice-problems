# There are a total of numCourses courses you have to take, labeled from 0 to numCourses - 1. 
# You are given an array prerequisites where prerequisites[i] = [ai, bi] indicates that you 
# must take course bi first if you want to take course ai.

# For example, the pair [0, 1], indicates that to take course 0 you have to first take course 1.

# Return true if you can finish all courses. Otherwise, return false.

# 1048 ms - 5.0%
# 15.4 MB - 94.13%

class Solution:
    # bfs
    def canFinishBfs(self, numCourses: int, prerequisites:list[list[int]]) -> bool:
        adjList = [[] for _ in range(numCourses)]
        for [fst,snd] in prerequisites:
            adjList[snd].append(fst)
        # bfs 
        for i in range(numCourses):
            seen = set()
            queue = [i]
            while queue:
                node = queue.pop(0)
                seen.add(node)
                for next_node in adjList[node]:
                    if next_node == i:
                        return False
                    elif next_node in seen:
                        pass
                    else:
                        queue.append(next_node)
        return  True
    def canFinish(self, numCourses, prerequisites):
        # topological sort
        # 92ms - 90.80%
        # 15.4MB - 84.88%
        adjList = []
        inDegree = []
        for _ in range(numCourses):
            adjList.append([])
            inDegree.append(0)
        for [fst,snd] in prerequisites:
            inDegree[fst] += 1
            adjList[snd].append(fst)
        stack = []
        for i,item in enumerate(inDegree):
            if item == 0:
                stack.append(i)
        count = 0
        while stack:
            item = stack.pop()
            count += 1
            neighbors = adjList[item]
            for i,neighbor in enumerate(neighbors):
                inDegree[neighbor] -= 1
                if inDegree[neighbor] == 0:
                    stack.append(neighbor)
        
        return count == numCourses
        



s1 = (2,[[1,0]]) # true
s2 = (2,[[1,0],[0,1]]) # false
s3 = (6,[[1,0],[2,1],[2,5],[0,3],[4,3],[3,5],[4,5]]) # true
s4 = (7, [[0,3],[1,0],[2,1],[4,5],[6,4],[5,6]]) # false
s5 = (0,[]) # true
def answer(x): 
    return Solution().canFinish(*x)



answer(s3)
# [answer(x) for x in [s1,s2,s3,s4,s5]]












