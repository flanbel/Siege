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

    void Update()
    {
        //移動が確認できる
       // if(move == true)
        Movement();
    }

    //移動
    void Movement()
    {
        //サーバーならポジションを計算し移動
        if (isServer)
        {
            //移動先計算
            transform.localPosition += dir;
            //移動量初期化(0にする)
            dir = Vector3.zero;
            //移動した先のポジションをクライアントに送りたい。
            pos = transform.localPosition;
        }
        //
        if (isClient)
        {
            //サーバーからのポジションを受け取る
            transform.localPosition = pos;
        }
    }
    //クライアントからホストへ、移動量情報を送る
    [Command]
    void CmdProvidePositionToServer(Vector3 vec)
    {
        //サーバー側が受け取る値
        //サーバーへ移動量を送信
        dir = vec;
    }

    //クライアントのみ実行される
    [ClientCallback]
    //移動量を送るメソッド
    public void SendMove(Vector3 vec)
    {
        //ローカル（操作している）かつ
        //移動量がある
        if (isLocalPlayer && vec.magnitude > 0)
        {
            CmdProvidePositionToServer(vec);
        }
    }


}
