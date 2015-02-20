using UnityEngine;
using System.Collections;

public class MenuAnimator : MonoBehaviour {
	public Animator animator;
	//public bool Clicked;
	//public bool Return;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//if (animator) {
			/*AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
			if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.CameraAnimation")) {*/
			//Debug.Log ("clicked: " + CanvasClick.Instance.clicked);
				//if (CanvasClick.Instance.clicked == true) {
				//		animator.SetBool ("TurnCamera", true);
				Debug.Log ("clicked");
				}
			//}
			//else if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.CameraTurn")) {
				//if (CanvasClick.Instance.returnToMenuz == true) {
				//		animator.SetBool ("TurnCamera", false);
				//Debug.Log ("hello");
				}
				
			//}
		//}
	
//	}
//}
