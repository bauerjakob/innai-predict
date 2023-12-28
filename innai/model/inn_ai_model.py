from torch import nn
import torch
import torch.nn.functional as F
from torchvision.transforms import Normalize



class InnAiModel(nn.Module):
    def __init__(self):
        super(InnAiModel, self).__init__()
        self.lin1 = nn.Linear(67, 50)
        self.lin2 = nn.Linear(50, 50)
        self.lin3 = nn.Linear(50, 30)
        self.lin4 = nn.Linear(30, 24)
        self.lin5 = nn.Linear(24, 24)

    def forward(self, input):
        x = F.relu(self.lin1(input))
        x = F.relu(self.lin2(x))
        x = F.relu(self.lin3(x))
        x = F.relu(self.lin4(x))
        x = F.sigmoid(self.lin5(x))
        return x

# class InnAiModel(nn.Module):
#     def __init__(self):
#         super(InnAiModel, self).__init__()
#
#         self.conv2 = nn.Conv2d(10, 4, kernel_size=4)
#         self.conv1 = nn.Conv2d(4, 10, kernel_size=4)
#         self.lin1 = nn.Linear(3, 3)
#
#         self.lin2 = nn.Linear(579, 100)
#         self.lin3 = nn.Linear(100, 50)
#         self.lin4 = nn.Linear(50, 3)
#
#     def forward(self, precipitationMaps, currentInnLevels):
#         x1 = F.sigmoid(self.conv1(precipitationMaps).float())
#         x1 = F.sigmoid(self.conv2(x1))
#
#         x2 = F.sigmoid(self.lin1(currentInnLevels))
#
#         x = torch.cat((x1.view(x1.size(0), -1),
#                               x2.view(x2.size(0), -1)), dim=1)
#
#         x = F.sigmoid(self.lin2(x))
#         x = F.sigmoid(self.lin3(x))
#         x = self.lin4(x)
#
#         return x
