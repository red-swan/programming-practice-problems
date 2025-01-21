



def fib(n):
    if n <0: raise ValueError("Cannot compute for negative numbers")
    if n < 2: return n

    a = 0
    b = 1

    for _ in range(n-1):
        c = a + b
        a = b
        b = c

    return c


[ fib(n) for n in range(15) ]












