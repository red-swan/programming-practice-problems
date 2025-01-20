


unsorted_scores = [37, 89, 41, 65, 91, 53]
HIGHEST_POSSIBLE_SCORE = 100

def sort_scores(unsorted_scores, highest):
    
    score_counts = [0] * highest

    for score in unsorted_scores:
        score_counts[score] += 1
    
    sorted_scores = [None] * len(unsorted_scores)
    i = 0

    for score in range(highest - 1, -1, -1):
        count = score_counts[score]
        for _ in range(count):
            sorted_scores[i] = score
            i += 1

    return sorted_scores

# Returns [91, 89, 65, 53, 41, 37]
sort_scores(unsorted_scores, HIGHEST_POSSIBLE_SCORE)










