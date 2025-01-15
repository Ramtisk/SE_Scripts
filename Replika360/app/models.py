from pymongo import MongoClient
import gridfs
from flask import send_file
from bson.objectid import ObjectId
import os

# Conectar ao MongoDB
client = MongoClient('mongodb://localhost:27017/')
db = client['3d_models']
fs = gridfs.GridFS(db)

# Função que vai guardar o projeto
def save_project(directory_path, metadata):
    for root, dirs, files in os.walk(directory_path):
        for file in files:
            file_path = os.path.join(root, file)
            with open(file_path, 'rb') as f:
                file_id = fs.put(f, filename=file_path)
                metadata['files'].append({
                    'file_id': str(file_id),
                    'filename': file,
                    'path': file_path
                })
    db.projects.insert_one(metadata)
    return metadata

# Função para obter os projetos
def get_projects():
    return db.projects.find()

# Função para obter os arquivos de um projeto
def get_project_files(project_id):
    project = db.projects.find_one({'_id': ObjectId(project_id)})
    return project['files'] if project else []

# Função para baixar um arquivo
def download_file(file_id):
    try:
        file_id = ObjectId(file_id)  # Certificar-se de que o ID é um ObjectId
        file = fs.get(file_id)
        return send_file(file, download_name=file.filename, as_attachment=True)
    except gridfs.errors.NoFile:
        print(f"No file found with ID: {file_id}")
        return "File not found", 404
    except Exception as e:
        print(f"Error: {e}")
        return "An error occurred", 500
