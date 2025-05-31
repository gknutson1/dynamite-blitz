from os import getenv
from typing import Type
from pathlib import Path

import mariadb
import json
import sys

from fastapi import FastAPI

# important config variables
leaderboards: list[str]
max_uname_len: int
max_board_len: int
pool_size: int
_conn: mariadb.ConnectionPool


def _setup():
    # Let config variables be accessible within this function
    global _conn, \
        leaderboards,\
        max_uname_len, \
        max_board_len

    def safe_get_key(data: dict, key: str, target_type: Type, default = None, section: str = "", cannot_be_false: bool = True, cannot_be_null: bool = True):
        """
        Retrive a key from a dict, with safety guards
        :param data: dict to retrieve key from
        :param key: key to retrieve from dict
        :param target_type: type that value should be. If value is not an instance of this type, a TypeError will be raised.
        :param default: If this is set and key does not exist, return this. Else if key does not exist, raise a KeyError.
        :param section: This will prefix the key name if an error message is emitted, seperated by a dot.
        :param cannot_be_false: If true and value evaluates to false, rase a ValueError
        :param cannot_be_null: If true and value is None, raise a ValueError
        :return: The value of data[key], if it exists.
        """

        if key not in data: # Handle key not existing in dict
            if default is not None:
                return default
            raise KeyError(f"{section}.{key} is not in config file")

        value = data[key]

        if target_type is not None: # Handle value not matching target type
            if not isinstance(value, target_type):
                raise TypeError(f"{section}.{key} is a {type(value)}, but it should be a {target_type}")

        if cannot_be_null and value is None: # Handle value being None
            raise ValueError(f"{section}.{key} is null")

        if cannot_be_false and value is False: # Handle value being Falsey
            raise ValueError(f"{section}.{key} is false")

        return value

    # Read config file name from environment variables. If it does not exist, default to config.json in the cwd.
    filename = Path(getenv("DYN_BLITZ_CONFIG", "config.json"))

    if not filename.is_file():
        raise FileNotFoundError(f"{filename} is not a file - is DYN_BLITZ_CONFIG set?")

    with open(filename, "r") as f:
        config = json.load(f)


    # Setup config variables
    restrict_leaderboards: bool = safe_get_key(config, "restrict_leaderboards", bool, False)
    leaderboards = None if not restrict_leaderboards else safe_get_key(config, "leaderboards", list)
    max_uname_len = safe_get_key(config, "max_uname_len", int)
    max_board_len = safe_get_key(config, "max_board_len", int, 0)
    pool_size = safe_get_key(config, "pool_size", int)

    db_dict: dict = safe_get_key(config, "database", dict)


    # Connect to MariaDB
    _conn = mariadb.ConnectionPool(
        pool_name="main_pool",
        pool_size=pool_size,
        user=    safe_get_key(db_dict, "username", str, section="database", default="dynamite_blitz"),
        password=safe_get_key(db_dict, "password", str, section="database"),
        host=    safe_get_key(db_dict, "host",     str, section="database"),
        port=    safe_get_key(db_dict, "port",     int, section="database", default=3306),
        database=safe_get_key(db_dict, "schema",   str, section="database", default="dynamite_blitz"),
        autocommit=True
    )

_setup()

class Cursor:
    """
    Helper class. Use in a with clause to ensure that an allocated cursor is properly returned to the pool.
    """
    conn = _conn
    cur: mariadb.Cursor = None
    dictionary:bool

    def __init__(self, dictionary: bool = True):
        self.dictionary = dictionary

    def __enter__(self) -> mariadb.Cursor:
        if len(self.conn._connections_free) == 0:
            return None
        self.cur = self.conn.get_connection()
        return self.cur.cursor(dictionary=self.dictionary)

    def __exit__(self, exc_type, exc_val, exc_tb):
        if self.cur is None: return
        self.cur.close()


# The main FastAPI object
app = FastAPI()