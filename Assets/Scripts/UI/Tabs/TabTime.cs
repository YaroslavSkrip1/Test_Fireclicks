using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Tabs
{
    public class TabTime : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private Button backButton;
        
        private StringBuilder _sb = new();
        private bool _running;
        private CancellationTokenSource _cts;

        [Inject] private TabsController _tabs;
        
        private void Start() => backButton.onClick.AddListener(BackToTabs);
        private void OnEnable()
        {
            _running = true;
            _cts = new();
            UpdateTimeLoop(_cts.Token).Forget();
        }

        private void OnDisable()
        {
            _running = false;
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private async UniTaskVoid UpdateTimeLoop(CancellationToken token)
        {
            while (_running && !token.IsCancellationRequested)
            {
                _sb.Clear();
                var now = DateTime.Now;
                _sb.Append(now.Hour.ToString("D2")).Append(":")
                    .Append(now.Minute.ToString("D2")).Append(":")
                    .Append(now.Second.ToString("D2")).Append(".")
                    .Append(now.Millisecond.ToString("D3"));

                timeText.SetText(_sb);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        public void BackToTabs() => _tabs.BackToMenu();
    }
}