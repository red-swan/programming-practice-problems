# A company has n employees with a unique ID for each employee from 0 to n - 1. 
# The head of the company has is the one with headID.

# Each employee has one direct manager given in the manager array where manager[i] is the 
# direct manager of the i-th employee, manager[headID] = -1. Also it's guaranteed that the 
# subordination relationships have a tree structure.

# The head of the company wants to inform all the employees of the company of an urgent 
# piece of news. He will inform his direct subordinates and they will inform their subordinates 
# and so on until all employees know about the urgent news.

# The i-th employee needs informTime[i] minutes to inform all of his direct subordinates 
# (i.e After informTime[i] minutes, all his direct subordinates can start spreading the news).

# Return the number of minutes needed to inform all the employees about the urgent news.



# 1404ms - 49.85%
# 39.9 MB - 79.61%




class Solution:

    # this is clever
    # for any node found, it runs up the tree and builds the values down to the input node
    # marking the "completed" nodes by destroying the input
    # lower nodes just work up until they find the higher completed node
    # and works since the head node (the boss) is always complete
    def numOfMinutes(self, n: int, headID: int, manager: list[int], informTime: list[int]) -> int:
        def dfs(i):
            if manager[i] != -1:
                informTime[i] += dfs(manager[i])
                manager[i] = -1
            return informTime[i]
        return max(map(dfs, range(n)))




def answer(x): 
    return Solution().numOfMinutes(*x)


s1 = (1,0,[-1],[0]) # 0
s2 = (6,2,[2,2,-1,2,2,2],[0,0,1,0,0,0]) # 1
s3= (7,6,[1,2,3,4,5,6,-1],[0,6,5,4,3,2,1]) # 21
s4 = (15,0,[-1,0,0,1,1,2,2,3,3,4,4,5,5,6,6],[1,1,1,1,1,1,1,0,0,0,0,0,0,0,0]) # 3
s5 = (4,2, [3,3,-1,2],[0,0,162,914]) # 1076
s6 = (8,4,[2,2,4,6,-1,4,4,5],[0,0,4,0,7,3,6,0]) #13
s7 = (3,1,[1,-1,0],[3,5,0])

[Solution().numOfMinutes(*s) for s in [s1,s2,s3,s4,s5,s6]]




answer(s7)














