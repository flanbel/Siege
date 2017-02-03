using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

//ネットワークマネージャーを継承
public class NetworkManager_Custom : NetworkManager {

    public GameObject SceneCamera;

    //ButtonStartHostボタンを押した時に実行
    //IPポートを設定し、ホストとして接続
    public void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    //ButtonJoinGameボタンを押した時に実行
    //IPアドレスとポートを設定し、クライアントとして接続
    public void JoinGame()
    {
        SetIPAddress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetIPAddress()
    {
       
        //Input Fieldに記入されたIPアドレスを取得し、接続する
        string ipAddress = GameObject.Find("InputFieldIPAddress").transform.FindChild("Text").GetComponent<Text>().text;
        NetworkManager.singleton.networkAddress = ipAddress;
    }

    //ポートの設定
    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    //プレイヤーが追加された時
    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        //基底クラスのOnServerAddPlayerを呼び出す
        base.OnServerAddPlayer(conn, playerControllerId);
    }

    //プレイヤーが削除された時
    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController playerController)
    {
        base.OnServerRemovePlayer(conn, playerController);
    }

    //ホストが開始した時呼ばれる
    public override void OnStartHost()
    {
        base.OnStartHost();
        
        SceneCamera.SetActive(false);
    }

    public override void OnStopHost()
    {
        SceneCamera.SetActive(true);
        base.OnStopHost();
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    //サーバーが切断されたときにクライアント上で呼び出されます。
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        // Do Something...
    }
}
