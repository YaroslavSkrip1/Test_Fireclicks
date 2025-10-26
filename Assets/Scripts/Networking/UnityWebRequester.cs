using Cysharp.Threading.Tasks;
using Networking.Impls;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class UnityWebRequester : IWebRequester
    {
        public async UniTask<string> Get(string url)
        {
            using var req = UnityWebRequest.Get(url);
            await req.SendWebRequest().ToUniTask();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Request failed: {req.error}");
                return string.Empty;
            }
            return req.downloadHandler.text;
        }
    }
}