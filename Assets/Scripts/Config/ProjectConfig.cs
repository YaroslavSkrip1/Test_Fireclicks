using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "ProjectConfig", menuName = "Config/ProjectConfig")]
    public class ProjectConfig : ScriptableObject
    {
        public string ApiUrl = "https://example.com";
        public string[] TabKeys = { "TabList", "TabTime", "TabAnimated", "TabRequest" };
    }
}
