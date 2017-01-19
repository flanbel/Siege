using UnityEngine;
using System.Collections;

//必須コンポーネント。
[RequireComponent(typeof(BoxCollider))]

//弾薬箱
public class AmmoPack : MonoBehaviour {
    //増加率
    private int IncreaseRate = 100;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCollisionEnter(Collision collision)
    {
        //プレイヤーに当たったなら
        if (collision.gameObject.tag == "Player")
        {

        }
    }
}
