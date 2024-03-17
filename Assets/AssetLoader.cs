using UnityEditor;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
    [SerializeField]
    public LoaderModule LoaderModule;

    private void Start()
    {
        string selectedAssetPath = EditorUtility.OpenFilePanel("Select obj model", "", "obj");
        string assetName = GetRelativePath(selectedAssetPath);

        Load(assetName);
    }

    private string GetRelativePath(string absolutePath)
    {
        string projectPath = Application.dataPath;
        projectPath = projectPath.Substring(0, projectPath.Length - "Assets".Length); // Remove "Assets" from the path

        if (absolutePath.StartsWith(projectPath))
        {
            string relativePath = absolutePath.Substring(projectPath.Length);
            // Remove "Assets/Resources/" part
            string assetName = relativePath.Substring("Assets/Resources/".Length);

            return assetName;
        }
        else
        {
            Debug.LogError("Selected file is not within the project folder.");
            return null;
        }
    }

    public void Load(string assetName)
    {
        LoaderModule.LoadAsset(assetName);
        LoaderModule.OnLoadCompleted += OnLoadCompleted;
    }


    private void OnLoadCompleted(GameObject loadedAsset)
    {
        loadedAsset.transform.SetParent(transform);
        // To do
    }
}
