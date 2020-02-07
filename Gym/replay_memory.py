import random

class ReplayMemory:
    def __init__(self, capacity):
        self._capacity = capacity
        self._memory = []

    def push(self, transition):
        self._memory.append(transition)
        if len(self._memory) > self._capacity:
            del self._memory[0]

    def sample(self, batch_size):
        return random.sample(self._memory, batch_size)

    def __len__(self):
        return len(self._memory)