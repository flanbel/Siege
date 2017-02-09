using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//必須コンポーネント。
[RequireComponent(typeof(AudioSource))]

public abstract class PlayerBase : MonoBehaviour
{
    //ハンドガン。
    GameObject handgun;
    //Gun。
    Gun gun;
    //キャラクターコントローラー。
    CharacterController Characon;
    //カメラ。
    new Camera camera;

    [System.Serializable]
    //プレイヤーの情報。
    public class PlayerInformation
    {
        //キャラクターの体力。
        public float HP = 0.0f;
        //キャラクターの体力の最大値。
        public float MaxHP = 0.0f;
        //キャラクターの移動速度。
        public float MoveSpeed = 0.0f;
        //キャラクターが装備できるメイン武器の数。
        public Fit[] MainWeaponNum;
        //今現在装備している武器のインデックス。
        public int NowWeaponIndex = 0;
        //装備している武器。
        public Fit SelectFit;
        //キャラクターの自然回復量。(今の所は全キャラ一律)。
        public float NaturalRecovery = 10;
        //自衛用の攻撃力。
        public float Power = 0.0f;
        //攻撃間隔(Fream数)。
        public int AttackInterval = 0;
        //プレイヤーの現在の状態。
        public PlayerState NowPlayerState = 0;
    }
    public PlayerInformation playerInfo;

    public enum PlayerState
    {
        State=0,
        Walk,
    };

    GameObject Spawner;
    //選択した武器のインスタンスをまとめて格納。
    public GameObject[] child;
    //マウスのホイールの値を格納。
    float MouseScrollValue;
    //設定されたインターバル。
    int Interval = 0;
    //前のIntervalから経過しているフレーム数。
    [SerializeField]
    int Elapsed = 0;
    //重力。
    float AddGavity = 0.0f;
    //ジャンプフラグ。
    bool Jamp = false;
    //レイ。
    Ray ray;
    //カメラがターゲットしているプレイヤーの位置。
    Vector3 TargetPos;
    //パッドから入力されたYの入力量。
    float CameraAngleY;

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
        //リスポーン地点を取得。
        Spawner = GameObject.Find("Blue_Spawner");
        //リスポーン地点内にプレイヤーを配置。
        this.transform.position = new Vector3(Random.Range(0.0f, Spawner.transform.localScale.x / 2.0f), 1.0f, Random.Range(0.0f, Spawner.transform.localScale.z / 2.0f)) + Spawner.transform.localPosition;
        //キャラクターコントローラーのコンポーネントを取得。
        Characon = this.GetComponent<CharacterController>();
        Audio = gameObject.GetComponent<AudioSource>();
        //ゲームオブジェクトのハンドガンを検索。
        handgun = GameObject.Find("HandGun");
        //検索してきたハンドガンからGunのコンポーネントを取得。
        gun = handgun.GetComponent<Gun>();
        //プレイヤーに設定された最大HPを現在のHPに設定。
        playerInfo.HP = playerInfo.MaxHP;
        State = PLAYERSTATE.WAIT;
        camera = Camera.main;
        TargetPos = transform.position;

        PlayerWeaponSet();

    }

    public void Update()
    {
       
        TargetPos = transform.position;
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

        WeaponChange();

    }

    public virtual void Attack()
    {
        Interval = playerInfo.AttackInterval;
        State = PLAYERSTATE.ATTACK;
        //音再生。
    }

    public virtual void Move()
    {
        //カメラから見たパッドの入力に変換。
        Vector3 Dir = camera.transform.TransformDirection(Input.GetAxisRaw("Horizontal"), 0.0f, -Input.GetAxisRaw("Vertical"));
        camera.transform.RotateAround(TargetPos, Vector3.up, Input.GetAxisRaw("Horizontal1"));
        CameraAngleY = Input.GetAxisRaw("Vertical1");
        if (-30 < camera.transform.rotation.y && camera.transform.rotation.y < 30)
        {
            camera.transform.Rotate(new Vector3(CameraAngleY, 0, 0));
        }

        //WASD機能。
        //カメラから見たキー入力に変更。
        if (Input.GetKey(KeyCode.W))
        {
            Dir = camera.transform.TransformDirection(0.0f, 0.0f, 1.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Dir = camera.transform.TransformDirection(0.0f, 0.0f, -1.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Dir = camera.transform.TransformDirection(-1.0f, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Dir = camera.transform.TransformDirection(1.0f, 0.0f, 0.0f);
        }

        //カメラのyが邪魔なので0にする。
        Dir.y = 0.0f;
        Characon.Move(Dir * Time.deltaTime * playerInfo.MoveSpeed);

        //マウスで回転。
        transform.localEulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")*1.2f);

        //重力を加算。
        AddGavity += Physics.gravity.y * Time.deltaTime;

        ray = new Ray(transform.position, Vector3.down);

        //レイが当たっているか、地上に当たっているか。
        if (Physics.Raycast(ray, 1.2f) || Characon.isGrounded)
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

            //XboxコントローラーのAボタン。
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
        State = PLAYERSTATE.MOVE;
    }
    //所持している銃の弾の数を増やす。
    public void AddNowBullet(int addnum)
    {
        gun.AddBullets(addnum);
    }
  
    //プレイヤーのHPの増減処理。
    public void AddHp(float addnum)
    {
        playerInfo.HP += addnum;
    }

    //アイテムを使ったHPの回復処理。
    public void ItemRecovery(float rate)
    {
       playerInfo.HP += playerInfo.MaxHP * rate;
    }
    public void WeaponChange()
    {
        //何か装備していたらホイールで武器切り替えをする。
        if (playerInfo.MainWeaponNum.Length > 1)
        {
            MouseScrollValue = Input.GetAxis("Mouse ScrollWheel");
            //ホイールが手前に入力。
            //武器の配列の位置を一つ後ろに進める。
            if (MouseScrollValue < 0.0f)
            {
                playerInfo.NowWeaponIndex = (playerInfo.NowWeaponIndex + 1) % playerInfo.MainWeaponNum.Length;
                //切り替えが発生したので武器を設定。
                playerInfo.SelectFit = playerInfo.MainWeaponNum[playerInfo.NowWeaponIndex];
            }
            //ホイールが奥に入力。
            //武器の配列の位置を一つ前に進める。
            if (MouseScrollValue > 0.0f)
            {
                //配列の添え字が0なら一番最後の添え字にジャンプ。
                if (playerInfo.NowWeaponIndex == 0)
                {
                    playerInfo.NowWeaponIndex = playerInfo.MainWeaponNum.Length - 1;
                }
                else
                {
                    playerInfo.NowWeaponIndex--;
                }
                //切り替えが発生したので武器を設定。
                playerInfo.SelectFit = playerInfo.MainWeaponNum[playerInfo.NowWeaponIndex];
            }
            //パッドでの武器切り替え。
            if (Input.GetKey("joystick button 3"))
            {
                playerInfo.NowWeaponIndex = (playerInfo.NowWeaponIndex + 1) % playerInfo.MainWeaponNum.Length;
                //切り替えが発生したので武器を設定。
                playerInfo.SelectFit = playerInfo.MainWeaponNum[playerInfo.NowWeaponIndex];
            }
        }
    }
    //生成した武器をプレイヤーの子にする処理。
    public void PlayerWeaponSet()
    {
        child = new GameObject[playerInfo.MainWeaponNum.Length];
        //child = Resources.LoadAll<Fit>("Prafabs/Fit/Weapon");
        //プレイヤーが所持している武器からインスタンスを生成。
        for (int i = 0; i < playerInfo.MainWeaponNum.Length; i++)
        {
            child[i] = (GameObject)Instantiate(playerInfo.MainWeaponNum[i].gameObject, Vector3.zero, Quaternion.identity);
            child[i].transform.SetParent(transform);
            child[i].transform.localPosition = new Vector3(0.57f, 0.0f, 0.81f);
        }
    }
}