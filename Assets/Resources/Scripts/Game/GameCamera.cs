using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
    public Camera camera;
    //1~4
    public int Idx = 0;
    public int peoplenum = 0;
	// Use this for initialization
	void Start () {
        //プレイヤーコンポーネントから添え字取得
        transform.parent.GetComponent<Player>();
        peoplenum = GameObject.Find("GameRule").GetComponent<ScreenSplit>().num;
        float X, Y, W, H;
        //1・3,2・4
        X = (Idx + 1) % 2 * 0.5f;
        Y = 0.0f
;        if (Idx / 3 == 0)
            Y = 0.5f;
        W = H = 0.5f;
        if(Idx == 3 && peoplenum == 3)
        {
            W = 1.0f;
        }
        camera.rect = new Rect(X, Y, W, H);

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
