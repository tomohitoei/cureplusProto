using UnityEngine;
using System.Collections;
using DesktopMascotMaker;
using System.Windows.Forms;

public class AnimationTestController : MonoBehaviour {

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

        MascotMaker.Instance.OnLeftDoubleClick += LeftMouseDoubleClick;
        mwo.SetMainWindowOpacity(0);
    }

    public MainWindowOpacity mwo = null;
    public bool mainWindowVisible = false;

    private Animator anim = null;
    private float time = 0.0f;
    private float th = 5.0f;
    public bool f = true;
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (th < time)
        {
            th += 5f;
            anim.SetBool("mailReceived", f);
            f = !f;
        }
	}

    public void LeftMouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (mainWindowVisible)
        {
            mwo.SetMainWindowOpacity(0);
            mainWindowVisible = false;
        }
        else
        {
            mwo.SetMainWindowOpacity(255);
            mainWindowVisible = true;
        }
    }

    public void OnGoMailerButtonClicked()
    {
        UnityEngine.Application.LoadLevel("MailPart");
    }
}
