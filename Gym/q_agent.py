from agent import Agent
import random
from q_torch import QTorch
import math
import numpy as np

EPS_START = 0.99
EPS_END = 0.001
EPS_DECAY = 200

class QAgent(Agent):
    def __init__(self, actionCount, convolutional=False):
        super().__init__(actionCount)
        self._Q = QTorch(actionCount, convolutional)
        self._currentState = None
        self._lastAction = 0
        self._iterations = 0

    def observe(self, state, reward, done):
        self._Q.update(self._currentState, self._lastAction, reward, state)
        self._currentState = state

    def act(self):
        self._iterations += 1
        eps_threshold = EPS_END + (EPS_START - EPS_END) * math.exp(-1.0 * self._iterations / EPS_DECAY)
        action = random.randrange(0, self._actionCount)
        if self._currentState is not None:
            scores = self._Q.getActionScores(self._currentState)
            bestAction = np.argmax(scores)
            if random.random() > eps_threshold:
                action = bestAction
        self._lastAction = action
        return action

