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

        // OptionButton�ɃC�x���g�ǉ�
        OptionButton.onClick.AddListener(() =>
        {
            OptionCanvas.SetActive(true);

            // OptionCanvas����Slider��I����Ԃɐݒ�
            eventSystem.SetSelectedGameObject(BGMSlider.gameObject);
        });
    }

    private void Update()
    {
        // ���ݑI������Ă���I�u�W�F�N�g���擾
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

        //B�{�^���Ŗ߂�
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && OptionCanvas.activeSelf)
        {
            OptionCanvas.SetActive(false);

            // MainCanvas����OptionButton��I����Ԃɐݒ�
            eventSystem.SetSelectedGameObject(OptionButton.gameObject);
        }
    }

    //�X���C�_�[�̓����x��ݒ肷��
    private void SetAlpha(Slider slider, float alpha)
    {
        var colors = slider.GetComponentInChildren<Image>().color;
        colors.a = alpha;
        slider.GetComponentInChildren<Image>().color = colors;
    }
}
