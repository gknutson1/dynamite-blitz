import uvicorn

from setup import app

# Load all the API endpoints
from endpoints import by_time, by_score, signup, upload, healthcheck, update_user


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)
