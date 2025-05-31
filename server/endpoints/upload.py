from typing import Annotated

import mariadb
from fastapi import Path, Query, Header
from starlette.responses import PlainTextResponse, Response

from setup import app, leaderboards, Cursor


@app.post("/boards/{board}", description="Upload a new score.",
responses={
    200: {
        "content": {None: None},
        "description": "Operation successful. Response body will be empty."
    },
    401: {
        "description": "Authorization header empty or not provided."
    },
    403: {
        "description": "Authorization header not a valid user ID."
    },
    404: {
        "description": "Board does not exist."
    },
    500: {
        "content": {"text/plain": {"example": "Internal server error: <reason>"}},
        "description": "A internal error occurred."
    },
    503: {
        "description": "The server is to busy to handle the request. This probably means that the server has ran out "
                       "of database connections. Either wait and try again later, or request the administrator "
                       "increase pool_size."
    }
})
def save_score(
        board: Annotated[str, Path(description="What leaderboard to post the score to.")],
        score: Annotated[int, Query(description="The score to post.", gt=0)],
        uid:   Annotated[str, Header(description="The user's UUID")]
):
    if leaderboards is not None:
        if board not in leaderboards:
            return PlainTextResponse(status_code=404)

    if uid is None or uid == False:
        return PlainTextResponse(status_code=401)

    with Cursor() as conn:
        if conn is None: return PlainTextResponse(status_code=503)

        try:
            # Check if uid is valid
            conn.execute("select exists (select * from users where id = ?)", (uid, ))
            if conn.fetchone() != (1,): return PlainTextResponse(status_code=403)

            # Update the score
            conn.execute("insert into boards (name, score, user) values (?, ?, ?)", (board, score, uid))
            return Response(status_code=200);

        except mariadb.IntegrityError as e:
            return PlainTextResponse(status_code=500, content=f"Unknown database error (code {e.errno})")

