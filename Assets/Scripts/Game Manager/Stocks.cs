using UnityEngine;
using System.Collections;

public class Stocks : MonoBehaviour 
{
	private int stocks;

	void Awake() 
	{
		stocks = 3;
	}

	public void AdjustStocks(int stockAdjust)
	{
		stocks += stockAdjust;
	}

	public int GetStocks()
	{
		return stocks;
	}


}
