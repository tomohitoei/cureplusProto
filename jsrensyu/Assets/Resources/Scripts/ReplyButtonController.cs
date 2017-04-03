using UnityEngine;
using System.Collections;

public class ReplyButtonController : MonoBehaviour {

    public string Title = string.Empty;
    public string Content = string.Empty;

    public UnityEngine.UI.Image Target = null;
    public UnityEngine.UI.Text TargetText = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        GetComponentInChildren<UnityEngine.UI.Text>().text = Title;
	} 

    public void Clicked()
    {
        //MailController mc = MailController.Instance;
        //var s = mc.MakeImage(Content, 400);
        //Target.sprite = s;
        //Target.GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = s.texture.height * 268 / s.texture.width;
        TargetText.text = Content;
        var lc = 0;
        using (System.IO.StringReader sr = new System.IO.StringReader(Content))
        {
            for (; ; )
            {
                lc++;
                string buf = sr.ReadLine();
                if (null == buf) break;
            }
        }
        TargetText.GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = lc * 35;
    }
}
