
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
 
/// <summary>
/// ポーズ機能を提供します
/// </summary>
public class TimePauser 
{
  /// <summary>
  /// ポーズ状態にしたUIオブジェクト配列
  /// </summary>
  private List<UnityEngine.UI.Selectable> _pause_selectables = new List<UnityEngine.UI.Selectable> ();
 
  /// <summary>
  /// ポーズ状態にしたオブジェクト配列
  /// </summary>
  private List<Behaviour> _pause_objects = new List<Behaviour> ();
 
  /// <summary>
  /// ポーズ状態にしたRigidbody配列
  /// </summary>
  private List<Rigidbody> _RigidBodies = new List<Rigidbody>();
  private List<Vector3> _RigidBodyVelocities = new List<Vector3>();
  private List<Vector3> _RigidBodyAngularVelocities = new List<Vector3>();
  
  /// <summary>
  /// ポーズ状態にしたRigidbody2D配列
  /// </summary>
  private List<Rigidbody2D> _RigidBodies2D = new List<Rigidbody2D>();
  private List<Vector2> _RigidBodyVelocities2D = new List<Vector2>();
  private List<float> _RigidBodyAngularVelocities2D = new List<float>();
 
  /// <summary>
  /// 無視するオブジェクト
  /// </summary>
  private GameObject _excludeObject = null;
 
  /// <summary>
  /// 
  /// </summary>
  public TimePauser()
  {
  }
 
  /// <summary>
  /// コンストラクタ
  /// </summary>
  /// <param name="excludeObject">無視するオブジェクト</param>
  public TimePauser( GameObject excludeObject )
  {
    this._excludeObject = excludeObject;
  }
 
  /// <summary>
  /// UI関係のオブジェクトを無効にします
  /// </summary>
  public void PauseUI()
  {
    Pause (UIObject ());
  }
 
  /// <summary>
  /// Game関係のオブジェクトを無効にします
  /// </summary>
  public void PauseGame()
  {
    Pause (GmObject ());
  }
 
  /// <summary>
  /// ポーズにしたオブジェクトを元に戻します
  /// </summary>
  public void Resume()
  {
    // UnityEngine.UI.Selectableを有効
    _pause_selectables.ForEach (o => o.interactable = true);
 
    // Behaviourを有効
    _pause_objects.ForEach (o => o.enabled = true);
 
    // Rigidbodyを有効
    for( var i=0; i<_RigidBodies.Count; i++ ) {
      _RigidBodies[i].WakeUp();
      _RigidBodies[i].velocity = _RigidBodyVelocities[i];
      _RigidBodies[i].angularVelocity = _RigidBodyAngularVelocities[i];
    }
    
    // Rigidbody2Dを有効
    for( var i=0; i<_RigidBodies2D.Count; i++ ) {
      _RigidBodies2D[i].WakeUp();
      _RigidBodies2D[i].velocity = _RigidBodyVelocities2D[i];
      _RigidBodies2D[i].angularVelocity = _RigidBodyAngularVelocities2D[i];
    }
  }
 
  /// <summary>
  /// 指定オブジェクトを無効化します
  /// </summary>
  /// <param name="objs">Objects.</param>
  private void Pause( GameObject[] objs )
  {
    foreach( var obj in objs ) {
      // 無効にする対象かどうか
      if (IsExclude (obj)) {
        continue;
      }
 
      // UnityEngine.UI.Selectableを無効
      var pauseSelectable = Array.FindAll(obj.GetComponentsInChildren<UnityEngine.UI.Selectable>(), (cmp) => { return cmp.interactable; });
      _pause_selectables.AddRange( pauseSelectable );
 
      // Behaviourを無効
      var pauseBehavs = Array.FindAll(obj.GetComponentsInChildren<Behaviour>(), (cmp) => { return !(cmp is UnityEngine.EventSystems.UIBehaviour) && cmp.enabled; });
      _pause_objects.AddRange( pauseBehavs );
 
      // Rigidbodyを無効
      _RigidBodies.AddRange( Array.FindAll(obj.GetComponentsInChildren<Rigidbody>(), (cmp) => { return !cmp.IsSleeping(); }) );
      _RigidBodyVelocities = new List<Vector3>( _RigidBodies.Count );
      _RigidBodyAngularVelocities = new List<Vector3>( _RigidBodies.Count );
      for ( var i = 0 ; i < _RigidBodies.Count ; ++i ) {
        _RigidBodyVelocities[i] = _RigidBodies[i].velocity;
        _RigidBodyAngularVelocities[i] = _RigidBodies[i].angularVelocity;
        _RigidBodies[i].Sleep();
      }
 
      // Rigidbody2Dを無効
      _RigidBodies2D.AddRange( Array.FindAll(obj.GetComponentsInChildren<Rigidbody2D>(), (cmp) => { return !cmp.IsSleeping(); }) );
      _RigidBodyVelocities2D = new List<Vector2>( _RigidBodies2D.Count );
      _RigidBodyAngularVelocities2D = new List<float>( _RigidBodies2D.Count );
      for ( var i = 0 ; i < _RigidBodies2D.Count ; ++i ) {
        _RigidBodyVelocities2D[i] = _RigidBodies2D[i].velocity;
        _RigidBodyAngularVelocities2D[i] = _RigidBodies2D[i].angularVelocity;
        _RigidBodies2D[i].Sleep();
      }
    }
 
    // 無効
    _pause_selectables.ForEach( o => o.interactable = false );
    _pause_objects.ForEach (o => o.enabled = false);
  }
 
  /// <summary>
  /// 例外的に処理をしないオブジェクトかどうか取得します
  /// </summary>
  /// <returns>無視する場合はtureを返します</returns>
  /// <param name="obj"></param>
  private bool IsExclude( GameObject obj )
  {
    // 外部指定の無視オブジェクト
    if (this._excludeObject == obj) {
      return true;
    }
    // カメラ
    if (obj.GetComponent<Camera> () != null) {
      return true;
    }
    // ライト
    if (obj.GetComponent<Light> () != null) {
      return true;
    }
    // イベントシステム
    if (obj.GetComponent<UnityEngine.EventSystems.EventSystem> () != null) {
      return true;
    }
 
    // MonoBehaviourのみで構成されたGameObject
    // どうやって判定するの？
 
    return false;
  }
 
  /// <summary>
  /// ルートオブジェクトを取得します
  /// </summary>
  private static GameObject[] Root()
  {
    return Array.FindAll( GameObject.FindObjectsOfType<GameObject> (), (item) => item.transform.parent == null);
  }
 
  /// <summary>
  /// UI関係のGameObjectを取得します
  /// </summary>
  /// <returns>The object.</returns>
  private static GameObject[] UIObject()
  {
    var roots = Root ();
    var canvas = Array.FindAll (roots, obj => obj.GetComponent<Canvas> () != null);
 
    // Canvas自体は無効にしたくないので、Canvasの子オブジェクトを対象にする
    var uiObjs = new List<GameObject> ();
    for (int i=0; i<canvas.Length; i++) {
      for( int n=0; n<canvas[i].transform.childCount; n++ ) {
        uiObjs.Add ( canvas[i].transform.GetChild( n ).gameObject );
      }
    }
    return uiObjs.ToArray();
  }
 
  /// <summary>
  /// Game関係のGameObjectを取得します
  /// </summary>
  /// <returns>The object.</returns>
  private static GameObject[] GmObject()
  {
    var roots = Root ();
    return Array.FindAll (roots, obj => obj.GetComponent<Canvas> () == null);
  }
}