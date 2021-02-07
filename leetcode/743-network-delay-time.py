# You are given a network of n nodes, labeled from 1 to n. 
# You are also given times, a list of travel times as directed 
# edges times[i] = (ui, vi, wi), where ui is the source node, 
# vi is the target node, and wi is the time it takes for a signal 
# to travel from source to target.

# We will send a signal from a given node k. Return the time it 
# takes for all the n nodes to receive the signal. If it is 
# impossible for all the n nodes to receive the signal, return -1.

from math import inf
from collections import deque


class Solution:

    # 476 ms - 59.27%
    # 16.3 MB - 30.69%

    def networkDelayTime(self, times: list[list[int]], n: int, k: int) -> int:
        travel_times = [inf] * n
        travel_times[k-1] = 0
        adjList = [[] for _ in range(n)]
        for [u,v,w] in times:
            adjList[u-1].append((v,w))

        nodes_remaining = set(range(n))

        while nodes_remaining:
            node = min(nodes_remaining, key = travel_times.__getitem__) + 1
            neighbors = adjList[node - 1]
            for neighbor,cost in neighbors:
                travel_times[neighbor - 1] = min(travel_times[neighbor - 1], travel_times[node - 1] + cost)
            nodes_remaining.remove(node - 1)

        output =  max(travel_times)
        if output == inf:
            return -1
        else:
            return output

            
        





s1 = ([[1,2,9],[1,4,2],[2,5,1],[4,2,4],[4,5,6],[3,2,3],[5,3,7],[3,1,5]],5,1)
s2 = ([[2,1,1],[2,3,1],[3,4,1]],4,2) # 2
s3 = ([[1,2,1]],2,1) # 1
s4 = ([[1,2,1]],2,2) # -1
all_samples = [s1,s2,s3,s4]

def answer(t):
    return Solution().networkDelayTime(*t)



answer(s1)
[answer(s) for s in all_samples]