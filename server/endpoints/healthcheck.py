
from starlette.responses import Response

from setup import app

@app.get("/ping", response_class=Response, description="Retuns a empty 200 OK response. Use to check if the server is running.")
async def ping():
    return