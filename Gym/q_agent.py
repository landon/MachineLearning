from agent import Agent
import random
import torch
from q_torch import QTorch
import math

EPS_START = 0.8
EPS_END = 0.01
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
        action = torch.LongTensor([[random.randrange(0, self._actionCount)]])
        if self._currentState is not None:
            scores = self._Q.getActionScores(self._currentState)
            bestAction = scores.data.max(1)[1].view(1, 1)
            if random.random() > eps_threshold:
                action = bestAction
        self._lastAction = action
        return action

