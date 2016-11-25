using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {
    //次のシーン
    public string nextScene;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
    public void ChengeScene()
    {
        //シーン切り替え
        SceneManager.LoadScene(nextScene);
    }
}
