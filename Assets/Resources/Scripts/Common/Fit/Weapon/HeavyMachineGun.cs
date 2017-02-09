using UnityEngine;
using System.Collections;
/// <summary>
/// ヘビーマシンガンのクラス
/// </summary>
public class HeavyMachineGun : Gun
{
    //銃身の回転
    [SerializeField]
    protected float Roll = 0.0f;
    //回転の加算
    public float AddRoll = 0.0f;
    //弾を発射するしきい値
    public float FireRoll = 0.0f;
    //発射数が増える区切り
    public float IncreaseRoll = 0;
    Transform t;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        if(IncreaseRoll < FireRoll)
        {
            IncreaseRoll = FireRoll;
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0))
        {
            Roll += AddRoll;
        }
        //回転が減る
        if(State != WEAPONSTATE.ATTACK && !Input.GetMouseButton(0))
        {
            if (Roll > 0)
            {
                Roll -= 1.0f;
            }
            else
                Roll = 0;
        }
    }

    protected override void Shot()
    {
        if (Roll > FireRoll)
        {

            //音再生
            Audio.PlayOneShot(Sounds.AttackSound);

            int num = (int)(Roll / IncreaseRoll);
            for (int i = 0; i < num + 1; i++)
            {
                //発射
                if(0 >= GunInfo.NowMagazine--)
                {
                    break;
                }
                //弾生成
                GameObject bullet = Instantiate(GunInfo.UsedBulletObj);
                //移動
                bullet.transform.localPosition = GunInfo.Muzzle.position;
                //設定
                Vector3 dir = transform.TransformDirection(Vector3.forward);
                dir.x += Random.Range(-GunInfo.Spread.x, GunInfo.Spread.x);
                dir.y += Random.Range(-GunInfo.Spread.y, GunInfo.Spread.y);
                bullet.GetComponent<BulletBase>().SetState(Power, dir, transform.rotation);
                //プレイヤーのタグをみて決定
                if (transform.parent.tag == "Red_Team_Player")
                {
                    //赤チーム
                    bullet.tag = "Red_Team";
                }
                else if (transform.parent.tag == "Blue_Team_Player")
                {
                    //青チーム
                    bullet.tag = "Blue_Team";
                }
            }
        }
    }
}
