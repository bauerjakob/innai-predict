from typing import List

from fastapi import FastAPI
import torch
from torch.autograd import Variable

from model.salmon_swirl_model import SalmonSwirlModel

app = FastAPI()

@app.post("/predict/{modelId}")
async def predict(modelId: str, input: List[float]) -> List[float]:
    model = None
    if (modelId == '60740723-5ce0-4989-9d79-ecf4c436a029'):
        model = SalmonSwirlModel()
    else:
        raise Exception()

    model.load_state_dict(torch.load(f'model/{modelId}.pt'))

    variable = Variable(torch.tensor(input, dtype=torch.float))

    result = model(variable)
    return result
