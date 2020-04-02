import numpy as np
import pandas as pd

def score(path_to_truth, path_to_test, path_to_prediction, join_ids, result_id, score_function, preprocess=None):
    truth = pd.read_csv(path_to_truth)
    test = pd.read_csv(path_to_test)[join_ids]
    pred = pd.read_csv(path_to_prediction)[[result_id]]

    truth.columns = truth.columns.str.lower()
    test.columns = test.columns.str.lower()
    pred.columns = pred.columns.str.lower()

    join_ids = [x.lower() for x in join_ids]
    result_id = result_id.lower()

    truth = truth[join_ids + [result_id]]
    if preprocess:
        preprocess(truth, test, pred, join_ids, result_id)

    cat = pd.concat([test, pred], axis = 1, sort=False)
    xy = pd.merge(truth, cat, on=join_ids, validate='one_to_one', sort=False)[join_ids + [result_id + '_x', result_id + '_y']]
    
    return score_function(xy.iloc[:,0 + len(join_ids)], xy.iloc[:,1 + len(join_ids)])


def _cleanup_quotes(truth, test, pred, join_ids, result_id):
    name = join_ids[0]
    test[name] = list(map(lambda x: x.replace('"', ''), test[name]))
    truth[name] = list(map(lambda x: x.replace('"', ''), truth[name]))

def score_basic(submission):
    from scoring import identical_fraction


    root = 'C:/Users/landon/Documents/GitHub/MachineLearning/kaggle/data/titanic/'
    the_score = score( root + 'ground.csv', 
                        root + 'test.csv', 
                        submission,
                        ['Name', 'Age'],
                        'Survived',
                        identical_fraction,
                        _cleanup_quotes)
    return the_score

if __name__ == '__main__':
    import sys
    if len(sys.argv) < 2:
        submission = 'C:/kaggle_test/titanic/basic.csv'
        print(f'put your submission file as first parameter. using {submission}')
    else:
        submission = sys.argv[1]
    print(score_basic(submission))