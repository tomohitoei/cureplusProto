using UnityEngine;
using System.ComponentModel;
using System.Collections;
using System.Linq;
using HLRemoting;
using ScriptDebugger;
using System;

public class KickScenario : MonoBehaviour {

	// Use this for initialization
	void Start () {
        string ts = string.Empty;
        
        var tempPath = System.IO.Path.GetTempPath();
        try
        {
             using (var sr = new System.IO.StreamReader(System.IO.Path.Combine(tempPath, "TargetScenario.txt")))
            {
                ts = sr.ReadLine();
            }
            Novel.NovelSingleton.StatusManager.callJoker(ts,"");

            try{
                var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(System.Collections.Generic.Dictionary<string, object>));
                var settings = new System.Xml.XmlReaderSettings();
                var xr = System.Xml.XmlReader.Create(new System.IO.StreamReader(System.IO.Path.Combine(tempPath, "params.xml")), settings);
                //var param = (System.Collections.Generic.Dictionary<string, object>))(serializer.ReadObject(xw));
                ExitAdventurePart.param = (System.Collections.Generic.Dictionary<string, object>)serializer.ReadObject(xr);
                xr.Close();
                foreach (string key in ExitAdventurePart.param.Keys)
                {
                    var val = ExitAdventurePart.param[key];
                    if (null==val) continue;
                    Novel.StatusManager.variable.set("f."+key ,val.ToString());
                }
                //    stg = _gameData.GameParameters
            }catch(System.Exception /*ex*/)
            {
            }
            

        }
        catch (System.Exception ex)
        {
            System.Windows.Forms.MessageBox.Show(ex.ToString() + " " + ts );
            Application.Quit();
         }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
