class Trie(object):

    def __init__(self): 
        self.trie = dict()

    def __loop(self, already_there, current_dict, char):
        already_there = already_there and (char in current_dict)
        if already_there:
            current_dict = current_dict[char]
        else:
            current_dict[char] = dict()
            current_dict = current_dict[char]
        return (already_there,current_dict)
    
    def add_word(self, word):
        current_dict = self.trie
        already_there = True
        for char in (word + "<"):
            already_there, current_dict = self.__loop(already_there, current_dict, char)
        
        return not already_there




trie = Trie()
trie.add_word("catch")
trie.add_word("cakes")
trie.add_word("cake")
trie.trie







