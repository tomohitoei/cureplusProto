using UnityEngine;
using System.Collections;

public class ShowReplyButtonController : MonoBehaviour {

    public GameObject MainPanel = null;
    public GameObject ReplyPanel = null;


    private Vector3 _pv;
	// Use this for initialization
	void Start () {
        _pv = ReplyPanel.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clicked()
    {
        ReplyPanel.transform.position = new Vector3(500, _pv.y, _pv.z);
        MainPanel.SetActive(false);
    }
}
