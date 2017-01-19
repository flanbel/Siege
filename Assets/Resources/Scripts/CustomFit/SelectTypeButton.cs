using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectTypeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //説明文を表示するText
    public Text ExplanatoryText;
    //説明文
    [SerializeField, MultilineAttribute(3)]
    public string Explanatory;
    //自身のボタンコンポーネント
    private Button MyButton;

    static private GameObject Custom = null, Select = null;
    // Use this for initialization
    void Start () {
        //コンポーネント取得
        ExplanatoryText = GameObject.Find("ExplanatoryText").GetComponent<Text>();
        MyButton = GetComponent<Button>();
        Explanatory = "クラスネーム：" + name + '\n'+ Explanatory;
        if (Custom == null)
        {
            Custom = GameObject.Find("Custom");
            Custom.SetActive(false);
        }
        if (Select == null)
        {
            Select = GameObject.Find("Select");
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("in");
        ExplanatoryText.text = Explanatory;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("out");
        ExplanatoryText.text = "";
    }

    public void SelectCustom()
    {
        Custom.SetActive(true);
        Select.SetActive(false);
        Custom.GetComponent<Custom>().SetClass("");
    }
}