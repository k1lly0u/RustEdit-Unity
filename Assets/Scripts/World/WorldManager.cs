using System.Collections;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class WorldManager : MonoBehaviour
{
    public string bundlePath = string.Empty;

    public string mapPath = string.Empty;

    private World.Data world;

    private TerrainManager terrainManager;

    private PrefabManager prefabManager;

    private PathManager pathManager;

    public static WorldManager Instance { get; private set; }

    public void Start()
    {
        Instance = this;

        if (string.IsNullOrEmpty(bundlePath))
        {
            string assetPath = PlayerPrefs.GetString("RustInstallPath");
            EditorCoroutineUtility.StartCoroutine(Initialize(assetPath), this);
        }
    }

    public IEnumerator Initialize(string assetPath)
    {
        ActionProgressBar.UpdateProgress("Loading Asset Bundles...", 0f);

        yield return null;

        if (!string.IsNullOrEmpty(assetPath))
        {
            Debug.Log("Loading Bundles...");

            FileSystem.Backend = new AssetBundleBackend(assetPath);
            if (!FileSystem.Backend.isError)
            {
                bundlePath = assetPath;

                GameManifest.Load();
            }
            else Debug.Log("Error Loading Bundles: " + FileSystem.Backend.loadingError);
        }

        terrainManager = gameObject.GetOrAddComponent<TerrainManager>();
        prefabManager = gameObject.GetOrAddComponent<PrefabManager>();
        pathManager = gameObject.GetOrAddComponent<PathManager>();

        ActionProgressBar.Close();
        Debug.Log("Ready!");
    }

    public IEnumerator Cleanup()
    {
        ActionProgressBar.UpdateProgress("Cleaning Map Contents", 0f);
        yield return null;
        yield return null;

        if (terrainManager)
        {
            ActionProgressBar.UpdateProgress("Destroying Terrain", 0f);
            yield return null;
            yield return null;
            terrainManager.Cleanup();            
        }

        if (prefabManager)
        {
            ActionProgressBar.UpdateProgress("Destroying Prefabs", 0.33f);
            yield return null;
            yield return null;
            prefabManager.Cleanup();
            
        }

        if (pathManager)
        {
            ActionProgressBar.UpdateProgress("Destroying Paths", 0.66f);
            yield return null;
            yield return null;
            pathManager.Cleanup();
        }

        ActionProgressBar.Close();
    }

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OnScriptsReloaded()
    {
        if (WorldManager.Instance == null)
        {
            WorldManager.Instance = GameObject.FindObjectOfType<WorldManager>();
        }

        if (WorldManager.Instance != null && !string.IsNullOrEmpty(PlayerPrefs.GetString("RustInstallPath")))
            WorldManager.Instance.ValidateFileSystem();

        ActionProgressBar.Close();
    }

    private void ValidateFileSystem()
    {
        if (FileSystem.Backend == null)
        {
            EditorCoroutineUtility.StartCoroutine(Initialize(PlayerPrefs.GetString("RustInstallPath")), this);
        }
    }

    public void Create(int size, int splat, int biome)
    {
        ValidateFileSystem();
        EditorCoroutineUtility.StartCoroutine(CreateNew(size, splat, biome), this);
    }

    public IEnumerator CreateNew(int size, int splat, int biome)
    {       
        yield return EditorCoroutineUtility.StartCoroutine(Cleanup(), this);

        ActionProgressBar.UpdateProgress("Creating New World Data...", 0f);
        yield return null;
        yield return null;

        world = World.NewDataFromSize(size, splat, biome);

        EditorCoroutineUtility.StartCoroutine(LoadMap(world), this);

        ActionProgressBar.Close();

        Debug.Log("World Loaded!");
    }

    #region Loading
    public void Load(string filename)
    {
        ValidateFileSystem();

        EditorCoroutineUtility.StartCoroutine(LoadMap(filename), this);
    }

    private IEnumerator LoadMap(string filename)
    {      
        yield return EditorCoroutineUtility.StartCoroutine(Cleanup(), this);

        WorldSerialization blob = new WorldSerialization();

        ActionProgressBar.UpdateProgress("Loading Map...", 0f);
        yield return null;
        yield return null;

        blob.Load(filename);

        world = World.WorldToTerrain(blob);

        yield return EditorCoroutineUtility.StartCoroutine(LoadMap(world), this);
       
        ActionProgressBar.Close();

        Debug.Log("World Loaded!");
    }

    private IEnumerator LoadMap(World.Data world)
    {
        yield return EditorCoroutineUtility.StartCoroutine(terrainManager.Load(world), this);
        yield return EditorCoroutineUtility.StartCoroutine(prefabManager.Load(world), this);
        yield return EditorCoroutineUtility.StartCoroutine(pathManager.Load(world), this);
    }
    #endregion

    #region Saving
    public void Save(string filename)
    {
        if (!TerrainManager.HasValidTerrain())
        {
            EditorUtility.DisplayDialog("Error", "You must create or load a map first!", "OK");
            return;
        }

        EditorCoroutineUtility.StartCoroutine(SaveMap(filename), this);
    }

    private IEnumerator SaveMap(string filename)
    {       
        WorldSerialization blob = new WorldSerialization();

        ActionProgressBar.UpdateProgress("Saving Terrain", 0f);
        yield return null;
        yield return null;
        terrainManager.Save(ref blob);

        ActionProgressBar.UpdateProgress("Saving Prefabs", 0.33f);
        yield return null;
        yield return null;
        prefabManager.Save(ref blob);

        ActionProgressBar.UpdateProgress("Saving Paths", 0.66f);
        yield return null;
        yield return null;
        pathManager.Save(ref blob);

        ActionProgressBar.UpdateProgress("Saving File", 0.95f);
        yield return null;
        yield return null;
        blob.Save(filename);

        ActionProgressBar.Close();
    }
    #endregion
}
