using UnityEngine;
using System.Collections;

public class MailItemButtonController : MonoBehaviour {

    public string Name = "ひめ";
    public string Subject = "聞いて聞いて";
    public UnityEngine.Sprite Icon = null;

    public UnityEngine.UI.Text NameField = null;
    public UnityEngine.UI.Text SubjectField = null;
    public UnityEngine.UI.Image IconObject = null;

    // Use this for initialization
	void Start () {
        NameField.text = Name;
        SubjectField.text = Subject;
        IconObject.sprite = Icon;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clicked()
    {
        var mc = MailController.Instance;
        mc.SetItem(this);
    }
}
