using UnityEngine;
using System.Collections;

public class Back : MonoBehaviour {

    public GameObject now = null, next = null;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void NextObj()
    {
        now.SetActive(false);
        next.SetActive(true);
    }
}
