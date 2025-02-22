using UnityEngine;

public class OptionClick : MonoBehaviour
{
    [SerializeField] private Canvas targetCanvas; // 操作対象のCanvasをInspectorで指定
    private bool UISetflag = false; // 初期化

    void Start()
    {
        if (targetCanvas != null)
        {
            targetCanvas.enabled = false; // 初期状態で非表示
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
        //    Debug.LogError("Canvasが設定されていません！");
        //}
    }
}
