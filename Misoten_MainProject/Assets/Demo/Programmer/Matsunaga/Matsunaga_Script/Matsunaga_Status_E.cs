using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Matsunaga_Status_E : MonoBehaviour
{
    static public float MaxHP = 10000;
    static public float NowHP = MaxHP;

    private string mySceneName; // 自身が配置されているシーン名

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
        // 現在のシーン名を取得
        mySceneName = gameObject.scene.name;

        // シーンロードイベントに登録
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        // シーンロードイベントを解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンが読み込まれたときに呼び出されるメソッド
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mySceneName)
        {
            Debug.Log($"シーン '{mySceneName}' が読み込まれました。オブジェクト: {gameObject.name}");
            HandleSceneLoaded(); // シーン読み込み時の処理
        }
    }

    // シーンが読み込まれたときの処理
    private void HandleSceneLoaded()
    {
        //HPの最大値化
        NowHP = MaxHP;

        // 必要な処理を記述
        Debug.Log($"'{NowHP}' 初期化を実行します。");
    }
}
