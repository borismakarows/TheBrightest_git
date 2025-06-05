using UnityEngine;
using TMPro;

public class TurnBanner : MonoBehaviour
{
    public TextMeshProUGUI turnText;

    void Start()
    {
        turnText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetTurnIndicator(string text)
    {
        turnText.SetText(text + "'s Turn");
    }
}
