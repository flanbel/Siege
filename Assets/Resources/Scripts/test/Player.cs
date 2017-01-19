using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//必須コンポーネント
[RequireComponent(typeof(CharacterController))]

public class Player : NetworkBehaviour
{
    //キャラクターコントローラー
    CharacterController Characon;
    //ネットワークのオブジェクトを識別する。
    NetworkIdentity Identity;
    Player_Move move;
    [SerializeField]
    float AddGavity = 0.0f;
    [SerializeField]
    float Speed = 3;
    bool Jamp = false;

    Ray ray;

    //ローカルで生成されたなら実行
    public override void OnStartLocalPlayer()
    {
        //ローカルなカメラ生成
        GameObject camera = Instantiate(Resources.Load("Prefabs/GameCamera")) as GameObject;
        camera.name = "GameCamera";
        //カメラを子に登録
        camera.transform.SetParent(this.transform);

        camera.transform.localPosition = new Vector3(0, 1, -10);
    }

    // Use this for initialization
    void Start () {
        //キャラクターコントローラーのコンポーネントを取得
        Characon = this.GetComponent<CharacterController>();
        //コンポーネント取得
        Identity = this.GetComponent<NetworkIdentity>();

        //
        move = this.GetComponent<Player_Move>();
    }
	
	// Update is called once per frame
	void Update () {
        //ローカルなオブジェクト(ローカル側で生成された)なら
        if (Identity.isLocalPlayer)
        {
            Vector3 dir = Vector3.zero;
            //WASD機能
            if (Input.GetKey(KeyCode.W))
            {
                dir = this.transform.TransformDirection(Vector3.forward) * Time.deltaTime * Speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                dir = this.transform.TransformDirection(Vector3.back) * Time.deltaTime * Speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                dir = this.transform.TransformDirection(Vector3.left) * Time.deltaTime * Speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                dir = this.transform.TransformDirection(Vector3.right) * Time.deltaTime * Speed;
            }
            //移動
            move.Transmit(dir);

            //マウスで回転
            transform.localEulerAngles += new Vector3(0.0f, Input.GetAxis("Mouse X"));

            //重力を加算
            AddGavity += Physics.gravity.y * Time.deltaTime;

            ray = new Ray(transform.position, Vector3.down);

            //レイが当たっているか、地上に当たっているか
            if (Physics.Raycast(ray, 0.6f) || Characon.isGrounded)
            {
                AddGavity = 0.0f;
                Jamp = false;
            }

            if (!Jamp)
            {
                //スペースキーでジャンプ
                if (Input.GetKey(KeyCode.Space))
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

    void FixedUpdate()
    {
        
    }
}
