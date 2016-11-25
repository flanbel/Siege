using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//必須コンポーネント。
[RequireComponent(typeof(AudioSource))]

public abstract class PlayerBase : MonoBehaviour
{
    //キャラクターコントローラー。
    CharacterController Characon;
    //カメラ。
    Camera camera;
    //キャラクターの体力。
    public float HP = 0.0f;
    //キャラクターの移動速度。
    public float MoveSpeed = 0.0f;
    //キャラクターが装備できるメイン武器の数。
    public WeaponBase[] MainWeaponNum;
    //キャラクターが装備できるサブの数。
    public WeaponBase[] MainWeaponSub;
    //キャラクターの自然回復量。(今の所は全キャラ一律)。
    public float NaturalRecovery = 10;
    //自衛用の攻撃力。
    public float Power = 0.0f;
    //攻撃間隔(Fream数)
    public int AttackInterval = 0;
    //設定されたインターバル
    int Interval = 0;
    //前のIntervalから経過しているフレーム数
    [SerializeField]
    int Elapsed = 0;
    //重力。
    float AddGavity = 0.0f;
    //ジャンプフラグ。
    bool Jamp = false;
    //レイ。
    Ray ray;

    //キャラクターが鳴らす音。
    [System.Serializable]
    public class PlayerSounds
    {
        //攻撃時の音。
        public AudioClip AttackSound;
        //攻撃を受けた時の音。
        public AudioClip DamageSound;
        //移動時の音。
        public AudioClip MoveSound;
    }

    public PlayerSounds PlayaerSounds;
    protected AudioSource Audio;

    //プレイヤーのステートマシン。
    public enum PLAYERSTATE
    {
        //待機中。
        WAIT = 0,
        //攻撃中。
        ATTACK,
        //移動中。
        MOVE,
        //ダメージを受けた時。
        DAMAGE,
    };
    //ステートマシン
    public PLAYERSTATE State;

    public void Start()
    {
        //キャラクターコントローラーのコンポーネントを取得。
        Characon = this.GetComponent<CharacterController>();
        Audio = gameObject.GetComponent<AudioSource>();
        State = PLAYERSTATE.WAIT;
        camera = Camera.main;
    }

    public void Update()
    {
        Move();

        //待機状態なら。
        if (State == PLAYERSTATE.WAIT)
        {

            //マウスの右クリックが押されている間。
            if (Input.GetMouseButton(1))
            {
                Attack();
            }
        }
        //何かしらを行っている状態。
        else
        {
            //インターバルを過ぎた。
            if (Interval <= Elapsed++)
            {
                Interval = Elapsed = 0;
                State = PLAYERSTATE.WAIT;
            }
        }
    }

    public virtual void Attack()
    {
        Interval = AttackInterval;
        State = PLAYERSTATE.ATTACK;
        //音再生。
    }

    public virtual void Move()
    {
        //カメラから見たパッドの入力に変換。
        Vector3 Dir = camera.transform.TransformDirection(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        //WASD機能。
        if (Input.GetKey(KeyCode.W))
        {
            Characon.Move(this.transform.TransformDirection(Vector3.forward) * Time.deltaTime * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Characon.Move(this.transform.TransformDirection(Vector3.back) * Time.deltaTime * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Characon.Move(this.transform.TransformDirection(Vector3.left) * Time.deltaTime * MoveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Characon.Move(this.transform.TransformDirection(Vector3.right) * Time.deltaTime * MoveSpeed);
        }

        //カメラのyが邪魔なので0になる。
        Dir.y = 0.0f;
        Characon.Move(Dir * Time.deltaTime * MoveSpeed);

        //マウスで回転。
        transform.localEulerAngles += new Vector3(0.0f, Input.GetAxis("Mouse X"));

        //重力を加算。
        AddGavity += Physics.gravity.y * Time.deltaTime;

        ray = new Ray(transform.position, Vector3.down);

        //レイが当たっているか、地上に当たっているか。
        if (Physics.Raycast(ray, 0.6f) || Characon.isGrounded)
        {
            AddGavity = 0.0f;
            Jamp = false;
        }

        if (!Jamp)
        {
            //スペースキーでジャンプ。
            if (Input.GetKey(KeyCode.Space))
            {
                Jamp = true;
            }

            if(Input.GetKey("joystick button 0"))
            {
                Jamp = true;
            }
        }

        if (Jamp)
        {
            Characon.Move(new Vector3(0, 10, 0) * Time.deltaTime);
        }

        Characon.Move(new Vector3(0, AddGavity, 0) * Time.deltaTime);
    }

}