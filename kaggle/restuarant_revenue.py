from datetime import datetime
from preprocess import do_regression

if __name__ == '__main__':
  root = 'C:/kaggle/restaurant_revenue/'
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