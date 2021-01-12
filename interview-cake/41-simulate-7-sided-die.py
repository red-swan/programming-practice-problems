def rand7():
    found = False
    while not found:
        result = (rand5() -1) * 5 + rand5()
        if result <= 21:
            found = True
    return result % 7 + 1
