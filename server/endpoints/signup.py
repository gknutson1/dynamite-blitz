import logging
import string
import uuid
from typing import Annotated

import mariadb
from fastapi import Path, Query
from starlette.responses import PlainTextResponse

from setup import Cursor, app


@app.get("/signup", response_class=PlainTextResponse,
    description="Create a new account, with a username and (optionally) a custom color.",
    responses={
    200: {
        "content": {"text/plain": {"example": "33d2db9287d14afdbe606da96b579bb2"}},
        "description": "Sign up successful. Response body will contain a uuid that will be used as the user's password."
    },
    409: {
        "description": "Provided username has already been claimed."
    },
    422: {
        "content": {"text/plain": {"example": "Username not valid: <reason>"}},
        "description": "Request was not valid. Response body will contain a plain text error message that should be "
                       "displayed to the user."
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
def signup(
        username: Annotated[str, Query(description="The desired username. Must be less than 44 characters, "
                                                   "and cannot start or end with whitespace.")],
        red: Annotated[int, Query(description="red component of the username's color. "
                                              "Must be between 0-255. If not provided, defaults to 255.")] = 255,
        green: Annotated[int, Query(description="Green component of the username's color. "
                                                "Must be between 0-255. If not provided, defaults to 255.")] = 255,
        blue: Annotated[int, Query(description="Blue component of the username's color. "
                                               "Must be between 0-255. If not provided, defaults to 255.")] = 255
    ):

    if len(username) > 20:
        return PlainTextResponse(status_code=422, content=f"Username not valid: must be 43 characters or less.")

    if username.startswith(string.whitespace) or username.endswith(string.whitespace):
        return PlainTextResponse(status_code=422, content=f"Username not valid: must not start or end with whitespace.")

    if not 0 <= red <= 255:
        return PlainTextResponse(status_code=422, content=f"Red component must be between 0 and 255.")

    if not 0 <= green <= 255:
        return PlainTextResponse(status_code=422, content=f"Green component must be between 0 and 255.")

    if not 0 <= blue <= 255:
        return PlainTextResponse(status_code=422, content=f"Blue component must be between 0 and 255.")

    # # Generate a new uuid
    uid = uuid.uuid4()

    with Cursor(False) as conn:
        if conn is None: return PlainTextResponse(status_code=503)
        try:
            conn.execute("insert into users (uname, id, color_r, color_g, color_b) values (?, ?, ?, ?, ?)",
                         (username, uid, red, green, blue))
        except mariadb.IntegrityError as e:
            if e.errno == 1062:
                return PlainTextResponse(status_code=409)
            else:
                logging.warning(f"Database error: {e}")
                return PlainTextResponse(status_code=500, content=f"Unknown database error (code {e.errno})")

    return uid.hex
