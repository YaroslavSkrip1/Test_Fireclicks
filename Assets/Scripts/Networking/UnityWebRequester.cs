using System;
using Cysharp.Threading.Tasks;
using Networking.Impls;
using UnityEngine.Networking;

namespace Networking
{
    public class UnityWebRequester : IWebRequester
    {
        public async UniTask<string> Get(string url, int timeoutSeconds = 10)
        {
            using var req = UnityWebRequest.Get(url);
            req.timeout = timeoutSeconds;

            await req.SendWebRequest().ToUniTask();

            if (req.result != UnityWebRequest.Result.Success)
                throw new Exception($"HTTP Error: {req.responseCode} ({req.error})");

            return req.downloadHandler.text;
        }
    }
}