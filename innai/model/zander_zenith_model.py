from torch import nn
import torch.nn.functional as F

class ZanderZenithModel(nn.Module):
    def __init__(self):
        super(ZanderZenithModel, self).__init__()
        self.lin1 = nn.Linear(1027, 800)
        self.lin2 = nn.Linear(800, 500)
        self.lin3 = nn.Linear(500, 200)
        self.lin4 = nn.Linear(200, 50)
        self.lin5 = nn.Linear(50, 24)
        self.lin6 = nn.Linear(24, 24)

    def forward(self, input):
        x = F.relu(self.lin1(input))
        x = F.relu(self.lin2(x))
        x = F.relu(self.lin3(x))
        x = F.relu(self.lin4(x))
        x = F.relu(self.lin5(x))
        x = F.sigmoid(self.lin6(x))
        return x