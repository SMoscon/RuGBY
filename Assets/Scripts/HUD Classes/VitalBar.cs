using UnityEngine;
using System.Collections;

public class VitalBar : MonoBehaviour {
	private bool _isPlayerHealthBar;

	private int _maxBarLength;
	private int _curBarLength;
	private int _display;

	// Use this for initialization
	void Start () {
		_isPlayerHealthBar = true;

		//_display = gameObject.GetComponent<GUITexture>();
		//_maxBarLength = (int)_display.pixelInset.width;
		OnEnable();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// This method is called when the gameobject is enabled
	public void OnEnable() {
		if (_isPlayerHealthBar == true){
			//Messenger<int, int>.AddListener("Player health changed", OnChangeHealthBarSize);
		}/*
		else if (_isPlayerHealthBar == 1){
			Messenger<float>.AddListener("Player 1 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 2){
			Messenger<float>.AddListener("Player 2 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 3){
			Messenger<float>.AddListener("Player 3 health changed", OnChangeHealthBarSize);
		}*/
	}

	// This method is called when the gameobject is disabled
	public void OnDisable() {
		if (_isPlayerHealthBar == true){
			//Messenger<int, int>.RemoveListener("Player health changed", OnChangeHealthBarSize);
		}
		/*
		else if (_isPlayerHealthBar == 1){
			Messenger<float>.RemoveListener("Player 1 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 2){
			Messenger<float>.RemoveListener("Player 2 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 3){
			Messenger<float>.RemoveListener("Player 3 health changed", OnChangeHealthBarSize);
		}*/

	}

	public void OnChangeHealthBarSize(int curHealth, int maxHealth){
		Debug.Log ("We heard an event");
		//_curBarLength = (curHealth / maxHealth) * _maxBarLength;	
	}

	// Setting the healthbar to Player
	public void SetPlayerHealth(bool b) {
		//_isPlayerHealthBar == b;
	}
}
