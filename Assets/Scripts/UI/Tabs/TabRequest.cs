using Config;
using Networking.Impls;
using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace UI.Tabs
{
    public class TabRequest : MonoBehaviour
    {
        [SerializeField] private TMP_Text responseText;
        [SerializeField] private Button backButton;

        private IWebRequester _requester;
        private ProjectConfig _config;
        private TabsController _tabs;

        [Inject]
        public void Construct(IWebRequester requester, ProjectConfig config, TabsController tabs)
        {
            _requester = requester;
            _config = config;
            _tabs = tabs;
        }

        private async void Start()
        {
            backButton.onClick.AddListener(BackToTabs);
            responseText.SetText("Loading...");

            try
            {
                var url = _config.ApiUrl;
                var result = await _requester.Get(url);

                if (string.IsNullOrEmpty(result))
                {
                    responseText.SetText("Empty response");
                    return;
                }

                var formatted = FormatResponse(result);
                responseText.SetText(formatted);
            }
            catch (System.Exception e)
            {
                responseText.SetText($"Request failed:\n{e.Message}");
            }
        }

        private string FormatResponse(string input)
        {
            input = input.Trim();

            if ((input.StartsWith("{") && input.EndsWith("}")) ||
                (input.StartsWith("[") && input.EndsWith("]")))
                return BeautifyJson(input);

            if (input.Contains("<html"))
                return ExtractHtmlText(input);

            return input;
        }

        private string BeautifyJson(string json)
        {
            json = Regex.Replace(json, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
            json = Regex.Replace(json, "([{\\[])", "$1\n");
            json = Regex.Replace(json, "(,)", "$1\n");
            json = Regex.Replace(json, "([}\\]])", "\n$1");

            var lines = json.Split('\n');
            int indent = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (line.StartsWith("}") || line.StartsWith("]"))
                    indent--;
                lines[i] = new string(' ', indent * 4) + line;
                if (line.EndsWith("{") || line.EndsWith("["))
                    indent++;
            }

            return string.Join("\n", lines);
        }

        private string ExtractHtmlText(string html)
        {
            var body = Regex.Match(html, "<body.*?>(.*?)</body>", RegexOptions.Singleline).Groups[1].Value;
            if (string.IsNullOrEmpty(body))
                body = html;

            body = Regex.Replace(body, "<.*?>", string.Empty);
            body = Regex.Replace(body, @"\s+", " ").Trim();

            return body;
        }

        public void BackToTabs() => _tabs.BackToMenu();
    }
}