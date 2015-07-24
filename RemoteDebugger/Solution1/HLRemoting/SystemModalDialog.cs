using UnityEngine;
using System.Collections;

/// <summary>
/// システムモーダルダイアログ
/// 自分自身以外のダイアログ入力禁止 + ゲーム内時間の停止を行います
/// </summary>
public class SystemModalDialog : Dialog
{
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        IsUIEnable = false;
        IsGameEnable = false;
    }
}
