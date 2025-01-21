

def get_max_profit(stock_prices):
    if len(stock_prices) < 2: raise ValueError("stock_prices must have at least length 2")
    min_price = stock_prices[0]
    max_price = stock_prices[0]
    profit = 0

    for i in range(1,len(stock_prices)):
        new_price = stock_prices[i]
        if new_price < min_price:
            min_price = new_price
            max_price = new_price
        elif max_price < new_price:
            max_price = new_price
            profit = max(profit, max_price - min_price)    
        
    return profit

stock_prices = [10, 7, 5, 8, 11, 9]
get_max_profit(stock_prices)









