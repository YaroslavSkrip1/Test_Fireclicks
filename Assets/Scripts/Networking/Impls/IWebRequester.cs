using Cysharp.Threading.Tasks;

namespace Networking.Impls
{
    public interface IWebRequester
    {
        UniTask<string> Get(string url);
    }
}