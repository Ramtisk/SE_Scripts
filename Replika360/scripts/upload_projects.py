import os
import pymongo

def upload_projects_to_mongo(directory, db):
    for project_name in os.listdir(directory):
        project_path = os.path.join(directory, project_name)
        if os.path.isdir(project_path):
            files = []
            for root, _, filenames in os.walk(project_path):
                for filename in filenames:
                    files.append({
                        'filename': filename,
                        'filepath': os.path.join(root, filename)
                    })
            project = {
                'name': project_name,
                'files': files
            }
            db.projects.insert_one(project)

if __name__ == "__main__":
    client = pymongo.MongoClient("mongodb://localhost:27017/")
    db = client['meshroom_projects']
    upload_projects_to_mongo('C:\\Users\\Ramos\\Documents\\MeshRoom_Projects\\MeshroomCache\\Texturing', db)