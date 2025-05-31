from typing import Annotated, List

import mariadb
from fastapi import Path, Query
from starlette.responses import PlainTextResponse

from setup import Cursor, app, leaderboards

from endpoints.models import BoardItem


@app.get("/boards/{board}/by-score", description="Get records for a board, sorted by score.", responses=
{
    404: {"description": "Board does not exist."},
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
async def leaderboard(
        board: Annotated[str,  Path(description="What leaderboard to look up.")],
        start: Annotated[int, Query(description="First record to fetch. records are stored in descending order - "
                                                "the highest score is at index 0, the second highest is 1, etc. ")] = 0,
        count: Annotated[int, Query(description="How many records to fetch. If there are not enough records,"
                                                "only available records will be returned.", gt=0)] = 10,
        user:  Annotated[str, Query(description="If this is set to a username, only records from this user"
                                                "will be included in the results.")] = None
) -> List[BoardItem]:
    if leaderboards is not None:
        if board not in leaderboards:
            return PlainTextResponse(status_code=404)


    with Cursor() as conn:
        if conn is None: return PlainTextResponse(status_code=503)

        try:
            # Check if uid is valid
            if user:
                conn.execute(
                    "select score, name, color_r, color_g, color_b, time from boards join users where boards.user = users.id and uname = ? order by score desc limit ? offset ?",
                    (user, count, start))
            else:
                conn.execute(
                    "select score, name, color_r, color_g, color_b, time from boards join users where boards.user = users.id order by score desc limit ? offset ?",
                    (user, count, start))

            data = conn.fetchall()
            return data


        except mariadb.IntegrityError as e:
            return PlainTextResponse(status_code=500, content=f"Unknown database error (code {e.errno})")