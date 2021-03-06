import numpy as np
import pandas as pd
from fit import transform_distribution
from util import save_results

from sklearn.experimental import enable_iterative_imputer
from sklearn.impute import IterativeImputer
from pandas.api.types import is_string_dtype
from pandas.api.types import is_numeric_dtype

def basic_preprocess(train, test, out_column, drop_columns=None, categorical=False, seed=42):
  train = train.copy()
  test = test.copy()
  if drop_columns:
    train.drop(drop_columns, axis=1, inplace=True)
    test.drop(drop_columns, axis=1, inplace=True)

  train_data = np.array(train[out_column])

  if categorical:
    normalize = lambda x: x
    denormalize = lambda x: x
  else:
    normalize, denormalize = transform_distribution(train_data)
    
  y = np.array(normalize(train_data))

  train_features = train.drop([out_column], axis=1)
  features = pd.concat([train_features, test]).reset_index(drop=True)
  
  objects = list(features.select_dtypes(include=[np.number]).columns.values)
  impute_with_mode(features, objects)

  numerics = list(features.select_dtypes(include=[np.number]).columns.values)
  if len(numerics) >= 2:
    imp = IterativeImputer(max_iter=10, sample_posterior=True, random_state=seed)
    imp.fit(features[numerics])
    features[numerics] = imp.transform(features[numerics])
  elif numerics:
    impute_with_median(features, numerics)
  
  final_features = pd.get_dummies(features).reset_index(drop=True)
  normalize_columns(final_features)

  X = final_features.iloc[:len(y), :]
  X_sub = final_features.iloc[len(X):, :]

  return X, y, X_sub, denormalize

def impute_with_median(dataframe, columns):
  for c in columns:
    dataframe[c] = dataframe[c].fillna((dataframe[c].median()))

def impute_with_mode(dataframe, columns):
  for c in columns:
    dataframe[c] = dataframe[c].fillna((dataframe[c].mode()))

def normalize_columns(dataframe, columns = None):
  if not columns:
    columns = dataframe.columns.values
  for c in columns:
    data = dataframe[c]
    if is_numeric_dtype(data):
      if data.nunique() >= 3:
        normalize, _ = transform_distribution(data)
        dataframe[c] = normalize(data)

if __name__ == '__main__':
    from sklearn.ensemble import RandomForestClassifier
    from sklearn.discriminant_analysis import LinearDiscriminantAnalysis
    from sklearn.linear_model import BayesianRidge
    from random import random

    baseline = pd.read_csv('c:/kaggle_test/bay_baseline.csv')['SalePrice'].to_numpy()

    root = 'C:/Users/landon/Documents/GitHub/MachineLearning/kaggle/data/house_prices/'

    train = pd.read_csv(root + 'train.csv')
    test = pd.read_csv(root + 'test.csv')

    train['SalePrice'] = (train['SalePrice'] / 10000).astype(int)

    train = train.drop('Id', axis=1)
    test = test.drop('Id', axis=1)

    columns = test.columns.values
    best = 1000
    while True:
      cc = list(filter(lambda _: random() < 0.1, columns))
      if not cc:
        continue

      tr = train[cc + ['SalePrice']]
      te = test[cc]

      X, y, X_sub, denormalize = basic_preprocess(tr, te, 'SalePrice', categorical=False)

      model = BayesianRidge()
      model.fit(X, y.astype(int))
      results = denormalize(model.predict(X_sub))

      results = np.round(10000 * np.floor(results)).astype(int).astype(float)

      log = np.vectorize(np.math.log2)
      loss = np.sum(np.abs(log(results) - log(baseline)))
      
      if loss < best:
        best = loss
        print(best)
        print(cc)

