using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

    NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if(Physics.Raycast(r,out hit))
            {
                agent.destination = hit.point;
                Debug.Log(hit.point);
            }
        }
	}
}
