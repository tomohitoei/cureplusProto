using UnityEngine;
using System.Collections;
using System.Linq;
using HLRemoting;
using ScriptDebugger;
using System;

public class ExitAdventurePart : MonoBehaviour {

    public static System.Collections.Generic.Dictionary<string, object> param = null;
	// Use this for initialization
	void Start () {
        //System.Collections.Generic.Dictionary<string, object> param = null;
        //try
        //{
        //    var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(System.Collections.Generic.Dictionary<string, object>));
        //    var settings = new System.Xml.XmlReaderSettings();
        //    var xr = System.Xml.XmlReader.Create(new System.IO.StreamReader("params.xml"), settings);
        //    //var param = (System.Collections.Generic.Dictionary<string, object>))(serializer.ReadObject(xw));
        //    param = (System.Collections.Generic.Dictionary<string, object>)serializer.ReadObject(xr);
        //    xr.Close();
        //}
        //catch (System.Exception /*ex*/)
        //{
        //}
        if (!Novel.StatusManager.variable.dicVar.ContainsKey("f")) return;

        foreach (string key in Novel.StatusManager.variable.dicVar["f"].Keys)
        {
            var val = parse(Novel.StatusManager.variable.dicVar["f"][key]);
            if (param.ContainsKey(key))
            {
                param[key] = val;
            }
            else
            {
                param.Add(key, val);
            }
        }

        try
        {
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(System.Collections.Generic.Dictionary<string, object>));
            var settings = new System.Xml.XmlWriterSettings();
            var tempPath = System.IO.Path.GetTempPath();
            using (var sw = new System.IO.StreamWriter(System.IO.Path.Combine(tempPath, "params.xml")))
            {
                var xw = System.Xml.XmlWriter.Create(sw, settings);
                //var param = (System.Collections.Generic.Dictionary<string, object>))(serializer.ReadObject(xw));
                serializer.WriteObject(xw, param);
                xw.Close();
            }
        }
        catch (System.Exception /*ex*/)
        {
            var xxx = 1;
            xxx += 1;
        }
        finally
        {
            Application.Quit();
        }
	}

    private object parse(string q)
    {
        int tempI;
        float tempF;
        double tempD;
        if (int.TryParse(q, out tempI))
        {
            return tempI;
        }
        else if (float.TryParse(q, out tempF))
        {
            return tempF;
        }
        else if (double.TryParse(q, out tempD))
        {
            return tempD;
        }
        return q;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
