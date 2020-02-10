import gym
from q_agent import QAgent
import numpy as np


def normalize(state):
    if np.isscalar(state):
        state = [state]
    return state.flatten()

def exercise_agent(gymName, episodes, render=True, convolutional=False):
    max_t = 0
    env = gym.make(gymName)
    agent = QAgent(env.action_space.n, convolutional)
    for i_episode in range(episodes):
        state = normalize(env.reset())
        agent.observe(state, 0, False)
     
        total_reward = 0
        for t in range(10000):
            if render:
                env.render()

            action = agent.act()
            state, reward, done, info = env.step(action)
            state = normalize(state)
            total_reward += reward
            agent.observe(state, reward, done)
            if done:
                max_t = max(max_t, t)
                print(f'{t} : {max_t} : {total_reward}')
                break
    env.close()


exercise_agent('CartPole-v0', 100000, render=False)
#exercise_agent('Pong-v0', 100000, render=False)
#exercise_agent('MsPacman-v0', 100000, render=True)