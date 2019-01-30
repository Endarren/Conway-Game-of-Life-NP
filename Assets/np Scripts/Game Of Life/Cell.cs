using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace npScripts.Conway
{
	public class Cell : MonoBehaviour
	{
		public SpriteRenderer rendering;
		public CellData cellData;
		public Color living = Color.green;
		public Color dead = Color.red;
		private void Awake()
		{
			cellData.isAlive = Random.Range(0, 2);
			UpdateCell(cellData);
		}
		public void UpdateCell(CellData cd)
		{
			cellData = cd;
			if (cellData.isAlive == 1)
				rendering.color = living;
			else
				rendering.color = dead;
		}
		private void OnMouseDown()
		{
			if (cellData.isAlive == 0)
				cellData.isAlive = 1;
			else
				cellData.isAlive = 0;
			if (cellData.isAlive == 1)
				rendering.color = living;
			else
				rendering.color = dead;
		}
	}
}
