using UnityEngine;
using System.Collections;

public class Opening : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.LoadLevel("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnApplicationQuit() { 
        Debug.Log("End");
        ScriptDebugger.DebuggerConsole.Instance().Close();        
    } 
}
