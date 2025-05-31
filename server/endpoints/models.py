from datetime import datetime
from typing import List

from pydantic import BaseModel


class BoardItem(BaseModel):
    score: int
    name: str
    color_r: int
    color_g: int
    color_b: int
    time: datetime
