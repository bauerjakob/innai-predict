from typing import List

from fastapi import FastAPI
import torch
from torch.autograd import Variable

from model.salmon_swirl_model import SalmonSwirlModel
from model.zander_zenith_model import ZanderZenithModel

app = FastAPI()

@app.post("/predict/{modelId}")
async def predict(modelId: str, input: List[float]) -> List[float]:
    model = None
    if modelId == '60740723-5ce0-4989-9d79-ecf4c436a029':
        model = SalmonSwirlModel()
    elif modelId == '043a0ff7-2f39-404a-a095-8710eff9106e':
        model = RoachRiverModel()
    elif modelId == '897fe3db-b56e-42fa-be38-555810029dea':
        model = ZanderZenithModel()
    else:
        raise Exception()

    model.load_state_dict(torch.load(f'model/{modelId}.pt'))

    variable = Variable(torch.tensor(input, dtype=torch.float))

    result = model(variable)
    return result
