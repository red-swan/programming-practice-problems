
# recursive
def count_recursive(amount, coins):
    memoised = dict()
    def add(n,m,c): 
        if (n,m) in memoised:
            pass
        else:
            memoised[(n,m)] = c

    coins = sorted(coins)

    def loop(amount,i):
        if amount == 0:
            return 1
        elif amount < 0 or (i == 0 and 1 <= amount):
            return 0
        elif (amount,i) in memoised:
            return memoised[(amount,i)]
        else:
            q = amount - coins[i-1]
            count1 = loop(amount, i-1)
            count2 = loop(q, i)
            add(amount,i-1,count1)
            add(q,i,count2)
            return (count1 + count2)

    return loop(amount, len(coins))

def count(amount, coins):
    ways = [0] * (amount + 1)
    ways[0] = 1
    for i in range(len(coins)):
        for j in range(coins[i], (amount+1)):
            ways[j] = ways[j] + ways[j-coins[i]]
    return ways[amount]

coinset1 = [1,5,10,25,50,100,200,500,1000,2000,5000,10000]
coinset2 = [1,2,5,10,20,50,100,200]

count(4, [1,2,3])
count(5, [10,20,30])
count(10,[100,50,10])
count(200, cointset2)
count(2500, coinset1) #max depth reached recursively
count(10000, coinset2)