using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enter : MonoBehaviour {

    float wave = 0.0f;
    public float addSin = 0.0f;
    Text text;

	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.Return))
        {
            //シーン切り替え
            SceneManager.LoadScene("Menu");
        }

        //チカチカさせる処理
        Color col = text.color;
        //aを変更
        wave += addSin;
        //sin波は-1~1なので0~1になるように調整
        col.a = ((Mathf.Sin(wave) + 1)/2);
        text.color = col;
	}
}
