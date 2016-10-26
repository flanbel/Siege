using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player : NetworkBehaviour
{
    //キャラクターコントローラー
    CharacterController Characon;
    //ネットワークのオブジェクトを識別する。
    NetworkIdentity Identity;
    [SerializeField]
    float AddGavity = 0.0f;
    [SerializeField]
    float speed = 3;

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
    }
	
	// Update is called once per frame
	void Update () {
        //ローカルなオブジェクト(ローカル側で生成された)なら
        if (Identity.isLocalPlayer)
        {
            //WASD機能
            if (Input.GetKey(KeyCode.W))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.S))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.back) * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.A))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.left) * Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.D))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.right) * Time.deltaTime * speed);
            }
            //マウスで回転
            transform.localEulerAngles += new Vector3(0.0f, Input.GetAxis("Mouse X"));

            //地面についていないなら
            if (!Characon.isGrounded)
            {
                AddGavity += Physics.gravity.y * Time.deltaTime;
            }
            else
            {
                AddGavity = 0.0f;
            }

            Characon.Move(new Vector3(0, AddGavity, 0) * Time.deltaTime);
        }
    }
}
