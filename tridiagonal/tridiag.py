import numpy as np

def solve(lower, diagonal, upper, target):
    N = len(diagonal)

    G = [np.linalg.inv(diagonal[0])]
    A = [np.dot(G[0], upper[0])]
    B = [np.dot(G[0], target[0])]
    
    for i in range(1, N):
        G.append(np.linalg.inv(diagonal[i] - np.dot(lower[i], A[-1])))
        if i < N - 1:
            A.append(np.dot(G[-1], upper[i]))
        B.append(np.dot(G[-1], target[i] - np.dot(lower[i], B[-1])))

    X = [0] * N
    X[-1] = B[-1]

    for i in reversed(range(0, N - 1)):
        X[i] = B[i] - np.dot(A[i], X[i + 1])

    return np.concatenate(X)

def to_dense(lower, diagonal, upper, target):
    N = len(diagonal)
    K = diagonal[0].shape[0]
    M = np.zeros((N * K, N * K))
    for i in range(1, N):
        M[K * i : K * (i + 1), K * (i - 1) : K * i] = lower[i]
    for i in range(0, N):
        M[K * i : K * (i + 1), K * i : K * (i + 1)] = diagonal[i]
    for i in range(0, N - 1):
        M[K * i : K * (i + 1), K * (i + 1)  : K * (i + 2)] = upper[i]
    T = np.concatenate(target)
    return M, T

if __name__ == '__main__':
    N = 11
    K = 3
    rand = True
    lower = []
    diagonal = []
    upper = []
    target = []

    if rand:
        for i in range(N):
            lower.append(np.random.rand(K, K))
            diagonal.append(np.random.rand(K, K))
            upper.append(np.random.rand(K, K))
            target.append(np.random.rand(K, K))
    else:
        for i in range(N):
            lower.append(np.eye(K, K) + 2.0 * np.ones((K, K)))
            diagonal.append(2 * np.eye(K, K) + 3 * np.ones((K, K)))
            upper.append(3 * np.eye(K, K) + 5 * np.ones((K, K)))
            target.append(7 * np.eye(K, K) + 11 * np.ones((K, K)))
    
    # sanity check: these values shouldn't be used in the algorithm
    lower[0] = None
    upper[N - 1] = None

    M, T = to_dense(lower, diagonal, upper, target)
    result_dense = np.linalg.solve(M, T)
    result_thomas = solve(lower, diagonal, upper, target)
    print(lower)
    print(diagonal)
    print(upper)
    print(target)
    print(M)
    print(T)
    print(result_dense)
    print(result_thomas)

    diff = result_dense - result_thomas
    print(f'worst_diff: {max(diff.flatten())}')



