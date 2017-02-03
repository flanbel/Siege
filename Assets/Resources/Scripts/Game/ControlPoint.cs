using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlPoint : MonoBehaviour {
    //奪取率
    float DeprivationRate = 0.0f;
    public float rate { get { return DeprivationRate; } }
    //1秒あたり何ポイント取れるか
    public float PoinstPerSecond = 0.0f;
    //奪取中の人数
    int DeprivationPeople = 0;
    //テキスト
    public Text DisplayDeprivationPeople;
    //イメージ
    public Image DisplayDeprivationRate;
    public GameRule Rule;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //ゲームが終了していない。
        if (!Rule.GameSet)
        {

            //誰か奪取中
            if (DeprivationPeople > 0)
            {
                //ポイント増加
                DeprivationRate += PoinstPerSecond;
            }//誰もとってない
            else if (DeprivationRate < 100.0f)
            {
                //ポイント減少
                //減速速度は1/2（適当）
                DeprivationRate -= PoinstPerSecond / 2.0f;
                //0より小さくさせない
                if (DeprivationRate < 0.0f)
                    DeprivationRate = 0;
            }
            //ゲージ更新
            DisplayDeprivationRate.fillAmount = DeprivationRate / 100.0f;
            //人数をテキストに
            if (DeprivationPeople > 0)
                DisplayDeprivationPeople.text = DeprivationPeople.ToString();
            else
                DisplayDeprivationPeople.text = "";
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        //奪取しに来た
        if (col.gameObject.tag == "Player")
        {
            DeprivationPeople++;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        //出て行った
        if (col.gameObject.tag == "Player")
        {
            DeprivationPeople--;
        }
    }
}
