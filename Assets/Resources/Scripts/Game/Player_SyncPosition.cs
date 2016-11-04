using UnityEngine;
using System.Collections;
using UnityEngine.Networking; //UNETを使う時に必要なライブラリ

//UNETを使う時に必要な継承
//相手プレイヤーのポジションを補完するクラス
public class Player_SyncPosition : NetworkBehaviour
{
    //Primitive型のみ許される
    [SyncVar]   //ホストから全クライアントへ送られる
    private Vector3 syncPos;

    //Playerの現在位置
    [SerializeField]
    Transform myTransform;
    //Lerp: ２ベクトル間を補間する
    [SerializeField]
    float lerpRate = 15;

    //前フレームの最終位置
    private Vector3 lastPos;
    //threshold: しきい値、境目となる値のこと
    //0.5unitを超えなければ移動していないこととする
    private float threshold = 0.5f;

    //固定フレームレートで呼び出されるUpdate
    void FixedUpdate()
    {
        Transmit();
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
    void CmdProvidePositionToServer(Vector3 pos,Quaternion qua)
    {
        //サーバー側が受け取る値
        syncPos = pos;
    }

    //クライアントのみ実行される
    [ClientCallback]
    //位置情報を送るメソッド
    void Transmit()
    {
        //自分がローカル（操作している）なら かつ
        //現在位置と前フレームの最終位置との距離がthresholdより大きい時
        if (isLocalPlayer || Vector3.Distance(myTransform.position, lastPos) > threshold)
        {
            CmdProvidePositionToServer(myTransform.position,myTransform.rotation);

            //更新
            lastPos = myTransform.position;
        }
    }
}