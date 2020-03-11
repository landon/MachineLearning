import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import scipy.stats
import math

from util import save_results

def exp_weight(score, scores):
    return np.exp(score) / np.sum(np.exp(scores))

def avg_weight(score, scores):
    return 1.0 / len(scores)

def blend(tuples):
    scores = list(map(lambda tup: tup[0], tuples))

    X = np.zeros(len(tuples[0][1]))
    for tup in tuples:
        w = avg_weight(tup[0], scores)
        X += w * tup[1]
    return X
   
if __name__ == '__main__':
    is_scored = True

    import os
    root = 'C:/kaggle_test/2d/blend/'
    files = os.listdir(root)
    files = list(filter(lambda f: not os.path.isdir(root + f), files))

    tuples = []
    for f in files:
        if is_scored:
            score = float('0.' + f.replace('.csv', ''))
        else:
            score = 0.5
        df = pd.read_csv(root + f)
        values = df[df.columns[1]].to_numpy()
        tuples.append((1.0 - score, values))

   # results = 10000 * np.floor(blend(tuples) / 10000).astype(int)
    
    results = np.round(blend(tuples)).astype(int)
    save_results(results, root, 'blend.csv')