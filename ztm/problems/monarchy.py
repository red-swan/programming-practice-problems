
from enum import Enum, auto
from collections import deque

class Status(Enum):
    ALIVE = auto()
    DEAD = auto()

class Monarchy():

    def __init__(self,name):
        self.name = name
        self.status = Status.ALIVE
        self.children = []

    def nlr(self):
        def loop(item,acc):
            acc.append(item)
            for child in item.children:
                loop(child,acc)
        output = [] 
        loop(self,output)
        return output
        

    def find(self,name):
        queue = deque([self])
        while queue:
            item = queue.popleft()
            if item.name == name:
                return item
            else:
                for child in item.children:
                    queue.append(child)

    def birth(self,child,parent):
        new_child = Monarchy(child)
        royal = self.find(parent)
        royal.children.append(new_child)
    def death(self,name):
        royal = self.find(name)
        royal.status = Status.DEAD
    def getOrderOfSuccession(self):
        return [royal.name for royal in self.nlr() if royal.status == Status.ALIVE]




house_of_ztm = Monarchy("Jake")
births = [
    ("Catherine","Jake"), ('Jane','Catherine'),('Farah','Jane'),
    ("Tom",'Jake'), ('Celine','Jake'), ('Mark','Catherine'),
    ('Peter','Celine')
]
for birth in births:
    house_of_ztm.birth(*birth)

house_of_ztm.getOrderOfSuccession()


for name in ["Jake","Jane"]:
    house_of_ztm.death(name)

house_of_ztm.getOrderOfSuccession()









