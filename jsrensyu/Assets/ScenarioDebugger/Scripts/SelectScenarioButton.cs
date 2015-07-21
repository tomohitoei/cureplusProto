using UnityEngine;
using System.Collections;
using System.Windows.Forms;
using System.Linq;
using HLRemoting;
using ScriptDebugger;

public class SelectScenarioButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnButtonClick()
    {
        OpenFileDialog f = new OpenFileDialog();
        var id = System.IO.Directory.GetCurrentDirectory() + "\\novel\\data\\scenario";
        f.InitialDirectory = id;
        if (f.ShowDialog() != DialogResult.OK) return;

        var fi=new System.IO.FileInfo(f.FileName);
        var target = f.FileName.Substring(id.Length+1, f.FileName.Length - id.Length - 1); // System.Text.RegularExpressions.Regex.Replace(f.FileName, id, string.Empty);
        target = target.Substring(0, target.Length - fi.Extension.Length); // System.Text.RegularExpressions.Regex.Replace(target, fi.Extension, string.Empty);
        target = System.Text.RegularExpressions.Regex.Replace(target, "\\\\", "/");

        var script = System.IO.File.ReadAllText(f.FileName);
        // Playerシーンから開始してノベルゲームエンジン側を初期化させないと
        // この呼び出しでぬるぽ発生．注意
        var r = Novel.NovelSingleton.GameManager.parser.parseScript(script);

        // TODO : スクリプト内のラベルを列挙して途中から実行できる機能を実装
        var labels = (from c in r where c.tagName.Equals("label") select c.line).ToList();
        var label = DebuggerConsole.Instance().SelectJumpTarget(labels);
        Novel.NovelSingleton.StatusManager.callJoker(target,label);
    }

    void OnApplicationQuit()
    {
        Debug.Log("End");
        ScriptDebugger.DebuggerConsole.Instance().Close();
    }
}
