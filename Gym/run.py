import gym
from q_agent import QAgent

def exercise_agent(gymName, episodes, render=True, convolutional=False):
    max_t = 0
    env = gym.make(gymName).env
    agent = QAgent(env.action_space.n, convolutional)
    for i_episode in range(episodes):
        state = env.reset()
        agent.observe(state, 0, False)
        total_reward = 0
        for t in range(10000):
            if render:
                env.render()

            action = agent.act()
            state, reward, done, info = env.step(action[0, 0].item())
            total_reward += reward
            agent.observe(state, reward, done)
            if done:
                max_t = max(max_t, t)
                print(f'{t} : {max_t} : {total_reward}')
                break
    env.close()


exercise_agent('Breakout-ram-v0', 100000, render=True)