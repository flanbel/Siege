using UnityEngine;
using System.Collections;
using UnityEngine.Networking; //UNETを使う時に必要なライブラリ

//UNETを使う時に必要な継承
public class Player_SyncPosition : NetworkBehaviour
{

    [SyncVar]   //ホストから全クライアントへ送られる
    private Vector3 syncPos;
    //Playerの現在位置
    [SerializeField]
    Transform myTransform;
    //Lerp: ２ベクトル間を補間する
    [SerializeField]
    float lerpRate = 15;

    void FixedUpdate()
    {
        TransmitPosition();
        LerpPosition(); //2点間を補間する
    }

    //ポジション補間用メソッド
    void LerpPosition()
    {
        //補間対象は相手プレイヤーのみ
        if (!isLocalPlayer)
        {
            //Lerp(from, to, 割合) from〜toのベクトル間を補間する
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
        }
    }
    //クライアントからホストへ、Position情報を送る
    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        //サーバー側が受け取る値
        syncPos = pos;
    }

    //クライアントのみ実行される
    [ClientCallback]
    //位置情報を送るメソッド
    void TransmitPosition()
    {
        if (isLocalPlayer)
        {
            CmdProvidePositionToServer(myTransform.position);
        }
    }
}