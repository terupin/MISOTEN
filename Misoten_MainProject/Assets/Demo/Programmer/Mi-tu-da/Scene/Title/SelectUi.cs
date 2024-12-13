using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Test : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject OptionCanvas;
    public Button OptionButton;
    public Slider BGMSlider;
    public Slider SESlider;

    private EventSystem eventSystem;

    private void Start()
    {
        eventSystem = EventSystem.current;

        // OptionButtonにイベント追加
        OptionButton.onClick.AddListener(() =>
        {
            OptionCanvas.SetActive(true);

            // OptionCanvas内のSliderを選択状態に設定
            eventSystem.SetSelectedGameObject(BGMSlider.gameObject);
        });
    }

    private void Update()
    {
        // 現在選択されているオブジェクトを取得
        GameObject currentSelected = eventSystem.currentSelectedGameObject;

        if (currentSelected == BGMSlider.gameObject)
        {
            SetAlpha(BGMSlider, 1.0f);
            SetAlpha(SESlider, 0.0f);
        }
        else if (currentSelected == SESlider.gameObject)
        {
            SetAlpha(BGMSlider, 0.0f);
            SetAlpha(SESlider, 1.0f);
        }

        //Bボタンで戻る
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && OptionCanvas.activeSelf)
        {
            OptionCanvas.SetActive(false);

            // MainCanvas内のOptionButtonを選択状態に設定
            eventSystem.SetSelectedGameObject(OptionButton.gameObject);
        }
    }

    //スライダーの透明度を設定する
    private void SetAlpha(Slider slider, float alpha)
    {
        var colors = slider.GetComponentInChildren<Image>().color;
        colors.a = alpha;
        slider.GetComponentInChildren<Image>().color = colors;
    }
}
