import pandas as pd

def save_results(results, root, name):
  import os
  if not os.path.exists(root):
    os.makedirs(root)
  path = root + name
  out = pd.read_csv("C:/Users/landon/Documents/GitHub/MachineLearning/kaggle/data/house_prices/sample_submission.csv")
  out.iloc[:,1] = results
  out.to_csv(path, index = False)
  print(f'saved results to {path}')