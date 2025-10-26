using TMPro;
using UnityEngine;

namespace UI.Item
{
    public class ItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemText;

        public void SetIndex(int index) => itemText.SetText(index.ToString());
    }
}