using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//必須コンポーネント
[RequireComponent(typeof(AudioSource))]

/// <summary>
/// すべての武器の基底クラス。
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    //武器のステートマシン
    public enum WEAPONSTATE
    {
        //待機中
        WAIT = 0,
        //攻撃中
        ATTACK,
        //リロード中
        RELOAD,
        //行動不可
        NOTACTIVE
    };

    //武器が鳴らす音たち
    [System.Serializable]
    public class WeaponSounds
    {
        //攻撃時になる音
        public AudioClip AttackSound;
        //攻撃できなかった時になる
        public AudioClip NotAttackSound;
        //リロード時になる音
        public AudioClip ReloadSound;
        //リロードできなかった時になる
        public AudioClip NotReloadSound;
    }

    //攻撃力
    public float Power = 0.0f;
    //攻撃間隔(Fream数)
    public int AttackInterval = 0;
    //リロード間隔(Fream数)
    public int ReloadInterval = 0;
    //設定されたインターバル
    int Interval = 0;
    //前のIntervalから経過しているフレーム数
    [SerializeField]
    int Elapsed = 0;
    //ステートマシン
    public WEAPONSTATE State;
    //武器が鳴らす音たち
    public WeaponSounds Sounds;
    protected AudioSource Audio;

    // Use this for initialization
    public void Start()
    {
        State = WEAPONSTATE.WAIT;
        Audio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {
        //待機状態なら
        if (State == WEAPONSTATE.WAIT)
        {
            //ホントはプレイヤーでするとこだと思うんです！！！

            //マウスの左クリックが押されている間
            if (Input.GetMouseButton(0))
            {
                Attack();
            }
            //Rキーを押下したとき
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }
        //何かしらを行っている状態
        else
        {
            //インターバルを過ぎた
            if (Interval <= Elapsed++)
            {
                Interval = Elapsed = 0;
                State = WEAPONSTATE.WAIT;
            }
        }
    }

    /// <summary>
    /// 攻撃に使う仮想関数
    /// </summary>
    public virtual void Attack()
    {
        Interval = AttackInterval;
        State = WEAPONSTATE.ATTACK;
    }

    /// <summary>
    /// リロードする仮想関数
    /// </summary>
    public virtual void Reload()
    {
        Interval = ReloadInterval;
        State = WEAPONSTATE.RELOAD;
    }
}