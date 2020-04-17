from preprocess import do_regression

if __name__ == '__main__':
  root = 'C:/kaggle/santander_value/'
  target_column = 'target'
  columns_to_drop = ['ID']
  forced_categorical = []
  forced_numeric = []
  columns_to_normalize = [target_column]
  use_labeler = []

  def manual_processing(features, complete_features):
    return features

  do_regression(root, 
                target_column, 
                columns_to_drop, 
                forced_categorical, 
                forced_numeric, 
                columns_to_normalize, 
                use_labeler, 
                manual_processing)