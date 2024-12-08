using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;

using Emericoude.Collections;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class StopwatchTester : MonoBehaviour
{
	public bool runOnstart = false;
	public int iterationCount = 100000;

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

		//write test here

		timer.Stop();
		UnityEngine.Debug.Log("Time taken for the operation (1): " + timer.ElapsedMilliseconds + " milliseconds");
	}

	[ContextMenu("Perform iteration 2")]
	public void PerformIteration_Two ()
	{
		Stopwatch timer = Stopwatch.StartNew();

		//write test here
		
		timer.Stop();
		UnityEngine.Debug.Log("Time taken for the operation (2): " + timer.ElapsedMilliseconds + " milliseconds");
	}
	
	[ContextMenu("Perform iteration 3")]
	public void PerformIteration_Three ()
	{
		Stopwatch timer = Stopwatch.StartNew();

		//write test here
		
		timer.Stop();
		UnityEngine.Debug.Log("Time taken for the operation (3): " + timer.ElapsedMilliseconds + " milliseconds");
	}
}
