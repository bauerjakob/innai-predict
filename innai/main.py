from typing import List

from fastapi import FastAPI
import torch
from torch.autograd import Variable

from model.inn_ai_model import InnAiModel

app = FastAPI()


@app.get("/predict")
async def predict(input: List[float]) -> List[float]:
    model = InnAiModel()
    model.load_state_dict(torch.load('model/model.pt'))

    variable = Variable(torch.tensor(input, dtype=torch.float))

    result = model(variable)
    return result
