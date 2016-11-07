using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//
public class MultiMessage : MonoBehaviour {

    //テキストのオブジェクト
    public GameObject text;
    public Vector2 space;
    RectTransform rect;
    //発信者
    //GameObject Sender;

	// Use this for initialization
	void Start () {
        //自身をキャンバスに登録
        this.transform.SetParent(GameObject.Find("WorldCanvas").transform);
        rect = this.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        AdjustSize();
    }

    //サイズを文字に合わせる
    void AdjustSize()
    {
        RectTransform textRect = text.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(textRect.sizeDelta.x + space.x, textRect.sizeDelta.y + space.y);
    }
}
