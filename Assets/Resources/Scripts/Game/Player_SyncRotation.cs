using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Player_SyncRotation : NetworkBehaviour
{

    //SyncVar: ホストサーバーからクライアントへ送られる
    //プレイヤーの角度
    [SyncVar]
    private Quaternion syncPlayerRotation;
    //FirstPersonCharacterのカメラの角度
    //[SyncVar]
    //private Quaternion syncCamRotation;

    [SerializeField]
    private Transform playerTransform;
    //[SerializeField]
    //private Transform camTransform;
    [SerializeField]
    private float lerpRate = 15;

    //前フレームの最終角度
    private Quaternion lastPlayerRot;
    //private Quaternion lastCamRot;
    //しきい値は5。5度以上動いた時のみメソッドを実行
    private float threshold = 5;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //クライアント側のPlayerの角度を取得
        TransmitRotations();
        //現在角度と取得した角度を補間する
        LerpRotations();
    }

    //角度を補間するメソッド
    void LerpRotations()
    {
        //自プレイヤー以外のPlayerの時
        if (!isLocalPlayer)
        {
            //プレイヤーの角度とカメラの角度を補間
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation,
                syncPlayerRotation, Time.deltaTime * lerpRate);
            //camTransform.rotation = Quaternion.Lerp(camTransform.rotation,
            //    syncCamRotation, Time.deltaTime * lerpRate);
        }
    }

    //クライアントからホストへ送られる
    [Command]
    void CmdProvideRotationsToServer(Quaternion playerRot)//, Quaternion camRot)
    {
        syncPlayerRotation = playerRot;
       // syncCamRotation = camRot;
    }

    //クライアント側だけが実行できるメソッド
    [Client]
    void TransmitRotations()
    {
        if (isLocalPlayer)
        {
            if (Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold
                //|| Quaternion.Angle(camTransform.rotation, lastCamRot) > threshold
                )
            {
                CmdProvideRotationsToServer(playerTransform.rotation);//, camTransform.rotation);

                lastPlayerRot = playerTransform.rotation;
                //lastCamRot = camTransform.rotation;
            }
        }
    }
}