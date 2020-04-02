import numpy as np
import scipy.stats
import math

def transform_distribution(data, cdf = scipy.stats.norm.cdf, cdf_inv = scipy.stats.norm.ppf, center=True):
    """
        return G and inverse G_inv such that
        G(data) is distributed as cdf and G_inv(G(x)) = x        
    """
    data = np.copy(data)
    data.sort()

    if center:
        mean = np.mean(data)
        std = np.std(data)
    else:
        mean = 0.0
        std = 1.0

    F = np.vectorize(lambda x: _EDF(data, x))
    G = lambda x: mean + std * cdf_inv(F(x))
    recoverF = lambda x: cdf((x - mean) / std)
    G_inv = np.vectorize(lambda x: _interpolate(data, recoverF(x)))

    return G, G_inv

def _EDF(data, x):
    edf = ((data <= x).sum() - 0.5) / len(data)
    if edf <= 0:
        return 0.5 / len(data)
    return edf

def _interpolate(data, Fx):
    epsilon = len(data) * Fx - 0.5
    j = int(epsilon)
    if j > len(data) - 2:
        return data[-1] 
    if j < 0:
        return 0
    delta = epsilon - j
    return (1 - delta) * data[j] + delta * data[j + 1]


if __name__ == "__main__":
    import matplotlib.pyplot as plt
    import pandas as pd

    cdf = scipy.stats.norm.cdf
    cdf_inv = scipy.stats.norm.ppf

 #   example of using another distribution
 #   cdf = lambda x: scipy.stats.johnsonsu.cdf(x, 2.25, 2.5)
 #   cdf_inv = lambda x: scipy.stats.johnsonsu.ppf(x, 2.25, 2.5)

    target = "SalePrice"
    idColumn = "Id"
    data = pd.read_csv("C:/Users/landon/Documents/GitHub/MachineLearning/kaggle/data/house_prices/train.csv")
    numeric_data = data.select_dtypes(include = 'number')
    numeric_data = numeric_data.fillna(0)

    samples = np.array(numeric_data[target])
    print(f'skew: {scipy.stats.skew(samples)}')
    print(f'kurt: {scipy.stats.kurtosis(samples)}')

    normalize, denormalize = transform_distribution(samples, cdf, cdf_inv)
    normalized_samples = normalize(samples)
    print('normalizing...')
    print(f'skew: {scipy.stats.skew(normalized_samples)}')
    print(f'kurt: {scipy.stats.kurtosis(normalized_samples)}')

    bins = 40
    #plt.hist(samples, edgecolor='black', linewidth=1.2, bins=bins, label='data', density=True, color='green', alpha=1)
    
    # check inversion works
    denormalized_normalized_samples = denormalize(normalized_samples)
    print(f'error: {np.max(np.abs(denormalized_normalized_samples - samples))}')
    plt.hist(denormalized_normalized_samples, edgecolor='black', linewidth=1.2, bins=bins, label='back', density=True, color='red', alpha=1)
    plt.hist(normalized_samples, edgecolor='black', linewidth=1.2, bins=bins, label='normalized', density=True, color='blue', alpha=1)
    plt.legend()
    plt.show()