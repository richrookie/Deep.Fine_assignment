using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;

public class AssetLoader3 : MonoBehaviour
{

    public LoaderModule LoaderModule
    {
        get
        {
            LoaderModule loaderModule = FindFirstObjectByType<LoaderModule>();
            if (loaderModule == null)
            {
                GameObject ldr = new GameObject() { name = "@LoadModule" };
                ldr.AddComponent<LoaderModule>();

                return ldr.GetComponent<LoaderModule>();
            }

            return loaderModule;
        }
    }

    async void Start()
    {
        string selectedAssetName = EditorUtility.OpenFilePanel("Select obj model", "", "obj");

        List<string> objFileNames = new List<string>();

        for (int i = 1; i <= 20; i++)
        {
            objFileNames.Add(selectedAssetName);
        }

        await LoadAllAssetsAsync(objFileNames);
    }

    async Task LoadAllAssetsAsync(List<string> objFileNames)
    {
        List<Task> loadTasks = new List<Task>();

        foreach (string objFileName in objFileNames)
        {
            Task loadTask = LoadAssetAsync(objFileName);
            loadTasks.Add(loadTask);
        }

        await Task.WhenAll(loadTasks);
    }

    async Task LoadAssetAsync(string assetName)
    {
        try
        {
            GameObject loadedAsset = await LoaderModule.LoadAssetAsync(assetName);
            loadedAsset.transform.SetParent(transform);
            // To Do
            Debug.Log("Complete load asset: " + loadedAsset.name);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading asset {assetName}: {e.Message}");
        }
    }
}
