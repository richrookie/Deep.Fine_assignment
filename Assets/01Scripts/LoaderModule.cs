using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.Networking;


#region 객체를 한 번에 Instantiate 하고 싶은 경우
// === 1. === //
// IEnumerator LoadAssetCoroutine(string assetName)
// {
//     // 경로에 위치된 파일네임에서 확장자명 제외 네임 반환 -
//     string assetNameWithoutExtension = Path.GetFileNameWithoutExtension(assetName);

//     // 비동기 작업을 위한 Resources.LoadAsync() 사용 (동적 로딩)
//     // 때문에 메인 스레드가 차단되지 않고 계속 실행될 수 있음
//     ResourceRequest request = Resources.LoadAsync(assetNameWithoutExtension, typeof(GameObject));

//     // 로딩이 완료될 때까지 대기 = 코루틴 중단점
//     yield return request;

//     if (request.asset != null)
//     {
//         GameObject loadedObject = Instantiate((GameObject)request.asset);

//         OnLoadCompleted?.Invoke(loadedObject);
//     }
//     else
//     {
//         Debug.LogError("Failed to load asset: " + assetName);
//     }
// }


//=== 2. 3. === //
// <summary>
// 표준 C# 비동기 방식
// </summary>
// <param name = "assetName" > 로드할 assetName</param>
// <returns></returns>
// public async Task<GameObject> LoadAssetAsync(string assetName)
// {
//     string assetNameWithoutExtension = Path.GetFileNameWithoutExtension(assetName);

//     ResourceRequest request = Resources.LoadAsync<GameObject>(assetNameWithoutExtension);

//     TaskCompletionSource<GameObject> tcs = new TaskCompletionSource<GameObject>();

//     request.completed += operation =>
//     {
//         if (request.asset != null)
//         {
//             GameObject loadedObject = Instantiate((GameObject)request.asset);

//             tcs.SetResult(loadedObject);
//         }
//         else
//         {
//             tcs.SetException(new Exception("Failed to load asset: " + assetName));
//         }
//     };

//     return await tcs.Task;
// }
#endregion


public class LoaderModule : MonoBehaviour
{
    GameObject CubePrefab = null;

    void Awake()
    {
        CubePrefab = Resources.Load("CubePrefab") as GameObject;
    }


    #region 1.
    public event Action<GameObject> OnLoadCompleted;

    public void LoadAsset(string assetName)
    {
        // 코루틴을 사용한 리소스 비동기적 로딩
        StartCoroutine(LoadAssetCoroutine(assetName));
    }

    /// <summary>
    /// Unity 기능을 활용한 비동기 방식
    /// </summary>
    /// <param name="assetName">로드할 assetName</param>
    /// <returns></returns>
    IEnumerator LoadAssetCoroutine(string assetName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, assetName);

        using (UnityWebRequest uwr = UnityWebRequest.Get(assetName))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Downloaded data: " + uwr.downloadHandler.text);
                yield return LoadObjModel(uwr.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Failed to load asset: " + assetName + ", Error: " + uwr.error);
            }
        }
    }

    IEnumerator LoadObjModel(string objData)
    {
        GameObject loadedObject = new GameObject("LoadedObject");
        string[] lines = objData.Split('\n');
        int batchSize = 1000;
        int count = 0;

        foreach (string line in lines)
        {
            if (line.StartsWith("v "))
            {
                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 4)
                {
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[2]);
                    float z = float.Parse(parts[3]);
                    Vector3 vertex = new Vector3(x, y, z);

                    GameObject cube = Instantiate(CubePrefab);
                    cube.transform.SetParent(loadedObject.transform);
                    cube.transform.localPosition = vertex;

                    count++;
                    if (count >= batchSize)
                    {
                        yield return new WaitForSeconds(.01f);
                        count = 0;
                    }
                }
            }
        }

        yield return new WaitForSeconds(.01f);

        OnLoadCompleted?.Invoke(loadedObject);
    }
    #endregion


    #region 2. 3.
    [SerializeField]
    int _batchSize = 1000;

    public async Task<GameObject> LoadAssetAsync(string assetName)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, assetName);

        UnityWebRequest uwr = UnityWebRequest.Get(filePath);

        uwr.SendWebRequest();

        while (!uwr.isDone)
        {
            await Task.Delay(10);
        }

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to load asset: " + assetName + ", Error: " + uwr.error);
            return null;
        }

        Debug.Log("Downloaded data: " + uwr.downloadHandler.text);

        return await LoadObjModelAsync(uwr.downloadHandler.text);
    }

    int order = 0;
    async Task<GameObject> LoadObjModelAsync(string objData)
    {
        GameObject loadedObject = new GameObject("LoadedObject");
        loadedObject.transform.localPosition = new Vector3(0, 0, order * 200);
        order++;

        string[] lines = objData.Split('\n');
        int batchSize = _batchSize;
        int count = 0;

        foreach (string line in lines)
        {
            if (line.StartsWith("v "))
            {
                string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 4)
                {
                    float x = float.Parse(parts[1]);
                    float y = float.Parse(parts[2]);
                    float z = float.Parse(parts[3]);
                    Vector3 vertex = new Vector3(x, y, z);

                    GameObject cube = Instantiate(CubePrefab);
                    cube.transform.SetParent(loadedObject.transform);
                    cube.transform.localPosition = vertex;
                }
            }

            count++;
            if (count >= batchSize)
            {
                await Task.Delay(10);
                count = 0;
            }
        }

        return loadedObject;
    }
    #endregion
}

