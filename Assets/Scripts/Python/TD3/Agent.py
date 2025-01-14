import threading
import torch
from Actor import Actor
from Critic import Critic
from ReplayBuffer import ReplayBuffer
import os
import numpy as np
from TD3 import TD3
import time


def mkdir(base, name):
    path = os.path.join(base, name)
    if not os.path.exists(path):
        os.makedirs(path)
    return path

class Agent(threading.Thread):

  def __init__(self, port, manager, env):
    threading.Thread.__init__(self)
    self.manager = manager
    self.port = port

    self.env = env
    self.state_dim = self.env.observation_space.shape[0]
    self.action_dim = self.env.action_space.shape[0]
    self.max_episode_steps = self.env.max_episode_steps
    self.episode_num = 0
    self.done = True
    self.movingAvgReward = 0

  #evaluations = [policy.evaluate_policy(env)]
  

  def run(self):
  ### TRAINING
    try:
      obs = 0
      episode_timesteps = 0
      episode_reward = 0
      while self.manager.total_timesteps < self.manager.max_timesteps:
        # If the episode is done
        if self.done:
          # When the training step is done, we reset the state of the environment
          obs = self.env.reset()
          # Set the Done to False
          self.done = False

          self.movingAvgReward = 0.9 * self.movingAvgReward + 0.1*episode_reward
          # Set rewards and episode timesteps to zero
          episode_reward = 0
          episode_timesteps = 0
          self.episode_num += 1
        
        # Before n timesteps, we play random actions
        if self.manager.total_timesteps < self.env.explorationSteps and self.manager.loadModel == False:
          action = self.env.actionSample()
        else: # After 10000 timesteps, we switch to the model
          while(self.manager.policy.isTraining):
            time.sleep(1)
          action = self.manager.policy.select_action(np.array(obs))
          # If the explore_noise parameter is not 0, we add noise to the action and we clip it
          if self.manager.expl_noise != 0:
            action = (action + np.random.normal(0, self.manager.expl_noise, size=self.env.action_space.shape[0])).clip(self.env.minAction, self.env.maxAction)

        # The agent performs the action in the environment, then reaches the next state and receives the reward
        new_obs, reward, self.done = self.env.step(action)

        if(episode_timesteps + 1 == self.env.max_episode_steps):
          self.done = True
        # We check if the episode is done
        done_bool = 1 if episode_timesteps + 1 == self.env.max_episode_steps else float(self.done)

        # We increase the total reward
        episode_reward += reward
        
        # We store the new transition into the Experience Replay memory (ReplayBuffer)
        self.manager.replayBuffer.add((obs, new_obs, action, reward, done_bool))
        # We update the state, the episode timestep, the total timesteps, and the timesteps since the evaluation of the policy
        obs = new_obs
        episode_timesteps += 1
        self.manager.AddTimesteps()
    finally:
          print('ended')


  


