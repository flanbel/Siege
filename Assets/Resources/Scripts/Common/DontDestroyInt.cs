using UnityEngine;
using System.Collections;
//破棄されない数字
public class DontDestroyInt : MonoBehaviour {
    public int num = 0;
	
	public void Create () {
        GameObject number = new GameObject();
        DontDestroyOnLoad(number);
        number.name = "people";
        number.transform.localPosition = new Vector3(num, num, num);
    }
}
