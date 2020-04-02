import numpy as np
import pandas as pd
from fit import transform_distribution
from util import save_results

from sklearn.experimental import enable_iterative_imputer
from sklearn.impute import IterativeImputer
from sklearn.preprocessing import OneHotEncoder, LabelEncoder
from sklearn import feature_selection
from sklearn import model_selection
from sklearn import metrics


from pandas.api.types import is_string_dtype
from pandas.api.types import is_numeric_dtype

from datetime import datetime

def basic_preprocess(train_complete, 
                     test_complete, 
                     out_column, 
                     drop_columns=None,
                     forced_categorical = None, 
                     forced_numeric = None, 
                     columns_to_normalize = None,
                     use_labeler = None,
                     manual_processing = None,
                     seed=42):
  complete_features = pd.concat([train_complete, test_complete], sort=False).reset_index(drop=True)
  train = train_complete.copy()
  test = test_complete.copy()

  normalize_output = columns_to_normalize and out_column in columns_to_normalize
  if normalize_output:
    columns_to_normalize.remove(out_column)

  if use_labeler:
    if not columns_to_normalize:
      columns_to_normalize = []
    for column in use_labeler:
      if column in columns_to_normalize:
        columns_to_normalize.remove(column)

  convert_dict = {}
  if forced_categorical:
    for column in forced_categorical:
      convert_dict[column] = 'str'
  
  if forced_numeric:
    for column in forced_numeric:
      convert_dict[column] = 'float64'

  train = train.astype(convert_dict)
  test = test.astype(convert_dict) 

  if drop_columns:
    train.drop(drop_columns, axis=1, inplace=True)
    test.drop(drop_columns, axis=1, inplace=True)

  train_data = np.array(train[out_column])

  if normalize_output:
    normalize, denormalize = transform_distribution(train_data)
  else:
    normalize = lambda x: x
    denormalize = lambda x: x
    
  y = np.array(normalize(train_data))

  train_features = train.drop([out_column], axis=1)
  features = pd.concat([train_features, test], sort=False).reset_index(drop=True)
  
  impute_with_mode(features)

  numerics = list(features.select_dtypes(include=[np.number]).columns.values)
  if len(numerics) >= 2:
    imp = IterativeImputer(max_iter=10, sample_posterior=False, random_state=seed)
    imp.fit(features[numerics])
    features[numerics] = imp.transform(features[numerics])
  elif numerics:
    impute_with_median(features)

  if use_labeler:
    labeler = LabelEncoder()
    for column in use_labeler:
      features[column] = labeler.fit_transform(features[column])
  
  final_features = pd.get_dummies(features).reset_index(drop=True)
  if columns_to_normalize:
    normalize_columns(final_features, columns_to_normalize)

  if manual_processing:
    final_features = manual_processing(final_features, complete_features)

  final_features.to_csv('final_features.csv')

  X = final_features.iloc[:len(y), :]
  X_sub = final_features.iloc[len(X):, :]

  X, X_sub = select_features(X, y, X_sub, final_features.columns)

  return X, y, X_sub, denormalize

def impute_with_median(dataframe):
  columns = list(dataframe.select_dtypes(include=[np.number]).columns.values)
  for c in columns:
    the_median = dataframe[c].median()
    dataframe[c] = dataframe[c].fillna(the_median)

def impute_with_mode(dataframe):
  columns = list(dataframe.select_dtypes(exclude=[np.number]).columns.values)
  for c in columns:
    the_mode = dataframe[c].mode()[0]
    dataframe[c] = dataframe[c].fillna(the_mode)
    

def normalize_columns(dataframe, columns_to_normalize):
  for c in columns_to_normalize:
    data = dataframe[c]
    if is_numeric_dtype(data):
      if data.nunique() >= 3:
        normalize, _ = transform_distribution(data)
        dataframe[c] = normalize(data)

def select_features(X, y, X_sub, feature_name, perc=10, max_depth=5, verbose=0):
  import sklearn.ensemble
  from boruta import BorutaPy

  rf = sklearn.ensemble.RandomForestRegressor(max_depth=max_depth)
  feat_selector = BorutaPy(rf, n_estimators='auto', perc=perc, verbose=verbose)
  feat_selector.fit(X.values, y)

  used_features = [feature_name[i] for i, x in enumerate(feat_selector.support_) if x]
  print(used_features)
  return feat_selector.transform(X.values), feat_selector.transform(X_sub.values)


def do_regression(root,
                  out_column, 
                  drop_columns=None,
                  forced_categorical = None, 
                  forced_numeric = None, 
                  columns_to_normalize = None,
                  use_labeler = None,
                  manual_processing = None,
                  round_results = True):
  train = pd.read_csv(root + 'train.csv')
  test = pd.read_csv(root + 'test.csv')

  print('loaded data')

  X, y, X_sub, denormalize = basic_preprocess(train, 
                                              test, 
                                              target_column, 
                                              columns_to_drop, 
                                              forced_categorical, 
                                              forced_numeric, 
                                              columns_to_normalize, 
                                              use_labeler,
                                              manual_processing)

  print('preprocessed')

  import sklearn.ensemble
  model = sklearn.ensemble.RandomForestRegressor(n_estimators=100)
  model.fit(X, y)
  results = denormalize(model.predict(X_sub))

  if round_results:
    results = np.round(results).astype(int)

  save_results(results, root, 'basic.csv')

if __name__ == '__main__':
  root = 'C:/Users/landon/Documents/GitHub/MachineLearning/kaggle/data/restaurant_revenue/'
  target_column = 'revenue'
  columns_to_drop = ['Id', 'Open Date', 'City']
  forced_categorical = []
  forced_numeric = []
  columns_to_normalize = ['revenue']
  use_labeler = ['P' + str(i) for i in range(1, 38)]

  def manual_processing(features, complete_features):
    features['days_since_open'] = [(datetime(2020, 1, 1) - datetime.strptime(f, '%m/%d/%Y')).days for f in complete_features['Open Date']]
    return features

  do_regression(root, 
                target_column, 
                columns_to_drop, 
                forced_categorical, 
                forced_numeric, 
                columns_to_normalize, 
                use_labeler, 
                manual_processing)