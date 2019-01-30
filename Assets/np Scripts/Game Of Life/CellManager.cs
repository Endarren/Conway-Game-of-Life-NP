using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
namespace npScripts.Conway
{
	public class CellManager : MonoBehaviour
	{
		public List<Cell> cells;
		public int rowSize;
		public int columnSize;
		NativeArray<CellData> cellData;
		NativeArray<CellData> cellData2;
		private CellLifeJob lifeJob;
		private JobHandle lifeHandler;
		public float updateEvery = 1f;
		float lastUpdate = 0;
		public bool isPaused;
		bool didUpdate = false;
		private void Start()
		{
			//lastUpdate = -updateEvery;
			cells = new List<Cell>(GetComponentsInChildren<Cell>());
			
		}
		public void SetPaused (bool b)
		{
			isPaused = b;
		}
		public void SetUpdateRate (string s)
		{
			float f = updateEvery;
			if (float.TryParse(s, out f))
			{
				updateEvery = f;
			}
		}
		private void Update()
		{
			if (lastUpdate + updateEvery <= Time.time && !isPaused)
			{
				List<CellData> cData = new List<CellData>();
				for (int i = 0; i < cells.Count; i++)
				{
					cData.Add(cells [i].cellData);
				}
				cellData = new NativeArray<CellData>(cData.ToArray(), Allocator.TempJob);
				cellData2 = new NativeArray<CellData>(cData.ToArray(), Allocator.TempJob);
				didUpdate = true;
				lastUpdate = Time.time;
				lifeJob = new CellLifeJob()
				{
					cells = cellData,
					otherCells = cellData2,
					rowSize = rowSize,
					columnSize = columnSize
				};
				lifeHandler = lifeJob.Schedule(cellData.Length, 64);
			}
		}
		private void LateUpdate()
		{
			if (didUpdate)
			{
				lifeHandler.Complete();
				didUpdate = false;
				for (int i = 0; i < cells.Count; i++)
				{
					cells [i].UpdateCell(cellData [i]);
				}
				if (cellData.IsCreated)
					cellData.Dispose();
				if (cellData2.IsCreated)
					cellData2.Dispose();
			}
		}
		private void OnDestroy()
		{
			lifeHandler.Complete();
			if (cellData.IsCreated)
				cellData.Dispose();
			if (cellData2.IsCreated)
				cellData2.Dispose();
		}
		private struct CellLifeJob : IJobParallelFor
		{
			public NativeArray<CellData> cells;
			[ReadOnly]
			public NativeArray<CellData> otherCells;
			public int rowSize;
			public int columnSize;
			public void Execute(int index)
			{
				int x = 0;
				int y = 0;
				x = index / rowSize;
				y = index % rowSize;
				int nX;
				int nY;
				int lifeCount = 0;
				int nIndex;
				int checkeds = 0;
				for (int a = -1; a < 2; a++)
				{
					for (int b = -1; b < 2; b++)
					{
						
						nX = x + a;
						nY = y + b;
						//if (nY != y && nX != x)
						{
							if (nY >= 0 && nX >= 0 && nX < rowSize && nY < columnSize)
							{
								//Debug.Log("[" + x + ", " + y + "] checks [" + nX + ", " + nY + "]");
								nIndex = nX * rowSize + nY;
								//checkeds++;
								if (otherCells [nIndex].isAlive == 1)
								{
									lifeCount++;
								}
							}
						}
					}
				}
				lifeCount -= cells [index].isAlive;
				//Debug.Log(checkeds);
				CellData cd = cells [index];
				cd.UpdateAlive(lifeCount);
				cd.livingNeighborCount = lifeCount;


				cells [index] = cd;
			}
		}
	}
}
