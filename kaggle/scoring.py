
def identical_fraction(a, b):
    identical_count = len(list(filter(lambda p: p[0] == p[1], zip(a, b)))) 
    total_count = len(a)
    return identical_count / total_count