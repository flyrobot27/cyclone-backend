from flask import request, Flask, jsonify
from http import HTTPStatus as status
from flask_cors import CORS
from neo4j import GraphDatabase
from decouple import config
import json

app = Flask(__name__)
CORS(app)

URL = config("NEO4J_URL")
AUTH = (config("NEO4J_USER"), config("NEO4J_PASSWORD"))


PROCESS_NAME = "processName"
WORKSPACE_DATA = "workspaceData"
WARNINGS = "warnings"

def check_if_in_db(processName: str):
    ''' Get if a process exists within the databse.'''
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


def overwrite_existing_with_data(processName: str, workspace: dict | list, currentWarnings: dict | list):
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


def create_new_process_with_data(processName: str, workspace: dict | list, currentWarnings: dict | list):
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


def get_process_workspace(processName: str):
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


def delete_existing_process(processName: str):
    with GraphDatabase.driver(URL, auth=AUTH) as driver:
        with driver.session() as session:
            session.run("MATCH (n:Process {name: $name}) DETACH DELETE n", name=processName)
    

def get_process_name(data: dict):
    return str(data[PROCESS_NAME]).casefold().strip()

def get_workspace(data: dict):
    return data[WORKSPACE_DATA]

def get_current_warnings(data: dict):
    return data[WARNINGS]


@app.route('/api/models', methods=['POST'])
def modify_model():
    data = request.json
    overwriteExisting = request.args.get('overwriteExisting', default=False, type=bool)
    try:
        processName = get_process_name(data)
        workspace = get_workspace(data)
        currentWarnings = get_current_warnings(data)
    except KeyError:
        return jsonify({"error": "Process name not found in request body."}), status.BAD_REQUEST.value

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
def get_model():
    try:
        processName = get_process_name(request.args)
    except KeyError:
        return jsonify({"error": "Process name not found in request body."}), status.BAD_REQUEST.value

    # check if in database
    if not check_if_in_db(processName):
        return jsonify({"error": f"Process {processName} does not exist."}), status.NOT_FOUND.value

    # return the workspace and current_warnings of the process
    return get_process_workspace(processName)

@app.route('/api/models', methods=['DELETE'])
def delete_model():
    processName = get_process_name(request.args)
    
    if not check_if_in_db(processName):
        return jsonify({"error": f"Process {processName} does not exist."}), status.NOT_FOUND.value

    delete_existing_process(processName)
    return jsonify({"message": f"Process {processName} deleted."}), status.OK.value


@app.route('/api/models/list', methods=['GET'])
def list_models():
    with GraphDatabase.driver(URL, auth=AUTH) as driver:
        with driver.session() as session:
            result = session.run(
                "MATCH (n:Process) RETURN n.name")
            processNames = [record['n.name'] for record in result]
    
    return jsonify({"processName": processNames}), status.OK.value
