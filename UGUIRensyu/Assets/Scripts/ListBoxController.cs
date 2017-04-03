using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListBoxController : MonoBehaviour
{

    public GameObject prefab = null;
    public RectTransform target = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool TopToBottom = true;

    private List<string> _items = new List<string>();
    private int _count = 1;
    public void AddNew()
    {
        var newItem = string.Format("{0}item", _count);
        _items.Add(newItem);
        _count++;

        var item = GameObject.Instantiate(prefab);
        var t = item.GetComponentInChildren<UnityEngine.UI.Text>();
        t.text = newItem;

        //var temp = new SortedDictionary<int,Transform>();
        //for (int i = 0; i < target.childCount; i++)
        //{
        //    var c = target.GetChild(i);
        //    temp.Add(c.GetSiblingIndex(), c);
        //}
        item.transform.SetParent(target, false);
        item.transform.SetSiblingIndex(1);
    }
}


