using UnityEngine;
using System.Collections;
using System.Linq;
using System.IO;

public class TextRendererController : MonoBehaviour {

    [Multiline()]public string Content = "あかねchanはめっちゃ可愛い！";
    public string FontName = "メイリオ";
    public int FontSize = 24;
    public float FontScale = 1.0f;
    public float StartPosition = 0.0f;
    public float PaperWidh = 10.0f;
    private HLRemoting.MyTextRenderer _r = null;
    private HLRemoting.IHoge dummy = null;

    // Use this for initialization
	void Start () {
        _r = new HLRemoting.MyTextRenderer(FontName, FontSize);
        var il = _r.hoge(Content);
        System.Collections.Generic.List<Sprite> sl = new System.Collections.Generic.List<Sprite>();
        float x = StartPosition;
        var mt = GetComponent<RectTransform>() as RectTransform;
        float pw = 0.0f;
        float height = il.Max((ii) => ii.Height)/100;
        float top = 0.0f;
        for(int i=0;i<il.Length;i++){
            var image=il[i];
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            image.Save(string.Format("D:\\unity\\akane_{0}.png",i), System.Drawing.Imaging.ImageFormat.Png);
            Texture2D t = new Texture2D(image.Width, image.Height);
            t.LoadImage(ms.GetBuffer());
            Sprite s = Sprite.Create(t, new Rect(0, 0, image.Width, image.Height), new Vector2(0.5f, 0.5f));
            sl.Add(s);

            GameObject go = new GameObject();
            var sr = go.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr.sprite = s;
            var tr = go.GetComponent<Transform>() as Transform;
            tr.parent = mt;
            tr.localScale = new Vector3(FontScale, FontScale, FontScale);
            x += (pw + s.bounds.size.x) / 2.0f* FontScale;
            tr.position = new Vector3(x, tr.position.y + top, tr.position.z);
            pw = s.bounds.size.x ;
            if (StartPosition + PaperWidh <= x + s.bounds.size.x / 2.0f)
            {
                pw = 0.0f;
                x = StartPosition;
                top -= height / 2.0f;
            }
            //x += image.Width * 0.01f;
            Debug.Log(x);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
