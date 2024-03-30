using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Emeric.Utilities;

using UnityEngine;

public class StopwatchTester : MonoBehaviour
{
	int[] testList = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
	public bool runOnstart = false;
	public int IterationCount = 100000;

	void Start ()
	{
		if (!runOnstart) return;
		PerformIteration_One();
		PerformIteration_Two();
	}

	[ContextMenu("Perform iteration 1")]
	public void PerformIteration_One ()
	{
		Stopwatch timer = Stopwatch.StartNew();

		for (int i = 0; i < this.IterationCount; i++)
		{
			int random = testList.GetRandomElement(testList.Length);
		}

		timer.Stop();
		UnityEngine.Debug.Log("Time taken for the operation: " + timer.ElapsedMilliseconds + " milliseconds");
	}

	[ContextMenu("Perform iteration 2")]
	public void PerformIteration_Two ()
	{
		Stopwatch timer = Stopwatch.StartNew();

		for (int i = 0; i < this.IterationCount; i++)
		{
			int random = testList.GetRandomElement();
		}

		timer.Stop();
		UnityEngine.Debug.Log("Time taken for the operation: " + timer.ElapsedMilliseconds + " milliseconds");
	}
}
