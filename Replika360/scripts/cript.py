import bpy
import sys
import os
import math

# Read command-line arguments
argv = sys.argv
args = argv[argv.index("--") + 1:]  # Blender-specific: Arguments after "--"

if len(args) < 2:
    print("Error: Please provide the .obj path and .blend save path.")
    sys.exit(1)

obj_path = args[0]
blend_save_path = args[1]

# Function to load OBJ file into Blender
def load_obj(filepath):
    if not os.path.exists(filepath):
        print(f"Error: File not found at {filepath}")
        return None
    bpy.ops.import_scene.obj(filepath=filepath)
    return bpy.context.selected_objects[0]

# Function to remove the default cube
def remove_starting_cube():
    if "Cube" in bpy.data.objects:
        cube = bpy.data.objects["Cube"]
        bpy.data.objects.remove(cube)
        print("Starter cube removed.")
    else:
        print("No starter cube found.")

# Main processing logic
def main():
    # Remove the default cube
    remove_starting_cube()

    # Import the OBJ file
    imported_object = load_obj(obj_path)
    if not imported_object:
        print("Error: Failed to load .obj file.")
        return

    # Perform transformations or modifications here
    # Example: Rotate object
    imported_object.rotation_euler[0] += math.radians(45)

    # Save the modified scene as .blend
    bpy.ops.wm.save_as_mainfile(filepath=blend_save_path)
    print(f"Blend file saved at {blend_save_path}")

if __name__ == "__main__":
    main()