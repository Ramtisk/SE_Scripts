# app/routes.py
from flask import render_template, request, send_file, Response
from app import app, mongo
from bson import ObjectId
import zipfile
import os
from io import BytesIO

@app.route('/')
def index():
    projects = mongo.db.projects.find()
    return render_template('index.html', projects=projects)

@app.route('/download/<project_id>/<filename>')
def download_file(project_id, filename):
    project = mongo.db.projects.find_one({'_id': ObjectId(project_id)})
    file = next((f for f in project['files'] if f['filename'] == filename), None)
    if file:
        return send_file(file['filepath'], as_attachment=True)
    return "File not found", 404

@app.route('/download_all/<project_id>')
def download_all_files(project_id):
    project = mongo.db.projects.find_one({'_id': ObjectId(project_id)})
    if not project:
        return "Project not found", 404

    memory_file = BytesIO()
    with zipfile.ZipFile(memory_file, 'w') as zf:
        for file in project['files']:
            zf.write(file['filepath'], arcname=os.path.basename(file['filepath']))
    memory_file.seek(0)
    return send_file(memory_file, download_name=f"{project['name']}.zip", as_attachment=True)