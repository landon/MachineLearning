import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

def binarize(arr, thresh):
    arr[arr < thresh] = 0
    arr[arr >= thresh] = 1

def save_results(results, root, name):
  save_root = root + '/submissions/'
  import os
  if not os.path.exists(save_root):
    os.makedirs(save_root)
  out = pd.read_csv(root + 'sample_submission.csv')
  out.iloc[:,1] = results
  path = save_root + name
  out.to_csv(path, index = False)
  print(f'saved results to {path}')
