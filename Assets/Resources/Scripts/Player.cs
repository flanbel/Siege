using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour
{
    //キャラクターコントローラー
    CharacterController Characon;
    //アニメーター
    public Animator Ani;
    //ネットワークのオブジェクトを識別する。
    NetworkIdentity Identity;
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
        camera.transform.localPosition = new Vector3(0, 1, -10);

        //カメラを子に登録
        camera.transform.SetParent(this.transform);
    }

    // Use this for initialization
    void Start () {
        //キャラクターコントローラーのコンポーネントを取得
        Characon = this.GetComponent<CharacterController>();
        //コンポーネント取得
        Identity = this.GetComponent<NetworkIdentity>();

        Ani = this.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //ローカルなオブジェクト(ローカル側で生成された)なら
        if (Identity.isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Ani.SetTrigger("WalkTrigger");
            }

            //WASD機能
            if (Input.GetKey(KeyCode.W))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.forward) * Time.deltaTime * Speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.back) * Time.deltaTime * Speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.left) * Time.deltaTime * Speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.right) * Time.deltaTime * Speed);
            }
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
