using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    // �ő�HP�ƌ��݂�HP�B
    private int MaxHp = Kato_Status_E.MaxHP;
    private float CurrentHp;
    private float TargetHp; // �_���[�W��̖ڕWHP
    public float DamageUiSpeed = 0.1f; // �_���[�W������X�s�[�h
    public Slider slider;

    void Start()
    {
        MaxHp = Kato_Status_E.MaxHP;
        slider.minValue = 0;
        slider.maxValue = MaxHp;
        slider.value = MaxHp;

        // ���݂�HP���ő�HP�Ɠ����ɁB
        CurrentHp = MaxHp;
        TargetHp = MaxHp;
    }

    private void Update()
    {
        TargetHp = Kato_Status_E.NowHP;

        Debug.Log(TargetHp);
        //slider.value = (float)TargetHp;

        // ���X��CurrentHp��TargetHp�Ɍ������Č���
        if (CurrentHp > TargetHp)
        {
            CurrentHp -= DamageUiSpeed * Time.deltaTime;
            CurrentHp = Mathf.Max(CurrentHp, TargetHp); // TargetHp�ȉ��ɂ͂Ȃ�Ȃ�
            slider.value = CurrentHp;
        }

        // D�L�[�������ꂽ�Ƃ��Ƀ_���[�W��^����
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Kato_Status_E.NowHP = Kato_Status_E.NowHP- 1000;
            //ApplyDamage(1000); // 1�_���[�W��^����
        }

        if(CurrentHp <= 0)
        {
            slider.gameObject.SetActive(false);
        }
    }

    // �_���[�W��K�p����
    private void ApplyDamage(int damage)
    {
        TargetHp = Mathf.Max(TargetHp - damage, 0); // TargetHp������
    }
}
