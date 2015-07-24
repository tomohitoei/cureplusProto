using UnityEngine;
using System.Collections;

/// <summary>
/// モーダルダイアログ
/// 自分自身以外のダイアログ入力禁止を行います
/// </summary>
public class NormalModalDialog : Dialog
{
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        IsUIEnable = false;
        IsGameEnable = true;
    }
}
