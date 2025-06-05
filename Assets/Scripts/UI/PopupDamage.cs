using UnityEngine;
using TMPro;

public class PopupDamage : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float fadeSpeed = 2f;
    public Vector3 floatOffset = new(0, 1f, 0);

    [SerializeField] TextMeshProUGUI damageTextMesh;
    private Color startColor;

    void Awake()
    {
        damageTextMesh = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        startColor = damageTextMesh.color;
    }

    void Update()
    {
        transform.position += floatSpeed * Time.deltaTime * floatOffset;

        Color c = damageTextMesh.color;
        c.a -= fadeSpeed * Time.deltaTime;
        damageTextMesh.color = c;

        if (c.a <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetText(string text)
    {
        damageTextMesh.SetText(text);
        damageTextMesh.color = startColor;
    }

    public void ChangeColor(Color color)
    {
        damageTextMesh.color = color;
    }
}
