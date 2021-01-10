def mergeAll(meetings):
    if len(meetings) == 1: return meetings
    sorted_meetings = sorted(meetings)
    merged_meetings = [sorted_meetings[0]]
    for s2,e2 in sorted_meetings[1:]:
        _,e1 = merged_meetings[-1]

        if e2 <= e1:
            pass
        elif s2 <= e1:
            e1 = e2
        else:
            merged_meetings.append((s2,e2)) 


    return merged_meetings

sample1 = [(0, 1), (3, 5), (4, 8), (10, 12), (9, 10)]
sample2 = [(1, 5), (2, 3)]
sample3 = [(1, 10), (2, 6), (3, 5), (7, 9)]
sample4 = [(1, 3), (2, 4)]
sample5 = [(1, 2), (2, 3)]

mergeAll(sample1)
mergeAll(sample2)
mergeAll(sample3)
mergeAll(sample4)
mergeAll(sample5)
