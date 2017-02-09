using UnityEngine;
using System.Collections;

public class ScreenSplit : MonoBehaviour {
    public int num = 0;
    //プレイヤーのプレハブ
    public GameObject PlayerPrefab;
	// Use this for initialization
	void Awake() {
        //人数取得
        GameObject people = GameObject.Find("people");
        if (people)
        {
            num = (int)people.transform.localPosition.x;
            GameObject.Destroy(people);
        }
        else
            num = 4;

        for(int i = 0; i < num; i++)
        {
            //プレイヤー生成
            Instantiate(PlayerPrefab);
        }
	}

    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
	
	}
}
