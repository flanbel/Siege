using UnityEngine;
using System.Collections;

public class ScreenSplit : MonoBehaviour {
    public int num = 0;
	// Use this for initialization
	void Awake() {
        GameObject people = GameObject.Find("people");
        if (people)
        {
            num = (int)people.transform.localPosition.x;
            GameObject.Destroy(people);
        }
        else
            num = 4;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
