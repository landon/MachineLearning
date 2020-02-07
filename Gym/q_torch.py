from q import Q
import torch
import torch.nn
from torch.functional import F
from torch.autograd import Variable
from replay_memory import ReplayMemory

BATCH_SIZE = 64
ALPHA = 0.25
GAMMA = 0.75
REPLAY_MEMORY_SIZE = 2 ** 12

class QTorch(Q):
    def __init__(self, actionCount, gamma=GAMMA, alpha=ALPHA, batchSize = BATCH_SIZE, replayMemorySize = REPLAY_MEMORY_SIZE):
        super().__init__(actionCount)
        self._gamma = gamma
        self._alpha = alpha
        self._batchSize = batchSize
        self._replayMemorySize = replayMemorySize
        self._NN = None
        self._optimizer = None
        self._memory = ReplayMemory(self._replayMemorySize)

    def initNN(self, state):
        if self._NN is None:
            self._NN = QTorch.NeuralNet(len(state), self._actionCount)
            self._optimizer = torch.optim.RMSprop(self._NN.parameters())

    def getActionScores(self, state):
        self.initNN(state)
        return self._NN(Variable(torch.FloatTensor([state])).type(torch.FloatTensor))
        
    def update(self, lastState, lastAction, reward, state):
        if lastState is None:
            return

        self.initNN(state)
        self._memory.push((torch.FloatTensor([lastState]), lastAction, torch.FloatTensor([reward]), torch.FloatTensor([state])))

        if len(self._memory) < self._batchSize:
            return

        transitions = self._memory.sample(self._batchSize)
        batch_state, batch_action, batch_reward, batch_next_state = zip(*transitions)

        batch_state =  torch.cat(batch_state)
        batch_action = torch.cat(batch_action)
        batch_reward = torch.cat(batch_reward)
        batch_next_state = torch.cat(batch_next_state)

        batch_prediction = self._NN(batch_state)
        batch_next_prediction = self._NN(batch_next_state)

        current_q_values = batch_prediction.gather(1, batch_action)[:,0]
        max_next_q_values = batch_next_prediction.detach().max(1)[0]

        expected_q_values = (1.0 - self._alpha)*current_q_values + self._alpha * (batch_reward + self._gamma * max_next_q_values)

        loss = F.smooth_l1_loss(current_q_values, expected_q_values)
        self._optimizer.zero_grad()
        loss.backward()
        self._optimizer.step()

    class NeuralNet(torch.nn.Module):
        def __init__(self, in_features, out_features):
            super().__init__()
            width = 64
            self._in = torch.nn.Linear(in_features, width)
            self._out = torch.nn.Linear(width, out_features)
        
        def forward(self, x):
            x = F.relu(self._in(x))
            x = self._out(x)
            return x