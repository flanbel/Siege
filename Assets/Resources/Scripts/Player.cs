using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    CharacterController Characon;
    [SerializeField]
    float AddGavity = 0.0f;

	// Use this for initialization
	void Start () {
        //キャラクターコントローラーのコンポーネントを取得
        Characon = this.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
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
        }else
        {
            AddGavity = 0.0f;
        }

        Characon.Move(new Vector3(0, AddGavity, 0) * Time.deltaTime);
    }
}
