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
            if (Input.GetKey(KeyCode.W))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.forward));
            }
            if (Input.GetKey(KeyCode.S))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.back));
            }
            if (Input.GetKey(KeyCode.A))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.left));
            }
            if (Input.GetKey(KeyCode.D))
            {
                Characon.Move(this.transform.TransformDirection(Vector3.right));
            }

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
