using UnityEngine;
using System.Collections;
/// <summary>
/// ナイフピストル(仮)のクラス
/// </summary>
public class KnifePistol : Gun
{

    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected override void Shot()
    {
        //音再生
        Audio.PlayOneShot(Sounds.AttackSound);
        //発射
        GunInfo.NowMagazine--;
        //弾生成
        GameObject bullet = Instantiate(GunInfo.UsedBulletObj);
        //移動
        bullet.transform.localPosition = GunInfo.Muzzle.position;
        //設定
        bullet.GetComponent<BulletBase>().SetState(Power, transform.TransformDirection(Vector3.forward), transform.rotation);
    }
}
