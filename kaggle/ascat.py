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
    print('dropping: ' + str(drop_columns))
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

  print('running iterative imputer...')
  imp = IterativeImputer(max_iter=10, sample_posterior=True, random_state=seed)
  numerics = list(features.select_dtypes(include=[np.number]).columns.values)
  imp.fit(features[numerics])
  features[numerics] = imp.transform(features[numerics])
  
  print('one-hot encoding...')
  final_features = pd.get_dummies(features).reset_index(drop=True)
  
  print('normalizing columns')
  normalize_columns(final_features)

  X = final_features.iloc[:len(y), :]
  X_sub = final_features.iloc[len(X):, :]

  overfit = []
  for i in X.columns:
      counts = X[i].value_counts()
      zeros = counts.iloc[0]
      if zeros / len(X) * 100 > 99.94:
          overfit.append(i)

  overfit = list(overfit)
  if overfit:
    print('dropping: ' + str(overfit))

  X = X.drop(overfit, axis=1).copy()
  X_sub = X_sub.drop(overfit, axis=1).copy()

  return X, y, X_sub, denormalize

def count_values(dataframe):
  count = {}
  for c in dataframe:
    count[c] = dataframe[c].value_counts()
  return count

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

    root = 'C:/Users/landon/Documents/GitHub/MachineLearning/kaggle/data/house_prices/'

    train = pd.read_csv(root + 'train.csv')
    test = pd.read_csv(root + 'test.csv')

    train['SalePrice'] = (train['SalePrice'] / 100000).astype(int)

    X, y, X_sub, denormalize = basic_preprocess(train, test, 'SalePrice', ['Id'], categorical=True)

    model = RandomForestClassifier(n_estimators=100)
    #model = LinearDiscriminantAnalysis()
    model.fit(X, y.astype(int))
    results = denormalize(model.predict(X_sub))


    #averaged = 10000 * np.ceil(averaged / 10000)
    results = 100000 * np.floor(results)

    save_results(np.round(results).astype(int), 'C:/kaggle_test/', 'basic.csv')

