using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

using Emericoude.Collections;
using UnityEngine.Serialization;

public class StopwatchTester : MonoBehaviour
{
	public bool runOnstart = false;
	public int iterationCount = 100000;
	
	private readonly int[] _testList = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

	void Start ()
	{
		if (!runOnstart) return;
		PerformIteration_One();
		PerformIteration_Two();
		PerformIteration_Three();
	}

	[ContextMenu("Perform iteration 1")]
	public void PerformIteration_One ()
	{
		Stopwatch timer = Stopwatch.StartNew();

		for (int i = 0; i < this.iterationCount; i++)
		{
			int random = _testList.GetRandomElement(_testList.Length);
		}

		timer.Stop();
		UnityEngine.Debug.Log("Time taken for the operation (1): " + timer.ElapsedMilliseconds + " milliseconds");
	}

	[ContextMenu("Perform iteration 2")]
	public void PerformIteration_Two ()
	{
		Stopwatch timer = Stopwatch.StartNew();

		for (int i = 0; i < this.iterationCount; i++)
		{
			int random = _testList.GetRandomElement();
		}

		timer.Stop();
		UnityEngine.Debug.Log("Time taken for the operation (2): " + timer.ElapsedMilliseconds + " milliseconds");
	}
	
	[ContextMenu("Perform iteration 3")]
	public void PerformIteration_Three ()
	{
		Stopwatch timer = Stopwatch.StartNew();

		for (int i = 0; i < this.iterationCount; i++)
		{
			//implement iteration here
		}

		timer.Stop();
		UnityEngine.Debug.Log("Time taken for the operation (3): " + timer.ElapsedMilliseconds + " milliseconds");
	}
}
