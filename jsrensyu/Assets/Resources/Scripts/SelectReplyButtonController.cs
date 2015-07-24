using UnityEngine;
using System.Collections;

public class SelectReplyButtonController : MonoBehaviour {

    public GameObject Parent = null;
    public GameObject MainPanel = null;

    private Vector3 _pv;

    public TimePauser Pauser = null;
    public UnityEngine.UI.ScrollRect[] srList = null;

	// Use this for initialization
	void Start () {
        _pv = Parent.transform.position;
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clicked()
    {
        Parent.transform.position = _pv;
        Pauser.Resume();
        for (int i = 0; i < srList.Length; i++)
        {
            srList[i].enabled = true;
        }
    }
}
