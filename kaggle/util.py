import pandas as pd

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
    path = save_results(results, root, f'{timestr}.csv')
    return path

def submit_to_kaggle(path, competition):
  from kaggle.api.kaggle_api_extended import KaggleApi
  api = KaggleApi()
  api.authenticate()

  api.competition_submit(path, 'API Submission', competition)

def get_kaggle_scores(competition):
    from kaggle.api.kaggle_api_extended import KaggleApi
    api = KaggleApi()
    api.authenticate()

    prior_submissions = [p.__dict__ for p in api.competition_submissions(competition)]
    return [p["publicScore"] for p in prior_submissions]

def load_train(root):
  X = pd.read_csv(root + "X.csv")
  y = pd.read_csv(root + "y.csv").to_numpy().ravel()
  return X, y

def make_submission_with_model(model, root):
  X, y = load_train(root)
  model.fit(X, y)
  return save_submission(model, root)
