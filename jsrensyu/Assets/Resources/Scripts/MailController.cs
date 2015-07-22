using UnityEngine;
using System.Collections;
using System.Xml;
using System.Linq;

public class MailController : MonoBehaviour {

    public GameObject MailList = null;
    public UnityEngine.UI.Text ContentName = null;
    public UnityEngine.UI.Text ContentTitle = null;
    public GameObject ContentPanel = null;

    public int FontSize = 24;
    public int MailContentWidth = 512;

    public static MailController Instance = null;

    public MailController()
    {
        Instance = this;
    }

    private System.Drawing.Image LoadImage(string filename)
    {
        var t=LoadTexture(filename);
        var bytes=t.EncodeToPNG();
        var img=System.Drawing.Image.FromStream(new System.IO.MemoryStream(bytes));
        return img;
    }
    private UnityEngine.Texture2D LoadTexture(string filename)
    {
        try
        {
            return (UnityEngine.Texture2D)Resources.Load(string.Format("Textures/{0}", filename));
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
            return null;
        }
    }
    private UnityEngine.Sprite LoadSprite(string filename)
    {
        try
        {
            var r = (UnityEngine.Texture2D)Resources.Load(string.Format("Textures/{0}", filename));
            UnityEngine.Sprite s = UnityEngine.Sprite.Create(r, new Rect(0, 0, r.width, r.height), new Vector2(0.5f, 0.5f));
            return s;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
            return null;
        }
    }
    private HLRemoting.MyTextRenderer _tr = null;

    // Use this for initialization
	void Start () {
        _tr = new HLRemoting.MyTextRenderer("メイリオ", FontSize);

        GameObject prefab = (GameObject)Resources.Load("Prefabs/MailItemButton");

        XmlDocument xd = new XmlDocument();
        using(System.IO.FileStream s = new System.IO.FileStream("happylagoon.xml",System.IO.FileMode.Open))
        {
            xd.Load(s);
        }
        int key = 1;
        foreach (XmlNode n in xd.DocumentElement.ChildNodes)
        {
            var o = Instantiate(prefab);
            o.transform.parent = MailList.transform;
            var c = o.GetComponent<MailItemButtonController>();
            c.Key = key;
            foreach (XmlAttribute a in n.Attributes)
            {
                if (a.Name == "Author") c.Name = a.Value;
                if (a.Name == "Subject") c.Subject = a.Value;
                if (a.Name == "Icon") c.Icon = LoadSprite(a.Value);
                if (a.Name.Equals("Stamp")) c.Stamp = LoadSprite(a.Value);
                if (a.Name.Equals("Adventure")) c.AdventurePart = a.Value;
            }
            foreach (XmlNode child in n.ChildNodes)
            {
                if (child.Name.Equals("Content"))
                {
                    c.Content = child.InnerText;
                }
                else if (child.Name.Equals("Repries"))
                {

                }
            }
            key += 1;
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

        GameObject ci = (GameObject)Resources.Load("Prefabs/ContentImage");
        // 本文
        {
            var emoji = new System.Collections.Generic.Dictionary<string, System.Drawing.Image>();
            emoji.Add("hare", LoadImage("emoji/hare"));
            emoji.Add("ame", LoadImage("emoji/ame"));
            emoji.Add("kumori", LoadImage("emoji/kumori"));

            _tr.EMoji = emoji;
            var mi = _tr.MakeImage(mic.Key, mic.Content, MailContentWidth, 1.5f);
            var ms =new System.IO.MemoryStream();
            mi.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            var o = ContentPanel.transform.FindChild("MailContent");
            Texture2D t = new Texture2D(mi.Width, mi.Height);
            t.LoadImage(ms.GetBuffer());
            o.GetComponent<UnityEngine.UI.Image>().sprite = UnityEngine.Sprite.Create(t, new Rect(0, 0, mi.Width, mi.Height), new Vector2(0, 0));
            var le = o.GetComponent<UnityEngine.UI.LayoutElement>();
            le.preferredHeight = (int)(mi.Height * 357 / mi.Width);
        }
        // スタンプ
        var so = ContentPanel.transform.FindChild("StampContent");
        if (null != mic.Stamp)
        {
            so.GetComponent<UnityEngine.UI.Image>().sprite = mic.Stamp;
            var le = so.GetComponent<UnityEngine.UI.LayoutElement>();
            le.preferredHeight = mic.Stamp.texture.height;
            var nn = le.preferredHeight;
        }else
        {
            so.GetComponent<UnityEngine.UI.Image>().sprite = null;
            so.GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 0;
        }
        // ボタン
        var bo = ContentPanel.transform.FindChild("GotoAdventure");
        if (!string.IsNullOrEmpty(mic.AdventurePart))
        {
            bo.transform.FindChild("Text").gameObject.GetComponent<UnityEngine.UI.Text>().text = mic.AdventurePart;
            bo.GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 30;
        }
        else
        {
            bo.GetComponent<UnityEngine.UI.LayoutElement>().preferredHeight = 0;
        }
    }

}
