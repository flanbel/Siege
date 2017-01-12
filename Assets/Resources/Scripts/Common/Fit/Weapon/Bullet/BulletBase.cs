using UnityEngine;
using System.Collections;
using UnityEditor;
/// <summary>
/// 弾の基底クラス
/// </summary>
public abstract class BulletBase : MonoBehaviour {

    [System.Serializable]
    public class BulletState
    {
        //攻撃力()
        public float Power = 0.0f;
        //速度
        public float Speed = 0.0f;
        //進行方向
        public Vector3 Dir = Vector3.zero;
        //寿命
        public float Lifespan = 0.0f;
        public float Elapsed = 0.0f;
        //威力の変動
        public AnimationCurve Fluctuation;
    }

    public BulletState State;

    public void SetState(float power,Vector3 dir,Quaternion rot)
    {
        State.Power = power;
        State.Dir = dir;
        transform.localRotation = rot;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {

        transform.localPosition += State.Dir;
        //寿命の経過時間増加
        State.Elapsed += Time.deltaTime;
        //寿命を過ぎたなら
        if(State.Elapsed >= State.Lifespan)
        {
            //削除
            Destroy(gameObject);
        }

    }

    /* ---- ここから拡張コード ---- */
#if UNITY_EDITOR
    /**
     * Inspector拡張クラス
     */
    [CustomEditor(typeof(BulletBase))]               //!< 拡張するときのお決まりとして書いてね
    public class CharacterEditor : Editor           //!< Editorを継承するよ！
    {
        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
        }
    }
#endif
}
