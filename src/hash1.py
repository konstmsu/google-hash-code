def gen(results, head, n, k, min_i):
    for i in range(min_i, n):
        new_head = head + [i]
        if len(new_head) == k:
            results.append(new_head)
        else:
            gen(results, new_head, n, k, i + 1)


def comb(n, k):
    # returns list of indexes
    """
    >>> comb(1, 1)
    [[0]]
    >>> comb(2, 1)
    [[0], [1]]
    >>> comb(2, 2)
    [[0, 1]]
    >>> comb(3, 2)
    [[0, 1], [0, 2], [1, 2]]
    >>> comb(3, 3)
    [[0, 1, 2]]
    """
    results = []

    gen(results, [], n, k, 0)

    return results


def solve(M, p):
    best_comb = None
    best_sum = 0
    for i in range(len(p)):
        for c in comb(len(p), i + 1):
            ps = [p[j] for j in c]
            sum_ps = sum(ps)
            if best_sum <= sum_ps <= M:
                best_comb = c
                best_sum = sum_ps

    return best_comb
    # return [0, 2, 4]


def main_inputs():
    """
    >>> main_inputs()
    5
    0 1 2 3 6
    """
    with open("b_small.in") as file:

        def get_numbers():
            return [int(v) for v in file.readline().split(" ")]

        M, N = get_numbers()
        p = get_numbers()
        main(M, N, p)


def main(M, N, p):
    """
    >>> main(1,1,[1])
    1
    0
    >>> main(15,5,[1,3,5,7,9])
    3
    1 2 3
    >>> main(17, 4, [2, 5, 6, 8])
    3
    0 2 3
    """
    types = solve(M, p)
    print(len(types))
    print(" ".join(str(t) for t in types))


# inputs from https://hashcodejudge.withgoogle.com/#/rounds/4684107510448128/

# main(1, 2, 3)

