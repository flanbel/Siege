using UnityEngine;
using System.Collections;
//ヘルスパック
public class HealthPack : MonoBehaviour {
    //回復割合(0~100%)
    [SerializeField,Range(0,100)]
    public float HealRate = 0.0f;
    //復活間隔
    public float RespawnInterval = 0.0f;
    float timer = 0.0f;
    //存在するか
    bool alive = true;
    //モデルレンダー
    MeshRenderer renderer = null;
    //パーティクル
    public GameObject Particle;



    // Use this for initialization
    void Start () {
        renderer = GetComponent<MeshRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        renderer.enabled = alive;
        Particle.SetActive(alive);
        //存在するなら
        if (alive)
        {
            //回転
            transform.Rotate(Vector3.up, 1.0f);
        }
        //消えてるなら
        else
        {
            timer += Time.deltaTime;
            //リスポーンタイムを超えた
            if(timer >= RespawnInterval)
            {
                //復活
                alive = true;
                transform.localEulerAngles = Vector3.zero;
            }
        }
	}
    //触れた瞬間
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Red_Team_Player" ||
            col.gameObject.tag == "Blue_Team_Player")
        {
            //回復させる
            col.GetComponent<PlayerBase>().ItemRecovery(HealRate / 100);
            col.GetComponent<PlayerBase>().AddNowBullet(100);

            //一時的に消える 
            alive = false;
            timer = 0;
        }
    }
}
