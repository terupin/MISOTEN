using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Matsunaga_HitBoxEM : MonoBehaviour
{
    [SerializeField, Header("����")]
    public GameObject WeponPoint;
    [SerializeField, Header("�����{")]
    public GameObject WeponRoot;
    private GameObject Clone_Effect;//�G�t�F�N�g�̃N���[��

    [SerializeField, Header("�v���C���[���f��")]
    public GameObject Player_Model;

    [SerializeField, Header("�G���f��")]
    public GameObject Enemy_Model;

    private bool P_G_flg ;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        gameObject.transform.position = WeponPoint.transform.position;
        gameObject.transform.rotation = WeponPoint.transform.rotation;
    }

}
