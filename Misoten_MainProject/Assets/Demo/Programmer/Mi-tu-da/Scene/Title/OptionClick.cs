using UnityEngine;

public class OptionClick : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas; // ‘€ì‘ÎÛ‚ÌCanvas‚ğInspector‚Åw’è
    private bool UISetflag = false; // ‰Šú‰»

    void Start()
    {
        if (targetCanvas != null)
        {
            targetCanvas.enabled = false; // ‰Šúó‘Ô‚Å”ñ•\¦
        }
    }

    public void OnClick()
    {
        if (targetCanvas != null)
        {
            UISetflag = !UISetflag;
            targetCanvas.enabled = UISetflag;
        }
        else
        {
            Debug.LogError("Canvas‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
        }
    }
}
