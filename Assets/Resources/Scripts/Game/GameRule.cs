using UnityEngine;
using System.Collections;
using UnityEngine.UI;
//ゲームのルールを決める
public class GameRule : MonoBehaviour {

    //制限時間
    public float RmitTime = 0.0f;
    //コントロールポイント
    public ControlPoint ControlPoint;

    //時間を表示するテキストオブジェクト
    public Text DisplayTimeText;
    //延長戦

    //ゲームセットのテキストオブジェクト
    public GameObject Gamesset;
    public bool GameSet = false;

    // Use this for initialization
    void Start () {
        //分
        string time = ((int)RmitTime / 60).ToString();
        time += ':';
        //秒
        time += ((int)RmitTime % 60).ToString();
        DisplayTimeText.text = time;
    }
	
	// Update is called once per frame
	void Update () {
        //終了してない
        if (!GameSet)
        {
            //制限時間を過ぎてない
            if (RmitTime > 0.0f)
            {
                //カウントダウン
                RmitTime -= Time.deltaTime;
                //分
                string time = ((int)RmitTime / 60).ToString();
                time += ':';
                //秒
                if (((int)RmitTime % 60) < 10)
                {
                    //一けたなら0を付け足す
                    time += '0' + ((int)RmitTime % 60).ToString();
                }
                else
                {
                    time += ((int)RmitTime % 60).ToString();
                }
                DisplayTimeText.text = time;
            }
            //延長戦
            else
            {
                //まだ奪取中なら延長戦
                if (ControlPoint.rate > 0.0f)
                {
                    DisplayTimeText.text = "延長戦！！";
                    DisplayTimeText.color = Color.red;
                }
                else
                {
                    //ゲーム終了
                    Gamesset.SetActive(true);
                    GameSet = true;
                }
            }
            //奪取したなら
            if (ControlPoint.rate >= 100.0f)
            {
                //ゲーム終了
                Gamesset.SetActive(true);
                GameSet = true;
            }
        }
    }
}
