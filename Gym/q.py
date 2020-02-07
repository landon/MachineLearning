import torch

class Q:
    def __init__(self, actionCount):
        self._actionCount = actionCount

    def getActionScores(self, state):
        return torch.zeros(self._actionCount)

    def update(self, lastState, lastAction, reward, state):
        pass