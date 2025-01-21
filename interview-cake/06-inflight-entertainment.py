

def find_movie_pair(flight_length, movie_lengths):
    length_set = set()
    for movie_length in movie_lengths:
        if flight_length - movie_length in length_set:
            return True
        length_set.add(movie_length)

    return False


# What if we wanted the movie lengths to sum to something close to the flight length (say, within 20 minutes)?
    # check twenty times
# What if we wanted to fill the flight length as nicely as possible with any number of movies (not just 2)?
    # store them in order and make lookup time O(n^2)
    # store all combinations up to k for lookup time O(1) but insert O(n^2)
# What if we knew that movie_lengths was sorted? Could we save some space and/or time?
    # we could walk in from the ends of the array checking if the pair of numbers gets close 
    # until we overshoot the flight time




