using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Matsunaga_Status_E : MonoBehaviour
{
    static public float MaxHP = 10000;
    static public float NowHP = MaxHP;

    private string mySceneName; // ���g���z�u����Ă���V�[����

    // Start is called before the first frame update
    void Start()
    {
        NowHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        // ���݂̃V�[�������擾
        mySceneName = gameObject.scene.name;

        // �V�[�����[�h�C�x���g�ɓo�^
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // �V�[�����[�h�C�x���g������
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �V�[�����ǂݍ��܂ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mySceneName)
        {
            Debug.Log($"�V�[�� '{mySceneName}' ���ǂݍ��܂�܂����B�I�u�W�F�N�g: {gameObject.name}");
            HandleSceneLoaded(); // �V�[���ǂݍ��ݎ��̏���
        }
    }

    // �V�[�����ǂݍ��܂ꂽ�Ƃ��̏���
    private void HandleSceneLoaded()
    {
        //HP�̍ő�l��
        NowHP = MaxHP;

        // �K�v�ȏ������L�q
        Debug.Log($"'{NowHP}' �����������s���܂��B");
    }
}
