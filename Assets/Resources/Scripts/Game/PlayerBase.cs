using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamepadInput;
using UnityEngine.UI;

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
    Camera camera;
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
        //ステートマシン
        public PLAYERSTATE State = PLAYERSTATE.WAIT;
        //カメラの回転度。
    }
    public PlayerInformation playerInfo;

    //リスポーン地点。
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
    //何番目のプレイヤーかの添え字
    private int PlayerIndex = 0;
    //プレイヤーの体力。
    Image HpImag;

    public int index
    {
        get{
            return PlayerIndex;
        }

        set { PlayerIndex = value; }
    }

    

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

    public void Awake()
    {
        playerInfo.SelectFit = playerInfo.MainWeaponNum[playerInfo.NowWeaponIndex];

        GameObject Img = (GameObject)Instantiate(Resources.Load("Prefabs/HPGauge"), GameObject.Find("Canvas").transform);
        HpImag = Img.transform.FindChild("HP").gameObject.GetComponent<Image>();
       switch(PlayerIndex)
        {
            case 0:
                Img.transform.localPosition = new Vector3(-289.5f, 35.0f, 0.0f);
                break;
            case 1:
                Img.transform.localPosition = new Vector3(35.0f, 35.0f, 0.0f);
                break;
            case 2:
                Img.transform.localPosition = new Vector3(-289.5f, -147.5f, 0.0f);
                break;
            case 3:
                Img.transform.localPosition = new Vector3(35.0f, -147.5f, 0.0f);
                break;
        }

        //プレイヤーがどのチームに所属しているかチェック。
        tag = gameObject.tag;
        if (tag == "Red_Team_Player")
        {
            //赤チームのリスポーン地点を取得。
            Spawner = GameObject.Find("Red_Spawner");
        }
        else if (tag == "Blue_Team_Player")
        {
            //青チームのリスポーン地点を取得。
            Spawner = GameObject.Find("Blue_Spawner");
        }
        //リスポーン地点内にプレイヤーを配置。
        this.transform.position = new Vector3(Random.Range(0.0f, Spawner.transform.localScale.x / 2.0f), 1.0f, Random.Range(0.0f, Spawner.transform.localScale.z / 2.0f)) + Spawner.transform.localPosition;
        //キャラクターコントローラーのコンポーネントを取得。
        Characon = this.GetComponent<CharacterController>();
        Audio = gameObject.GetComponent<AudioSource>();
        
        //プレイヤーに設定された最大HPを現在のHPに設定。
        playerInfo.HP = playerInfo.MaxHP;
        playerInfo.State = PLAYERSTATE.WAIT;
        GameObject came = gameObject.transform.FindChild("Main Camera").gameObject;
        camera = came.GetComponent<Camera>();
        TargetPos = transform.position;
        PlayerWeaponSet();


    }

    public void Update()
    {    
        TargetPos = transform.position;
        Move();
        var KeyState = GamePad.GetState((GamePad.Index)PlayerIndex + 1, false);
        //待機状態なら。
        if (playerInfo.State == PLAYERSTATE.WAIT)
        {
            //var playerNo = GamePad.Index.One;
            
            //マウスの右クリックが押されている間。
            //if (Input.GetMouseButton(1))
            //{
            //    Attack();
            //}
        }
        //何かしらを行っている状態。
        else
        {
            //インターバルを過ぎた。
            if (Interval <= Elapsed++)
            {
                Interval = Elapsed = 0;
                playerInfo.State = PLAYERSTATE.WAIT;
            }
        }

        //if (KeyState.RightShoulder)
        //{
        //    Attack();
        //}

        //if (KeyState.X)
        //{
        //    Reload();
        //}

        //落下死したら自陣地に復活。
        if (TargetPos.y < -10)
        {
            transform.position = new Vector3(Random.Range(0.0f, Spawner.transform.localScale.x / 2.0f), 1.0f, Random.Range(0.0f, Spawner.transform.localScale.z / 2.0f)) + Spawner.transform.localPosition;
        }

        //体力が無くなったら自陣地に復活。
        if (playerInfo.HP < 0)
        {
            transform.position = new Vector3(Random.Range(0.0f, Spawner.transform.localScale.x / 2.0f), 1.0f, Random.Range(0.0f, Spawner.transform.localScale.z / 2.0f)) + Spawner.transform.localPosition;
            playerInfo.HP = playerInfo.MaxHP;
        }

        WeaponChange();

        HpImag.fillAmount = playerInfo.HP / playerInfo.MaxHP;

    }

    public virtual void Attack()
    {
        Interval = playerInfo.AttackInterval;
        playerInfo.State = PLAYERSTATE.ATTACK;
        WeaponBase weapon;
        weapon = (WeaponBase)playerInfo.SelectFit;
        weapon.Attack();
        //音再生。
    }

    public virtual void Reload()
    {
        WeaponBase weapon;
        weapon = (WeaponBase)playerInfo.SelectFit;
        weapon.Reload();
    }

    public virtual void Move()
    {
        //var playerNo = GamePad.Index.Any;
        var KeyState = GamePad.GetState((GamePad.Index)PlayerIndex+1, false);
        //カメラから見たパッドの入力に変換。
        Vector3 Dir = camera.transform.TransformDirection(KeyState.LeftStickAxis.x, 0.0f, KeyState.LeftStickAxis.y);
        transform.RotateAround(TargetPos, Vector3.up, KeyState.rightStickAxis.x);

        float angle = transform.eulerAngles.x + -KeyState.rightStickAxis.y;
        //if ((0 < angle && angle < 40) || (angle < 0 && angle < -330))
        //{
        //    transform.Rotate(-KeyState.rightStickAxis.y, 0.0f, 0.0f);
        //}

        if(Mathf.Abs(angle)>40&&
            Mathf.Abs(angle)<360-30)
        {
            transform.Rotate(-KeyState.rightStickAxis.y, 0.0f, 0.0f);
        }
        //WASD機能。
        //カメラから見たキー入力に変更。
        //if (Input.GetKey(KeyCode.W))
        //{
        //    Dir = camera.transform.TransformDirection(0.0f, 0.0f, 1.0f);
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    Dir = camera.transform.TransformDirection(0.0f, 0.0f, -1.0f);
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    Dir = camera.transform.TransformDirection(-1.0f, 0.0f, 0.0f);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    Dir = camera.transform.TransformDirection(1.0f, 0.0f, 0.0f);
        //}

        //カメラのyが邪魔なので0にする。
        Dir.y = 0.0f;
        //入力量を直接移動量にしていたので極端に下を見ると入力量が減り移動が遅くなるので正規化して解決。
        Dir.Normalize();
        Characon.Move(Dir * Time.deltaTime * playerInfo.MoveSpeed);

        //マウスで回転。
        //transform.localEulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X")*1.2f);

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
            //if (Input.GetKey(KeyCode.Space))
            //{
            //    Jamp = true;
            //}

            //XboxコントローラーのAボタン。
            if (KeyState.A)
            {
                Jamp = true;
            }
        }

        if (Jamp)
        {
            Characon.Move(new Vector3(0, 10, 0) * Time.deltaTime);
        }

        Characon.Move(new Vector3(0, AddGavity, 0) * Time.deltaTime);
        playerInfo.State = PLAYERSTATE.MOVE;
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
        float Heal = playerInfo.MaxHP * rate;
        if (playerInfo.MaxHP < playerInfo.HP + Heal)
        {
            playerInfo.HP = playerInfo.MaxHP;
        }
        else
        {
            playerInfo.HP += Heal;
        }
    }
    public void WeaponChange()
    {
        //何か装備していたらホイールで武器切り替えをする。
        if (playerInfo.MainWeaponNum.Length > 1)
        {
           // MouseScrollValue = Input.GetAxis("Mouse ScrollWheel");
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


            var KeyState = GamePad.GetState((GamePad.Index)PlayerIndex+1, false);
            //パッドでの武器切り替え。
            if (KeyState.Y)
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
            if (child[i] == null)
                continue;
            child[i] = (GameObject)Instantiate(playerInfo.MainWeaponNum[i].gameObject, Vector3.zero, Quaternion.identity);
            child[i].transform.SetParent(transform);
            child[i].transform.localPosition = new Vector3(0.57f, 0.0f, 0.81f);
        }
    }

    public void  OnCollisionEnter(Collision coll)
    {
        //自分が赤チームで飛んできた弾のタグが青ならダメージを食らう。
        if (tag == "Blue_Team_Player" && coll.gameObject.tag == "Red_Team")
        {
            AddHp(-10);
        }
        //自分が青チームで飛んできた弾のタグが赤ならダメージを食らう。
        else if (tag == "Red_Team_Player" && coll.gameObject.tag == "Blue_Team")
        {
            AddHp(-10);
        }

    }

    public void OnTriggerEnter(Collider coll)
    {
        //自分が赤チームで飛んできた弾のタグが青ならダメージを食らう。
        if (tag == "Blue_Team_Player" && coll.gameObject.tag == "Red_Team")
        {
            AddHp(-10);
        }
        //自分が青チームで飛んできた弾のタグが赤ならダメージを食らう。
        else if (tag == "Red_Team_Player" && coll.gameObject.tag == "Blue_Team")
        {
            AddHp(-10);
        }
    }

}