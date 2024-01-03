from torch import nn
import torch
import torch.nn.functional as F
from torchvision.transforms import Normalize


class RoachRiverModel(nn.Module):
    def __init__(self):
        super(RoachRiverModel, self).__init__()
        self.lin1 = nn.Linear(259, 200)
        self.lin2 = nn.Linear(200, 150)
        self.lin3 = nn.Linear(150, 100)
        self.lin4 = nn.Linear(100, 50)
        self.lin5 = nn.Linear(50, 30)
        self.lin6 = nn.Linear(30, 24)
        self.lin7 = nn.Linear(24, 24)

    def forward(self, input):
        x = F.relu(self.lin1(input))
        x = F.relu(self.lin2(x))
        x = F.relu(self.lin3(x))
        x = F.relu(self.lin4(x))
        x = F.relu(self.lin5(x))
        x = F.relu(self.lin6(x))
        x = F.sigmoid(self.lin7(x))
        return x