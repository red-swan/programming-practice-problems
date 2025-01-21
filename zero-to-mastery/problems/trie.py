

from enum import Enum, auto

end_char = "*"

class Trie():

    def __init__(self):
        self.trie = dict()
        
    def insert(self,word):
        def loop(already_there, current_dict, char):
            already_there = already_there and (char in current_dict)
            if already_there:
                current_dict = current_dict[char]
            else:
                current_dict[char] = dict()
                current_dict = current_dict[char]
            return (already_there,current_dict)
        
        current_dict = self.trie
        already_there = True

        for char in (word + end_char):
            already_there, current_dict = loop(already_there, current_dict, char)

    def __search(self,word):
        current_dict = self.trie
        def loop(d,c):
            if c in d:
                return d[c]
            else:
                return False
        for char in word:
            if current_dict:
                current_dict = loop(current_dict,char)
            else:
                return False
        return current_dict

    def search(self, word):
        current_dict = self.__search(word)
        return bool(current_dict) and end_char in current_dict
        
    def startsWith(self,word):
        current_dict = self.__search(word)
        return bool(current_dict)






trie = Trie()
trie.insert('apple')
trie.search('dog') # false
trie.insert('dog')
trie.search('dog') # true
trie.startsWith('app') # true
trie.search('app') # false
trie.insert('app') 
trie.search('app') # true










