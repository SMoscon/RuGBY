using UnityEngine;
using System.Collections;

public class VitalBar : MonoBehaviour {
	private int _isPlayerHealthBar;

	// Use this for initialization
	void Start () {
		_isPlayerHealthBar = 0;

		//OnEnable();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// This method is called when the gameobject is enabled
	/*public void OnEnable() {
		if (_isPlayerHealthBar == 0){
			Messenger<float>.AddListener("Player 0 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 1){
			Messenger<float>.AddListener("Player 1 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 2){
			Messenger<float>.AddListener("Player 2 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 3){
			Messenger<float>.AddListener("Player 3 health changed", OnChangeHealthBarSize);
		}
	}

	// This method is called when the gameobject is disabled
	public void OnDisable() {
		if (_isPlayerHealthBar == 0){
			Messenger<float>.RemoveListener("Player 0 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 1){
			Messenger<float>.RemoveListener("Player 1 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 2){
			Messenger<float>.RemoveListener("Player 2 health changed", OnChangeHealthBarSize);
		}
		else if (_isPlayerHealthBar == 3){
			Messenger<float>.RemoveListener("Player 3 health changed", OnChangeHealthBarSize);
		}

	}*/

	public void OnChangeHealthBarSize(int curHealth, int maxHealth){

	}

	// Setting the healthbar to Player 0 - 3
	public void SetPlayerHealth(int b) {
		_isPlayerHealthBar = 0;
	}
}
