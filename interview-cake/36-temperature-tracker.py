


class TempTracker(object):

    # Implement methods to track the max, min, mean, and mode
    def __init__(self):
        self.max = None
        self.min = None
        self.cumsum = 0
        self.counts = [0] * 111 # 0-110
        self.max_count = 0
        self.mode = None
        self.i = 0

    def insert(self, temperature):
        self.i += 1
        if self.max is None or self.max < temperature:
            self.max = temperature
        if self.min is None or temperature < self.min:
            self.min = temperature
        self.cumsum += temperature
        self.counts[temperature] += 1
        if self.max_count < self.counts[temperature]:
            self.mode = temperature
            self.max_count = self.counts[temperature]
        

    def get_max(self):
        return self.max

    def get_min(self):
        return self.min

    def get_mean(self):
        if self.i == 0:
            return None
        else:
            return self.cumsum / self.i

    def get_mode(self):
        return self.mode


















