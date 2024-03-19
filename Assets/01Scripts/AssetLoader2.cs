using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class AssetLoader2 : MonoBehaviour
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

        await Load(selectedAssetName);
    }


    async Task Load(string assetName)
    {
        GameObject loadedAsset = await LoaderModule.LoadAssetAsync(assetName);
        loadedAsset.transform.SetParent(transform);
        // To do
        Debug.Log("Complete load asset: " + loadedAsset.name);
    }

}
