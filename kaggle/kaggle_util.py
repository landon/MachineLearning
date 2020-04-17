



def do_api():
    from kaggle.api.kaggle_api_extended import KaggleApi
    api = KaggleApi()
    api.authenticate()

    competition = 'house-prices-advanced-regression-techniques'
    prior_submissions = [p.__dict__ for p in api.competition_submissions(competition)]
    print([p["publicScore"] for p in prior_submissions])
    #api.competition_submit(f'C:/data/{competition}/submissions/basic.csv','API Submission', competition)
    #leaderboard = api.competition_leaderboard_cli(competition, view=True, csv_display=True)
    #print(leaderboard)

    #api.dataset_download_files('house-prices-advanced-regression-techniques')

    #competitions = api.competitions_list(search='cat', category="playground")
    #for comp in competitions:
    #    print(comp.ref,comp.reward,comp.userRank,sep=',')
    #api.competitions_data_list_files('titanic')

    #api.competition_download_files('titanic', path='C:/data/')
    #api.competition_download_files('house-prices-advanced-regression-techniques', path='C:/data/')



