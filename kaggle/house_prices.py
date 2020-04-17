from preprocess import preprocess
from kaggle_util import make_submission_with_model, get_kaggle_scores, submit_to_kaggle

def house_prices_preprocess(root):
  target_column = 'SalePrice'
  columns_to_drop = ['Id']
  forced_categorical = []
  forced_numeric = []
  columns_to_normalize = [target_column]
  use_labeler = []

  def manual_processing(features, complete_features):
    return features

  preprocess(root, 
                target_column, 
                columns_to_drop, 
                forced_categorical, 
                forced_numeric, 
                columns_to_normalize, 
                use_labeler, 
                manual_processing)

if __name__ == '__main__':
  competition = 'house-prices-advanced-regression-techniques'
  root = f'C:/data/{competition}/' 

  print(get_kaggle_scores(competition))

  from sklearn.linear_model import BayesianRidge
  model = BayesianRidge()
  path = make_submission_with_model(model, root)

  submit_to_kaggle(path, competition)