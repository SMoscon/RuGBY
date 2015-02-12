using UnityEngine;
using System.Collections;

public class ClimbingVolume : MonoBehaviour
{
	void OnTriggerEnter()
	{
		TP_Animator.Instance.SetClimbPoint(transform);
	}

	void OnTriggerExit()
	{
		TP_Animator.Instance.SetClimbPoint(transform);
	}
}