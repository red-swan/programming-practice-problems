



class MaxHeap():
    def __init__(self,input):
        self.vals = []
        if type(input) is list:
            self.insertMany(input)
        else:
            self.insert(input)
    def __iter__(self):
        self.i = 0
        return self
    def __next__(self):
        if self.i <= len(self.vals) - 1:
            result = self.vals[self.i]
            self.i += 1
            return result
        else:
            self.i = 0
            raise StopIteration
    def __repr__(self):
        return str(self.vals)
    def _swap(self,i,j):
        temp = self.vals[i]
        self.vals[i] = self.vals[j]
        self.vals[j] = temp
    def _get_parent(self,i):
        if i == 0:
            return None
        else:
            return int((i-1)/2)
    def _get_left_child(self,i):
        j = i * 2 + 1
        if j <= len(self.vals) - 1:
            return j
        else:
            return None
    def _get_right_child(self,i):
        j = i * 2 + 2
        if j <= len(self.vals) - 1:
            return j
        else:
            return None
    def _get_max_child(self,i):
        lidx = self._get_left_child(i)
        ridx = self._get_right_child(i)
        if lidx and ridx:
            if self.vals[lidx] < self.vals[ridx]:
                return ridx
            else:
                return lidx
        elif lidx:
            return lidx
        else:
            return None
    def insert(self,item):
        self.vals.append(item)
        self.siftUp(len(self.vals) - 1)
    def insertMany(self,vals):
        for item in vals:
            self.insert(item)
    def siftUp(self,i):
        while True:
            pidx = self._get_parent(i)
            if pidx is not None:
                par = self.vals[pidx]
                if par < self.vals[i]:
                    self._swap(i, pidx)
                    i = pidx
                else:
                    break # not larger than parent
            else:
                break # no parent
    def siftDown(self,i):
        while True:
            par = self.vals[i]
            chi_idx = self._get_max_child(i)
            if chi_idx:
                chi = self.vals[chi_idx]
                if par < chi:
                    self._swap(i,chi_idx)
                    i = chi_idx
                else:
                    break
            else:
                break

    def delete(self,i):
        self.vals[i] = self.vals[-1]
        del self.vals[-1]
        if self.vals:
            self.siftDown(0)
    def isEmpty(self):
        return bool(self.vals)
    def head(self):
        return self.vals[0]
    def merge(self,heap):
        self.insertMany(heap.vals)
    def pop(self):
        result = self.vals[0]
        self.delete(0)
        return result
    def replace(self,item):
        self.vals[0] = item
        self.siftDown(0)


heap1 = MaxHeap([45,35,10,15,50,25,20,40,75])
heap2 = MaxHeap([100,1,23,13,80,52,8])
heap2.vals

heap1.merge(heap2)
heap1.pop()
heap1
heap1.insert(100)
heap1.replace(13)

sample= MaxHeap([15,12,50,7,40,22])
while sample:
    print(sample.pop())









