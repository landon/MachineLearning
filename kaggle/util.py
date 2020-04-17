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
  return path

def denormalize_dill(dill_path, y):
  import dill
  return dill.load(open(dill_path, "rb"))(y)

def save_submission(model, root):
    import numpy as np
    import pandas as pd
    X_sub = pd.read_csv(root + 'X_sub.csv')
    denorm_dill_path = root + "denormalize.dill"
    results = denormalize_dill(denorm_dill_path, model.predict(X_sub))

    import time
    timestr = time.strftime("%Y%m%d-%H%M%S")
    return save_results(results, root, f'{timestr}.csv')
    
