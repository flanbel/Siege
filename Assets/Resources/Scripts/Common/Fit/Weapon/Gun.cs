using UnityEngine;
using System.Collections;
/// <summary>
/// 銃の基底クラス
/// </summary>
public abstract class Gun : WeaponBase
{

    /// <summary>
    /// 銃の情報
    /// </summary>
    //[System.Serializable]を書くことによってInspectorに表示・編集することができる
    [System.Serializable]
    public class GunInformation
    {
        //現在の弾数
        public int NowBulletsNum = 0;
        //最大総弾数
        public int MaxBulletsNum = 0;
        //現在のマガジン内の弾の数
        public int NowMagazine = 0;
        //マガジンに装填できる最大数
        public int MaxMagazine = 0;
        //ブレ具合
        public Vector2 Spread;
        //弾の出現するポジション
        public Transform Muzzle;
        //使用する弾のオブジェクト
        public GameObject UsedBulletObj;
    }

    public GunInformation GunInfo;

    // Use this for initialization
    new public void Start() {
        //最大数に合わせる
        GunInfo.NowBulletsNum = GunInfo.MaxBulletsNum;
        GunInfo.NowMagazine = GunInfo.MaxMagazine;
        base.Start();
    }

    // Update is called once per frame
    new public void Update()
    {
        base.Update();
    }

    //WeaponBaseのAttackをオーバーライド
    public override void Attack()
    {
        //マガジンの残弾確認
        if (GunInfo.NowMagazine > 0)
        {
            //ステート切り替え
            if (State == WEAPONSTATE.WAIT)
            {
                base.Attack();

                Shot();
            }
        }
        else
        {
            if (GunInfo.NowBulletsNum > 0)
            {
                //リロード
                Reload();
            }
            //リロードも発射もできない
            else
            {
                //不発音再生
                Audio.PlayOneShot(Sounds.NotAttackSound);
            }
        }
    }

    //リロードはすべて同じものとして実装
    public override void Reload()
    {
        //所持弾数が0より多い　かつ マガジンが最大ではない
        if (GunInfo.NowBulletsNum > 0 &&
            GunInfo.NowMagazine != GunInfo.MaxMagazine)
        {
            if (State == WEAPONSTATE.WAIT)
            {
                base.Reload();

                Audio.PlayOneShot(Sounds.ReloadSound);
                //補填数
                int Compensation = GunInfo.MaxMagazine - GunInfo.NowMagazine;

                //補填数分はある
                if (GunInfo.NowBulletsNum >= Compensation)
                {
                    //補填
                    GunInfo.NowMagazine += Compensation;
                    GunInfo.NowBulletsNum -= Compensation;
                }
                //足りない
                else
                {
                    GunInfo.NowMagazine += GunInfo.NowBulletsNum;
                    GunInfo.NowBulletsNum = 0;
                }
            }
        }
        else
        {
            //リロードできない
            Audio.PlayOneShot(Sounds.NotReloadSound);
        }
    }

    /// <summary>
    /// 弾を補充
    /// </summary>
    /// <param name="addnum"></param>
    public void AddBullets(int addnum)
    {
        GunInfo.NowBulletsNum += addnum;
    }

    protected abstract void Shot();
}
