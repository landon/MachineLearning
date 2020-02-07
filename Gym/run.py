import gym
from random_agent import RandomAgent
from q_agent import QAgent

AgentType = QAgent
def exercise_agent(gymName, episodes, render=True):
    max_t = 0
    env = gym.make(gymName).env
    agent = AgentType(env.action_space.n)
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


exercise_agent('MountainCar-v0', 1000, render=True)