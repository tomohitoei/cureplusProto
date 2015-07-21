using UnityEngine;
using System.Collections;
using System.Xml;
using System.Linq;

public class MailController : MonoBehaviour {

    public GameObject MailList = null;
    public UnityEngine.UI.Text ContentName = null;
    public UnityEngine.UI.Text ContentTitle = null;

    public static MailController Instance = null;

    public MailController()
    {
        Instance = this;
    }

	// Use this for initialization
	void Start () {

        GameObject prefab = (GameObject)Resources.Load("Prefabs/MailItemButton");

        XmlDocument xd = new XmlDocument();
        using(System.IO.FileStream s = new System.IO.FileStream("happylagoon.xml",System.IO.FileMode.Open))
        {
            xd.Load(s);
        }
        foreach (XmlNode n in xd.DocumentElement.ChildNodes)
        {
            var o = Instantiate(prefab);
            o.transform.parent = MailList.transform;
            var c = o.GetComponent<MailItemButtonController>();
            foreach (XmlAttribute a in n.Attributes)
            {
                if (a.Name == "Author") c.Name = a.Value;
                if (a.Name == "Subject") c.Subject = a.Value;
                if (a.Name == "Icon")
                {
                    try
                    {
                        var r = (UnityEngine.Texture2D)Resources.Load(string.Format("Textures/{0}", a.Value));
                        UnityEngine.Sprite s = UnityEngine.Sprite.Create(r, new Rect(0, 0, r.width, r.height), new Vector2(0.5f, 0.5f));
                        c.Icon = s;
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogError(ex.ToString());
                    }
                }
            }
            Debug.Log(string.Format("{0}:{1}", c.Name, c.Subject));
            foreach (XmlNode n2 in n.ChildNodes)
            {
            }
        }

        

        //for (int i = 0; i < 15; i++)
        //{
        //    var o = Instantiate(prefab);
        //    o.transform.parent = MailList.transform;
        //    var c = o.GetComponent<MailItemButtonController>();
        //    c.Name = string.Format("{0}item", i);
        //    c.Subject = string.Format("{0}subject", i);
        //}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetItem(MailItemButtonController mic)
    {
        ContentName.text = mic.Name;
        ContentTitle.text = mic.Subject;
    }

}
