using UnityEngine;
using System.Collections;
using UnityEngine.Networking; //UNETを使う時に必要なライブラリ

public class Player_Move : NetworkBehaviour
{

    //Primitive型のみ許される
    [SyncVar]
    private Vector3 dir;    //移動量
    [SyncVar]
    private Vector3 pos;    //移動した先
    [SyncVar]
    private bool move = false;      //移動フラグ

    void Update()
    {
        if(move == true)
        LerpPosition();
    }

    //ポジション補間用メソッド
    void LerpPosition()
    {
        //サーバーなら
        if (isServer)
        {
            //移動
            transform.localPosition += dir;
            pos = transform.localPosition;
        }else if (isLocalPlayer)
        {
            transform.localPosition = pos;
        }
    }
    //クライアントからホストへ、Position情報を送る
    [Command]
    void CmdProvidePositionToServer(Vector3 vec)
    {
        //サーバー側が受け取る値
        //サーバーへ移動量を送信
        dir = vec;
    }

    //クライアントのみ実行される
    [ClientCallback]
    //位置情報を送るメソッド
    public void Transmit(Vector3 vec)
    {
        move = false; 
        //ローカル（操作している）かつ
        //移動量がある
        if (isLocalPlayer && vec.magnitude > 0)
        {
            CmdProvidePositionToServer(vec);
            move = true;
        }
    }
}
