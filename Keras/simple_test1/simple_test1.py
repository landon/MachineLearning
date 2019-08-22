from keras.models import Sequential
from keras.layers import Dense
from sklearn.model_selection import train_test_split
import numpy

dataset = numpy.loadtxt("data.csv", delimiter=",")
X = dataset[:,0:8]
Y = dataset[:,8]

model = Sequential()
model.add(Dense(12, input_dim=8, activation='relu'))
model.add(Dense(8, activation='relu'))
model.add(Dense(1, activation='sigmoid'))
model.summary()

model.compile(loss='binary_crossentropy', optimizer='adam', metrics=['accuracy'])
model.fit(X, Y, epochs=2000, batch_size=10, validation_split=0.2)
