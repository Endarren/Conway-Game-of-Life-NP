using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace npScripts.Conway
{
	[Serializable]
	public struct CellData 
	{
		public int isAlive;
		public int minNeighborsToLive;
		public int maxNeighborsToLive;

		public int minNeighborsToBirth;
		public int maxNeighborsToBirth;
		public int livingNeighborCount;
		public void UpdateAlive (int livingNeighbors)
		{
			if (isAlive == 1)
			{
				if (livingNeighbors < minNeighborsToLive)
				{
					isAlive = 0;
					return;
				}
				if (livingNeighbors > maxNeighborsToLive)
				{
					isAlive = 0;
					return;
				}
			}
			else
			{
				if (livingNeighbors >= minNeighborsToBirth && livingNeighbors <= maxNeighborsToBirth)
				{
					isAlive = 1;
				}
			}
			//if(isAlive == 1)
			//{
			//	if(livingNeighbors >= minNeighborsToLive && livingNeighbors <= maxNeighborsToLive)
			//	{
			//		isAlive = 1;
			//	}
			//	else
			//	{
			//		isAlive = 0;
			//	}
			//}
			//else
			//{
			//	if (livingNeighbors >= minNeighborsToBirth && livingNeighbors <= maxNeighborsToBirth)
			//	{
			//		isAlive = 1;
			//	}
			//	else
			//	{
			//		isAlive = 0;
			//	}
			//}
		}
	}
}
