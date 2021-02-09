# You are given a network of n nodes, labeled from 1 to n. 
# You are also given times, a list of travel times as directed 
# edges times[i] = (ui, vi, wi), where ui is the source node, 
# vi is the target node, and wi is the time it takes for a signal 
# to travel from source to target.

# We will send a signal from a given node k. Return the time it 
# takes for all the n nodes to receive the signal. If it is 
# impossible for all the n nodes to receive the signal, return -1.

from math import inf
import heapq
from collections import defaultdict


class Solution:

    # dijkstra's algorithm =====================================================
    # 476 ms - 59.27%
    # 16.3 MB - 30.69%
    def networkDelayTime(self, times: list[list[int]], n: int, k: int) -> int:
        travel_times = [inf] * n
        travel_times[k-1] = 0
        adjList = [[] for _ in range(n)]
        for [u,v,w] in times:
            adjList[u-1].append((v,w))
            
        h = []
        heapq.heappush(h,(0,k))

        while h:
            current_cost,node = heapq.heappop(h)
            for neighbor,next_cost in adjList[node - 1]:
                if current_cost + next_cost < travel_times[neighbor - 1]:
                    travel_times[neighbor - 1] = travel_times[node-1] + next_cost
                    heapq.heappush(h,(travel_times[neighbor - 1],neighbor))

        output = max(travel_times)
        if output == inf:
            return -1
        else:
            return output

    # leetcode's implementation
    # 488 ms - 50.83%
    # 16.4 MB - 30.59%
    def networkDelayTime(self, times, N, K):
        graph = defaultdict(list)
        for u, v, w in times:
            graph[u].append((v, w))

        pq = [(0, K)]
        dist = {}
        while pq:
            d, node = heapq.heappop(pq)
            if node in dist: continue
            dist[node] = d
            for nei, d2 in graph[node]:
                if nei not in dist:
                    heapq.heappush(pq, (d+d2, nei))

        return max(dist.values()) if len(dist) == N else -1
    
    # bellman ford =============================================================
    # 468 ms - 66.11%
    # 15.8 MB - 99.62%
    def networkDelayTime(self,times,n,k):
        distances = [inf] * n
        distances[k-1] = 0

        for _ in range(n-1):
            count = 0
            for u,v,w in times:
                if distances[u-1] + w < distances[v-1]:
                    distances[v-1] = distances[u-1] + w
                    count += 1
            if count == 0:
                break
        output = max(distances)

        if output == inf:
            return -1
        else:
            return output





s1 = ([[1,2,9],[1,4,2],[2,5,1],[4,2,4],[4,5,6],[3,2,3],[5,3,7],[3,1,5]],5,1) # 14
s2 = ([[2,1,1],[2,3,1],[3,4,1]],4,2) # 2
s3 = ([[1,2,1]],2,1) # 1
s4 = ([[1,2,1]],2,2) # -1
s5 = ([[1,2,1],[2,3,2],[1,3,4]],3,1) # 3
all_samples = [s1,s2,s3,s4,s5]

def answer(t):
    return Solution().networkDelayTime(*t)


# answer(s5)
[answer(s) for s in all_samples]