using UnityEditor;
using UnityEngine;

public class AssetLoader : MonoBehaviour
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

    void Start()
    {
        string selectedAssetPath = EditorUtility.OpenFilePanel("Select obj model", "", "obj");

        Load(selectedAssetPath);
    }

    public void Load(string assetName)
    {
        LoaderModule.LoadAsset(assetName);
        LoaderModule.OnLoadCompleted += OnLoadCompleted;
    }


    void OnLoadCompleted(GameObject loadedAsset)
    {
        loadedAsset.transform.SetParent(transform);
        // To do
        Debug.Log("Complete load asset: " + loadedAsset.name);
    }
}
