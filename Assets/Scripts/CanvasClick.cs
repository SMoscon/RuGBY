using UnityEngine;
using System.Collections;

public class CanvasClick : MonoBehaviour {
	//public bool clicked;
	//public bool returnToMenuz;
	//public static CanvasClick Instance;
	public Animator animator;
	public Animator animatorColor;
	//public Button button;

	// Use this for initialization
	void Awake () {
		//Debug.Log ("are we doing this ro");
		//clicked = false;
		//returnToMenuz = false;
		//Instance = this;
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		//if (animator) {
			//AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo (0);
			//if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.Camera")) {
				/*if (clicked == true) {
				Debug.Log ("does satisfy");
					animator.SetBool ("Clicked", true);

				}*/
			//}
			//else if (stateInfo.nameHash == Animator.StringToHash ("Base Layer.CanvasFade")) {
				/*if (returnToMenuz == true) {
					animator.SetBool ("Return", true);
				}*/
			//}
		//}
		//button.onClick.AddListener (Clicked ());
	}
	


	public void Clicked()
	{
		animator.SetBool("TurnCamera", true);
		//Debug.Log("Clicked() called to " + clicked);
		animatorColor.SetBool("FadeButton", true);
	}

	public void Options() 
	{
		//animator.SetBool("TurnCamera", false);
		Debug.Log("Return to menu called");
		//animatorColor.SetBool("FadeButton", false);
	}

	public void ReturnToMenu() 
	{
		animator.SetBool("TurnCamera", false);
		//Debug.Log("Return to menu called to " + clicked);
		animatorColor.SetBool("FadeButton", false);
	}

}
