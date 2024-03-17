using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class AssetLoaderAsync : MonoBehaviour
{
    [field: SerializeField]
    public LoaderModule LoaderModule { get; set; }

    private async void Start()
    {
        string selectedAssetName = EditorUtility.OpenFilePanel("Select obj model", "", "obj");
        await Load(selectedAssetName);
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


    public async Task Load(string assetName)
    {
        GameObject loadedAsset = await LoaderModule.LoadAssetAsync(assetName);
        loadedAsset.transform.SetParent(transform);
    }
}
