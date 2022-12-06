from random import sample
from cv2 import repeat
from AgentClient import AgentClient
import numpy as np
from Environment import Environment

class CalEnv(Environment):
    def __init__(self, port, loadModel):
        Environment.__init__(self, port)
        self.action_space = np.empty((18,))
        self.observation_space = np.empty((158,)) #numb. has to be equal to unity current state data length. TLDR same number
        self.maxAction = np.array([1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1]) #same as previous
        self.minAction = np.array([-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1])
        self.max_episode_steps = 2500#5000
        if loadModel == True:
            self.max_episode_steps = 50000
        self.trainAfterSteps = 10_000
        self.explorationSteps = 5000#20_000
        self.env_name = "Cal"