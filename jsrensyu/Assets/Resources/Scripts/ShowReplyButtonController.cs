using UnityEngine;
using System.Collections;

public class ShowReplyButtonController : MonoBehaviour {

    public GameObject MainPanel = null;
    public GameObject ReplyPanel = null;

    public UnityEngine.Camera C = null;
    public UnityEngine.UI.Image SS = null;

    public SelectReplyButtonController src = null;
    private Vector3 _pv;

    public UnityEngine.UI.ScrollRect[] srList = null;

    public MailItemButtonController CurrentMail = null;

    private TimePauser _pauser = null;
	// Use this for initialization
	void Start () {
        _pauser = new TimePauser(ReplyPanel); // MainPanelを止めたいのにMainPanelを渡すとReplyPanelが止まってしまう
        _pv = ReplyPanel.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clicked()
    {
        ReplyPanel.transform.position = new Vector3(500, _pv.y, _pv.z);

        src.Pauser = _pauser;
        src.srList = srList;
        _pauser.PauseUI();
        _pauser.PauseGame();
        for (int i = 0; i < srList.Length; i++)
        {
            srList[i].enabled = false;
        }
    }
}
