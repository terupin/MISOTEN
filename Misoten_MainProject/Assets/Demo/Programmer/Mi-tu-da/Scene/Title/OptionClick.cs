using UnityEngine;

public class OptionClick : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas; // ����Ώۂ�Canvas��Inspector�Ŏw��
    private bool UISetflag = false; // ������

    void Start()
    {
        if (targetCanvas != null)
        {
            targetCanvas.enabled = false; // ������ԂŔ�\��
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
            Debug.LogError("Canvas���ݒ肳��Ă��܂���I");
        }
    }
}
