using UnityEngine;
using System.Collections;

public class AdventurePartButtonController : MonoBehaviour {

    public string AdventurePartFile = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clicked()
    {
        Novel.NovelSingleton.StatusManager.callJoker(AdventurePartFile, string.Empty);
    }
}
