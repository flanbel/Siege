using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
//マルチチャット機能
public class MultiChat : MonoBehaviour {

    //NetworkIdentity iden;

    //キャンバスを保持する用
    GameObject Canvas;
    //ローカルなインプットフィールド
    GameObject inputField = null;
    InputField field;
    //サーバーに発信する用のテキストかな
    //GameObject MultiText;

    // Use this for initialization
    void Start () {
        
        //キャンバスを探す
        Canvas = GameObject.Find("CameraCanvas");
	}
	
	// Update is called once per frame
	void Update () {
        //インプットフィールド生成
        if (Input.GetKeyDown(KeyCode.T) && !inputField)
        {
            //インプットフィールドのインスタンスを生成
            inputField = Instantiate(Resources.Load("Prefabs/InputField")) as GameObject;
            inputField.name = this.name + "InputField";
            //キャンバスに登録
            inputField.transform.SetParent(Canvas.transform);
            inputField.transform.localPosition = new Vector3(0, -150, 0);
            //テキスト取得
            field = inputField.GetComponent<InputField>();
            //フォーカスをアクティブに
            field.ActivateInputField();
        }

        //インプットフィールドがあるとき
        if (inputField)
        {
            //ReturnはEnterのこと
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //文字数
                if (field.text.Length > 0)
                {
                    //テキスト送信
                    //インスタンスを生成
                    GameObject message = Instantiate(Resources.Load("prefabs/MultiMessage") as GameObject);
                    
                    //子のTextに文字を設定
                    message.transform.FindChild("Text").GetComponent<Text>().text = field.text;
                    //テキスト初期化
                    field.text = string.Empty;
                    field.ActivateInputField();
                    
                }
                else
                {
                    //削除
                    Destroy(inputField);
                }
            }
        }
	}
}
