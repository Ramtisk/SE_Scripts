import bpy
import os
import math

# Path to your OBJ file2
obj_path = r"C:\Users\Ramos\Documents\OutputMeshroomProjeto\texturedMesh.obj"  # Replace with the path to your OBJ file

# Path to save the .blend file
blend_file_path = r"C:\Users\Ramos\Documents\OutputMeshroomProjeto\processed_mesh.blend"  # Replace with the desired output path

# Load the OBJ file into Blender
def load_obj(filepath):
    if not os.path.exists(filepath):
        print(f"Error: File not found at {filepath}")
        return None
    
    bpy.ops.wm.obj_import(filepath=obj_path)
    return bpy.context.selected_objects[0]  # Assuming the imported object is the first selected


def remove_starting_cube():
    # Get the default cube (assuming it's the first object)
    if "Cube" in bpy.data.objects:
        cube = bpy.data.objects["Cube"]
        bpy.data.objects.remove(cube)
        print("Starter cube removed.")
    else:
        print("No starter cube found.")
# Function to create a cube at a specific location
def create_cube(location, size=5):
    bpy.ops.mesh.primitive_cube_add(size=size, location=location)
    return bpy.context.object
# Function to apply a Boolean modifier
def apply_boolean_modifier(target_object, cutter_object, operation='DIFFERENCE'):
   # Add a boolean modifier to the target object   
    bpy.ops.object.modifier_add(type='BOOLEAN')    
    # Set the modifier properties
    modifier = bpy.context.object.modifiers["Boolean"]
    modifier.operation = operation
    modifier.solver = 'FAST'
    modifier.object = cutter_object
    
    # Apply the modifier (cut out the part of the target object)
    bpy.ops.object.modifier_apply(modifier=modifier.name)

# Function to cut a mesh using 4 cubes
def cut_mesh_with_cubes(target_object):
    # Deselect all objects
    bpy.ops.object.select_all(action='DESELECT')

    # Create 4 cubes at different positions around the mesh
    cube_positions = [(-3.5, 0, 0), (3.5, 0, 0), (0, -3.5, 0), (0, 3.5, 0)]
    for pos in cube_positions:
        # Create a cube
        cube = create_cube(location=pos)
        # Duplicate the target object for each cut    
        bpy.context.view_layer.objects.active = target_object
        target_object.select_set(True)             
        # Apply the boolean operation to cut with the cube
        apply_boolean_modifier(target_object, cube, operation='DIFFERENCE')

        # Clean up the cube
        #bpy.data.objects.remove(cube)

    print("Mesh cut into 4 parts using cubes!")
def rotate_object_x(obj, angle_in_degrees):
    # Convert angle from degrees to radians  
    bpy.context.view_layer.objects.active = obj
    obj.select_set(True)      
    angle_in_radians = math.radians(angle_in_degrees)
    # Apply the rotation along the X-axis
    obj.rotation_euler[0] = angle_in_radians
    obj.location[0] += 0.8  # Adds to the current X position
    obj.location[1] -= 1.8  # Adds to the current X position
    obj.location[2] += 3.85  # Adds to the current X position
    print(f"Object rotated by {angle_in_degrees} degrees along the X-axis.")


# Main function
def main():
    # Load the OBJ file
    imported_object = load_obj(obj_path)
    rotate_object_x(imported_object,0)
    if imported_object is None:
        return
    remove_starting_cube()
    # Set the imported object as active
    bpy.context.view_layer.objects.active = imported_object
    
    # Cut the mesh with cubes
    cut_mesh_with_cubes(imported_object)

    # Save the file as .blend
    bpy.ops.wm.save_as_mainfile(filepath=blend_file_path)
    print(f"File saved as {blend_file_path}")

# Execute the main function
if __name__ == "__main__":
    main()