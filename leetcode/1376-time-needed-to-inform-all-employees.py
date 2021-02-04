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



# 1940ms - 11.17%
# 38.1 MB - 83.58%




class Solution:

    def buildAdjList(self,l,n):
        output = [[] for _ in range(n)]
        for i in range(n):
            if 0 <= l[i]:
                output[l[i]].append(i)
        return output

    def _dfs(self,i,employee_tree,inform_costs):
        if not employee_tree[i]:
            return 0
        else:
            cost = inform_costs[i]
            return max([cost + self._dfs(employee,employee_tree,inform_costs) for employee in employee_tree[i]])


    def numOfMinutes(self, n: int, headID: int, manager: list[int], informTime: list[int]) -> int:
        adjList = self.buildAdjList(manager,n)
        return self._dfs(headID, adjList, informTime)




def answer(x): 
    return Solution().numOfMinutes(*x)


s1 = (1,0,[-1],[0]) # 0
s2 = (6,2,[2,2,-1,2,2,2],[0,0,1,0,0,0]) # 1
s3= (7,6,[1,2,3,4,5,6,-1],[0,6,5,4,3,2,1]) # 21
s4 = (15,0,[-1,0,0,1,1,2,2,3,3,4,4,5,5,6,6],[1,1,1,1,1,1,1,0,0,0,0,0,0,0,0]) # 3
s5 = (4,2, [3,3,-1,2],[0,0,162,914]) # 1076
s6 = (8,4,[2,2,4,6,-1,4,4,5],[0,0,4,0,7,3,6,0]) #13

[Solution().numOfMinutes(*s) for s in [s1,s2,s3,s4,s5,s6]]




answer(s6)

















