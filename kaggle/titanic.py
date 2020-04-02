import numpy as np
import pandas as pd
from util import save_results, binarize
from preprocess import basic_preprocess, impute_with_median, impute_with_mode
from ground_truth import score_basic
from sklearn.preprocessing import OneHotEncoder, LabelEncoder

np.random.seed(42)

root = 'C:/Users/landon/Documents/GitHub/MachineLearning/kaggle/data/titanic/'

train = pd.read_csv(root + 'train.csv')
test = pd.read_csv(root + 'test.csv')

target_column = 'Survived'
columns_to_drop = ['PassengerId', 'Ticket', 'Name', 'Cabin', 'SibSp', 'Parch', 'Age', 'Fare']
forced_categorical = []
forced_numeric = []
skip_normalization = [target_column, 'Pclass']
use_labeler = ['Sex']

def manual_processing(features, complete_features):
    impute_with_median(complete_features)
    impute_with_mode(complete_features)

    features['Fare2'] = LabelEncoder().fit_transform(pd.qcut(complete_features['Fare'], 13, labels=False, duplicates='drop'))
    features['Age2'] = LabelEncoder().fit_transform(pd.qcut(complete_features['Age'], 10, labels=False, duplicates='drop'))

    family_size = complete_features['SibSp'] + complete_features['Parch'] + 1
    features['FamilySize'] = family_size
    features['FamilySizeBins'] = LabelEncoder().fit_transform(pd.qcut(family_size, 8, labels=False, duplicates='drop'))

    title = complete_features['Name'].str.split(', ', expand=True)[1].str.split('.', expand=True)[0]
    features['Is_Mrs'] = LabelEncoder().fit_transform(title == 'Mrs')
    features['Is_Master'] = LabelEncoder().fit_transform(title == 'Master')
    features['Title'] = LabelEncoder().fit_transform(title)
     
    features['Ticket_Frequency'] = complete_features.groupby('Ticket')['Ticket'].transform('count')


    return features


X, y, X_sub, denormalize = basic_preprocess(train, 
                                            test, 
                                            target_column, 
                                            columns_to_drop, 
                                            forced_categorical, 
                                            forced_numeric, 
                                            skip_normalization, 
                                            use_labeler,
                                            manual_processing)

import sklearn.ensemble
import sklearn.gaussian_process
import sklearn.naive_bayes
import sklearn.neighbors
import sklearn.svm
import sklearn.tree
from sklearn.model_selection import KFold
import xgboost as xgb

splits = 11
max_model_count = 300000
accuracy_cut = 0.8

soft_learners = [
    ('ada', sklearn.ensemble.AdaBoostClassifier()),
    ('bc',  sklearn.ensemble.BaggingClassifier(n_estimators=100)),
    ('etc', sklearn.ensemble.ExtraTreesClassifier(n_estimators=100)),
    ('gbc', sklearn.ensemble.GradientBoostingClassifier()),
    ('rfc', sklearn.ensemble.RandomForestClassifier(n_estimators=100)),
    ('gpc', sklearn.gaussian_process.GaussianProcessClassifier()),
    ('lr',  sklearn.linear_model.LogisticRegressionCV(cv=5, max_iter=1000)),
    ('bnb', sklearn.naive_bayes.BernoulliNB()),
    ('gnb', sklearn.naive_bayes.GaussianNB()),
    ('knn', sklearn.neighbors.KNeighborsClassifier()),
    ('svc', sklearn.svm.SVC(probability=True, gamma='scale')),
    ('xgb', xgb.XGBClassifier()),
]

voter = sklearn.ensemble.VotingClassifier(estimators=soft_learners, voting='soft')

hard_learners = [
    ('linreg', sklearn.linear_model.SGDClassifier()),
    ('ridge', sklearn.linear_model.RidgeClassifier()),
    ('passagg', sklearn.linear_model.PassiveAggressiveClassifier()),
    ('dtc', sklearn.tree.DecisionTreeClassifier()),
]

def ensemble():
    results = []
    def learn(model, tag=''):
        total_prediction = None
        total_accuracy = 0
        total_used = 0
        kf = KFold(n_splits = splits, shuffle=True)
        for train, test in kf.split(X, y):
            model.fit(X.iloc[train,:], y[train])

            y_pred = model.predict(X.iloc[test, :])
            y_pred = denormalize(y_pred)

            accuracy = sklearn.metrics.accuracy_score(y[test], y_pred)
            if accuracy >= accuracy_cut:
                yy = model.predict(X_sub)
                yy = denormalize(yy)
                if total_prediction is None:
                    total_prediction = yy
                else:
                    total_prediction += yy
                total_used += 1

                total_accuracy += accuracy

        if total_used > 0:
            results.append((total_prediction / total_used, total_accuracy / total_used, tag))
            print(f'{tag}: {total_accuracy / total_used}')
        

    for learner in soft_learners:
        learn(learner[1], learner[0])

    learn(voter, 'voter')

    for learner in hard_learners:
        learn(learner[1], learner[0])

    results = sorted(results, key=lambda x: -x[1])
    model_count = min(max_model_count, len(results))
    results = results[:model_count]

    result = sum(list(zip(*results))[0])
    binarize(result, model_count // 2)

    for i in range(len(results)):
        print(f'acc: {results[i][1]} from {results[i][2]}')

    return result

def basic():
    model =  sklearn.ensemble.RandomForestClassifier(criterion='gini', 
                                           n_estimators=1100,
                                           max_depth=5,
                                           min_samples_split=4,
                                           min_samples_leaf=5,
                                           max_features='auto',
                                           oob_score=True,
                                           random_state=42,
                                           n_jobs=-1,
                                           verbose=0)
    #model = sklearn.tree.DecisionTreeClassifier(max_depth=8)
    model.fit(X, y)

    yy = model.predict(X_sub)
    yy = denormalize(yy)

    return yy

if __name__ == '__main__':
    result = basic()
    #result = ensemble()
    save_results(result.astype(int), 'C:/kaggle_test/titanic/', 'basic.csv', root)
    score = score_basic('C:/kaggle_test/titanic/basic.csv')
    print(score)



