# RustEdit-Unity
Basic Unity based map editor for Rust

Made in Unity 2019.2.0f1

Requires that you have the EditorCoroutines package installed from the package manager (https://docs.unity3d.com/Packages/com.unity.editorcoroutines@0.0/manual/index.html)

The editor runs in edit mode of Unity. Attempting to run in play mode will only force a reload of the asset bundles.

To get started select the "WorldManager" object in the hierarchy. 

Before you start making dopeasf maps select your Rust install directory (/steamapps/common/Rust/) by clicking "Select Rust Directory"

The path can be changed at anytime by clicking "Select Rust Directory" again.

# World Manager
The WorldManager object is where you create/load and save maps.

Once you have created or loaded a map you will be able to access the other components on the WorldManager object.

If you want to start the scene from scratch hit the "Reset" button

# Terrain Manager
Allows you to switch between different texture types. 

To paint those texture select the Terrain object from the hierarchy and use the paint tools as you would normally

# Prefab Manager
Allows you to spawn prefabs. Select the prefab you want to spawn via the dropdown menu and click "Spawn"

The prefab list is not filtered for what can and can't be used. There will be 100's of unusable prefabs in it

If you are spawning prefabs from outside of the inspector (third party plugins), ensure all spawned prefabs have a PrefabData component attached to them.

Prefabs without a PrefabData component will be ignored when saving the map

# Path Manager
Allows you to create new paths. Select the path type you want to create and click "Create"

If you are creating paths from outside of the inspector (third party plugins), ensure all path nodes have a PathNode component attached to them.

PathNodes and saved in order of hierarchy index

All path nodes should be parented to a seperate GameObject with a PathData component attached.

Path nodes that aren't parented to a GameObject with a PathData component will be ignored when saving the map.

PathData objects have a custom inspector that will allow you to modify various elements of the path data.

PathNode objects have a custom inspector that will show you the index of the node

# Other Shit
The asset bundles will automatically unload and reload when any project assets are changed.

I have no intention of continuing development or providing support for this project. It was quickly chucked together as a favour for a friend.

Feel free to fork this repository and do with it as you wish.
