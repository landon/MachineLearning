from agent import Agent
import random

class RandomAgent(Agent):
    def __init__(self, actionCount):
        super().__init__(actionCount)

    def observe(self, state, reward, done):
        pass

    def act(self):
        return random.randrange(0, self._actionCount)

