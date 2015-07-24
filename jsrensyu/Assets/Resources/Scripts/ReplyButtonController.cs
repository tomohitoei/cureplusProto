using UnityEngine;
using System.Collections;

public class ReplyButtonController : MonoBehaviour {

    public string Title = string.Empty;
    public string Content = string.Empty;

    public UnityEngine.UI.Image Target = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponentInChildren<UnityEngine.UI.Text>().text = Title;
	}

    public void Clicked()
    {
        MailController mc = MailController.Instance;
        var s = mc.MakeImage(Content, 400);
        Target.sprite = s;
    }
}
