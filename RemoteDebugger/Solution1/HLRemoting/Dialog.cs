using UnityEngine;
using System.Collections;

/// <summary>
/// モーダル/モーダレスダイアログ、ゲーム内時間Pause状態の設定が行えるダイアログクラス
/// </summary>
public abstract class Dialog : MonoBehaviour
{
    /// <summary>
    /// ポーズ状態
    /// </summary>
    TimePauser _pauser = null;

    /// <summary>
    /// UI状態
    /// </summary>
    private bool _isUIEnable = true;

    /// <summary>
    /// ゲーム内時間Pause状態
    /// </summary>
    private bool _isGameEnable = true;

    /// <summary>
    /// モーダルダイアログ状態の取得または設定します
    /// </summary>
    /// <value></value>
    public bool IsUIEnable
    {
        get { return _isUIEnable; }
        set { _isUIEnable = value; }
    }

    /// <summary>
    /// 起動時のゲーム内時間Pause状態を取得または設定します
    /// </summary>
    /// <value></value>
    public bool IsGameEnable
    {
        get { return _isGameEnable; }
        set { _isGameEnable = value; }
    }

    /// <summary>
    /// Start
    /// </summary>
    void Start()
    {
        // ダイアログ開始
        _pauser = new TimePauser(gameObject);
        if (IsUIEnable != true)
        {
            _pauser.PauseUI();
        }
        if (IsGameEnable != true)
        {
            _pauser.PauseGame();
        }
    }

    /// <summary>
    /// OnDestory
    /// </summary>
    void OnDisable()
    {
        // ダイアログ終了
        _pauser.Resume();
    }

    /// <summary>
    /// 閉じるイベント
    /// </summary>
    public void OnCloseButton()
    {
        Destroy(gameObject);
    }
}