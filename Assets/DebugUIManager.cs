using TMPro;
using UnityEngine;

public class DebugUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText; // Reference to the UI text element

    /// <summary>
    /// Updates the debug info text on the UI.
    /// </summary>
    /// <param name="info">The string to display.</param>
    public void UpdateDebugInfo(string info)
    {
        if (debugText != null)
        {
            debugText.text = info;
        }
    }
}