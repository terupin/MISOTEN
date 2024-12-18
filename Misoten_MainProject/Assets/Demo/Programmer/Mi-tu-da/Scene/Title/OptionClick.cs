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
        if(UnityEngine.Input.GetKeyDown("joystick button 0"))
        {
            UISetflag = true;
            targetCanvas.enabled = UISetflag;
        }
        else if (UnityEngine.Input.GetKeyDown("joystick button 1"))
        {
            UISetflag = false;
            targetCanvas.enabled = UISetflag;
        }

        //if (targetCanvas != null)
        //{
        //    UISetflag = !UISetflag;
        //    targetCanvas.enabled = UISetflag;

        //}
        //else
        //{
        //    Debug.LogError("Canvas���ݒ肳��Ă��܂���I");
        //}
    }
}
