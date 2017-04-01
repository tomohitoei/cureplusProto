using UnityEngine;
using System.Collections;

public class DMMSceneChange : MonoBehaviour
{
    public string nextSceneName;

    void Start() { }

    public void SceneChange()
    {
        Application.LoadLevel(nextSceneName);
    }
}
