using UnityEngine;
using System.Collections;

public class SpawnPlayer : MonoBehaviour {
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
            GameObject player = Instantiate(PlayerPrefab);
            //プレイヤーの添え字生成
            player.GetComponent<NormalPlayer>().index = i;
            //プレイヤーのタイプ設定
            if(i % 2 == 0)
            {
                //赤
                player.tag = "Red_Team_Player";
            }
            else
            {
                //青
                player.tag = "Blue_Team_Player";
            }
            
        }
	}

    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
	
	}
}
