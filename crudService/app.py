from flask import Response, request, Flask, jsonify
from http import HTTPStatus as status
from flask_cors import CORS
from neo4j import GraphDatabase
from decouple import config
import json
from typing import Tuple

app = Flask(__name__)
CORS(app)

URL = config("NEO4J_URL")
AUTH = (config("NEO4J_USER"), config("NEO4J_PASSWORD"))


PROCESS_NAME = "processName"
WORKSPACE_DATA = "workspaceData"
WARNINGS = "warnings"

def check_if_json_true(value: str):
    return str(value).casefold() == "true"

def check_if_in_db(processName: str) -> bool:
    """Check if a process exists in the database

    Args:
        processName (str): Name of the process

    Returns:
        bool: If the process exists in the database
    """
    existing_processes = list()
    with GraphDatabase.driver(URL, auth=AUTH) as driver:
        with driver.session() as session:
            result = session.run(
                "MATCH (n:Process) RETURN n.name")
            for record in result:
                existing_processes.append(record['n.name'])
    
    for process in map(str.casefold, existing_processes):
        if process == processName.casefold():
            return True
    return False


def overwrite_existing_with_data(processName: str, workspace: dict | list, currentWarnings: dict | list) -> str:
    """Overwrite the workspace and current_warnings of an existing process

    Args:
        processName (str): Name of the process
        workspace (dict | list): Workspace JSON
        currentWarnings (dict | list): Current warnings JSON

    Returns:
        str: Name of the process that was overwritten
    """    
    with GraphDatabase.driver(URL, auth=AUTH) as driver:
        with driver.session() as session:
            result = session.run(
                """
                MATCH (n:Process {name: $name})
                SET n.workspace = $workspace, n.current_warnings = $current_warnings
                RETURN n.name
                LIMIT 1
                """, name=processName, workspace=json.dumps(workspace), current_warnings=json.dumps(currentWarnings))

            # return the only result (There should only be 1)
            return result.single()['n.name']


def create_new_process_with_data(processName: str, workspace: dict | list, currentWarnings: dict | list) -> str:
    """Create a new process with workspace and current_warnings

    Args:
        processName (str): Name of the process
        workspace (dict | list): Workspace JSON
        currentWarnings (dict | list): Current warnings JSON

    Returns:
        str: Name of the process that was created
    """

    with GraphDatabase.driver(URL, auth=AUTH) as driver:
        with driver.session() as session:
            result = session.run(
                """
                CREATE (n:Process {name: $name, workspace: $workspace, current_warnings: $current_warnings})
                RETURN n.name
                LIMIT 1
                """, name=processName, workspace=json.dumps(workspace), current_warnings=json.dumps(currentWarnings))

            # return the only result (There should only be 1)
            return result.single()['n.name']


def get_process_workspace(processName: str) -> Tuple[Response, status]:
    """Get the workspace and current_warnings of a process

    Args:
        processName (str): Name of the process

    Returns:
        Tuple[Response, status]: response JSON, and the status value
    """    

    with GraphDatabase.driver(URL, auth=AUTH) as driver:
        with driver.session() as session:
            result = session.run(
                "MATCH (n:Process {name: $name}) RETURN n.workspace, n.current_warnings LIMIT 1", name=processName)

            record = result.single()
    
            if record is None:
                return jsonify({"error": f"Process {processName} does not exist."}), status.NOT_FOUND.value
            # get the workspace and current_warnings from the record
            workspace = json.loads(record['n.workspace'])
            currentWarnings = json.loads(record['n.current_warnings'])
            return {PROCESS_NAME: processName, "workspace": workspace, "currentWarnings": currentWarnings}, status.OK.value


def delete_existing_process(processName: str) -> None:
    """Delete an existing process from the database

    Args:
        processName (str): Name of the process
    """

    with GraphDatabase.driver(URL, auth=AUTH) as driver:
        with driver.session() as session:
            session.run("MATCH (n:Process {name: $name}) DETACH DELETE n", name=processName)
    

def get_process_name(data: dict) -> str:
    """Gets the process name from request JSON

    Args:
        data (dict): Request JSON

    Returns:
        str: Process name
    """
    return str(data[PROCESS_NAME]).casefold().strip()

def get_workspace(data: dict) -> dict | list:
    """Gets the workspace from request JSON

    Args:
        data (dict): Request JSON

    Returns:
        dict | list: Workspace JSON
    """
    return data[WORKSPACE_DATA]

def get_current_warnings(data: dict) -> dict | list:
    """Gets the current_warnings from request JSON

    Args:
        data (dict): Request JSON

    Returns:
        dict | list: Current warnings JSON
    """    
    return data[WARNINGS]


@app.route('/api/models', methods=['POST'])
def modify_model() -> Tuple[Response, status]:
    """Create or overwrite a process in the database

    Returns:
        Tuple[Response, status]: Response JSON, and the status value
    """    
    data = request.json
    overwriteExisting = request.args.get('overwriteExisting', default=False, type=check_if_json_true)
    try:
        processName = get_process_name(data)
        workspace = get_workspace(data)
        currentWarnings = get_current_warnings(data)
    except KeyError as e:
        return jsonify({"error": f"{e.args[0]} not found in request body."}), status.BAD_REQUEST.value

    # connect to database and check if process name exists in neo4j
    if check_if_in_db(processName):
        if overwriteExisting:
            overWrittenProcess = overwrite_existing_with_data(processName, workspace, currentWarnings)
            return jsonify({"message": f"Process {overWrittenProcess} overwritten."}), status.OK.value
        else:
            return jsonify({"error": "Process already exists. Use url parameter `overwriteExisting=true` to overwrite."}), status.CONFLICT.value

    # if process name does not exist, create a new process
    createdProcess = create_new_process_with_data(processName, workspace, currentWarnings)
    return jsonify({"message": f"Process {createdProcess} created."}), status.CREATED.value


@app.route('/api/models', methods=['GET'])
def get_model() -> Tuple[Response, status]:
    """Get the workspace and current_warnings of a process

    Returns:
        Tuple[Response, status]: Response JSON, and the status value
    """    

    try:
        processName = get_process_name(request.args)
    except KeyError:
        return jsonify({"error": "Process name not found in request parameters."}), status.BAD_REQUEST.value

    # check if in database
    if not check_if_in_db(processName):
        return jsonify({"error": f"Process {processName} does not exist."}), status.NOT_FOUND.value

    # return the workspace and current_warnings of the process
    return get_process_workspace(processName)

@app.route('/api/models', methods=['DELETE'])
def delete_model() -> Tuple[Response, status]:
    """Delete a process from the database

    Returns:
        Tuple[Response, status]: Response JSON, and the status value
    """

    try:
        processName = get_process_name(request.args)
    except KeyError:
        return jsonify({"error": "Process name not found in request parameters."}), status.BAD_REQUEST.value

    if not check_if_in_db(processName):
        return jsonify({"error": f"Process {processName} does not exist."}), status.NOT_FOUND.value

    delete_existing_process(processName)
    return jsonify({"message": f"Process {processName} deleted."}), status.OK.value


@app.route('/api/models/list', methods=['GET'])
def list_models() -> Tuple[Response, status]:
    """List all process names in the database

    Returns:
        Tuple[Response, status]: Response JSON, and the status value
    """

    with GraphDatabase.driver(URL, auth=AUTH) as driver:
        with driver.session() as session:
            result = session.run(
                "MATCH (n:Process) RETURN n.name")
            processNames = [record['n.name'] for record in result]
    
    return jsonify({"processName": processNames}), status.OK.value
