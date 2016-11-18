using UnityEngine;
using System.Collections;
/// <summary>
/// 装備の基底クラス
/// </summary>
public abstract class Fit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 装備使用
    /// </summary>
    public virtual void Use() { }
}
