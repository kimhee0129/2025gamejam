// JsonUtilityWrapper.cs
using System.Collections.Generic;
using MiniJSON; // 깃허브 다운로드하기!

public static class JsonUtilityWrapper
{
    public static Dictionary<string, object> Deserialize(string json)
    {
        return Json.Deserialize(json) as Dictionary<string, object>;
    }
}