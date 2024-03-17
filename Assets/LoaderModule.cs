using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

public class LoaderModule : MonoBehaviour
{
    #region 1.
    public event Action<GameObject> OnLoadCompleted;

    public void LoadAsset(string assetName)
    {
        StartCoroutine(LoadAssetCoroutine(assetName));
    }

    private IEnumerator LoadAssetCoroutine(string assetName)
    {
        string assetNameWithoutExtension = Path.GetFileNameWithoutExtension(assetName);

        ResourceRequest request = Resources.LoadAsync(assetNameWithoutExtension, typeof(GameObject));

        yield return request;

        if (request.asset != null)
        {
            GameObject loadedObject = Instantiate((GameObject)request.asset);

            OnLoadCompleted?.Invoke(loadedObject);
        }
        else
        {
            Debug.LogError("Failed to load asset: " + assetName);
        }
    }
    #endregion


    #region 2. 3.
    public static async Task<GameObject> LoadAssetAsync(string assetName)
    {
        string assetNameWithoutExtension = Path.GetFileNameWithoutExtension(assetName);

        ResourceRequest request = Resources.LoadAsync<GameObject>(assetNameWithoutExtension);

        TaskCompletionSource<GameObject> tcs = new TaskCompletionSource<GameObject>();

        request.completed += operation =>
        {
            if (request.asset != null)
            {
                GameObject loadedObject = Instantiate((GameObject)request.asset);

                tcs.SetResult(loadedObject);
            }
            else
            {
                tcs.SetException(new Exception("Failed to load asset: " + assetName));
            }
        };

        return await tcs.Task;
    }
    #endregion
}