import torch.nn as nn
import torch.nn.functional as F
import torch


class Critic(nn.Module):
  
  def __init__(self, state_dim, action_dim):
    super(Critic, self).__init__()
    # Defining the first Critic neural network
    self.layer_1 = nn.Linear(state_dim + action_dim, 1024)
    self.layer_2 = nn.Linear(1024, 512)
    self.layer_3 = nn.Linear(512, 300)
    self.layer_4 = nn.Linear(300, 1)
    # Defining the second Critic neural network
    self.layer_5 = nn.Linear(state_dim + action_dim, 1024)
    self.layer_6 = nn.Linear(1024, 512)
    self.layer_7 = nn.Linear(512, 300)
    self.layer_8 = nn.Linear(300, 1)

  def forward(self, x, u):
    xu = torch.cat([x, u], 1)
    # Forward-Propagation on the first Critic Neural Network
    x1 = F.relu(self.layer_1(xu))
    x1 = F.relu(self.layer_2(x1))
    x1 = F.relu(self.layer_3(x1))
    x1 = self.layer_4(x1)
    # Forward-Propagation on the second Critic Neural Network
    x2 = F.relu(self.layer_5(xu))
    x2 = F.relu(self.layer_6(x2))
    x2 = F.relu(self.layer_7(x2))
    x2 = self.layer_8(x2)
    return x1, x2

  def Q1(self, x, u):
    xu = torch.cat([x, u], 1)
    x1 = F.relu(self.layer_1(xu))
    x1 = F.relu(self.layer_2(x1))
    x1 = F.relu(self.layer_3(x1))
    x1 = self.layer_4(x1)
    return x1