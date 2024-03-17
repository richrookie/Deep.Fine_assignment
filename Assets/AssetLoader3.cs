using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;

public class AssetLoader3 : MonoBehaviour
{
    [SerializeField]
    public LoaderModule LoaderModule;

    private async void Start()
    {
        string selectedAssetName = EditorUtility.OpenFilePanel("Select obj model", "", "obj");

        List<string> objFileNames = new List<string>();

        for (int i = 1; i <= 20; i++)
        {
            objFileNames.Add(selectedAssetName);
        }

        await LoadAllAssetsAsync(objFileNames);
    }

    private async Task LoadAllAssetsAsync(List<string> objFileNames)
    {
        List<Task> loadTasks = new List<Task>();

        foreach (string objFileName in objFileNames)
        {
            Task loadTask = LoadAssetAsync(objFileName);
            loadTasks.Add(loadTask);
        }

        await Task.WhenAll(loadTasks);
    }

    private async Task LoadAssetAsync(string assetName)
    {
        try
        {
            GameObject loadedAsset = await LoaderModule.LoadAssetAsync(assetName);
            loadedAsset.transform.SetParent(transform);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading asset {assetName}: {e.Message}");
        }
    }
}
