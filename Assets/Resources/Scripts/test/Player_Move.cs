using UnityEngine;
using System.Collections;
using UnityEngine.Networking; //UNETを使う時に必要なライブラリ

public class Player_Move : NetworkBehaviour
{

    //Primitive型のみ許される
    [SyncVar]   //ホストから全クライアントへ送られる
    private Vector3 dir;    //移動量

    void Update()
    {
        LerpPosition();
    }

    //ポジション補間用メソッド
    void LerpPosition()
    {
        //補間対象は相手プレイヤーのみ
        //if (!isLocalPlayer)
        {
            //Lerp(from, to, 割合) from〜toのベクトル間を補間する
            transform.position += dir;
        }
    }
    //クライアントからホストへ、Position情報を送る
    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        //サーバー側が受け取る値
        dir = pos;
    }

    //クライアントのみ実行される
    [ClientCallback]
    //位置情報を送るメソッド
    public void Transmit(Vector3 vec)
    {
        //ローカル（操作している）かつ
        //移動量がある
        if (isLocalPlayer || vec.magnitude > 0)
        {
            CmdProvidePositionToServer(vec);
        }
    }
}
