// CL.cs
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CL : MonoBehaviour
{
    private static CL _instance;
    public static CL Instance => _instance;

    private Dictionary<string, object> config = new Dictionary<string, object>();

    public static T Get<T>(string key)
    {
        if (Instance.config.TryGetValue(key, out var value))
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }

        Debug.LogError($"Config key '{key}' not found.");
        return default;
    }

    


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        LoadConfig();
    }

    private void LoadConfig()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "constants.json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            config = JsonUtilityWrapper.Deserialize(json);
        }
        else
        {
            Debug.LogError("constants.json 파일을 찾을 수 없습니다.");
        }
    }
}