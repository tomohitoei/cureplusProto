using UnityEngine;
using System.Collections;

public class AdventurePartButtonController : MonoBehaviour {

    public string AdventurePartFile = "";

    private HLRemoting.GameEngine _ge = null;
    // Use this for initialization
	void Start () {
        _ge = HLRemoting.GameEngine.Instance();
	}
	
	// Update is called once per frame
	void Update () {
        _ge.Progress(Time.deltaTime);
	}

    public void Clicked()
    {
        Novel.NovelSingleton.StatusManager.callJoker(AdventurePartFile, string.Empty);
    }
}
