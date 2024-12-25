using System.Diagnostics;
using UnityEngine;

namespace Emericoude.Tests
{
	public abstract class StopwatchTester : MonoBehaviour
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

			for (int i = 0; i < this.iterationCount; i++)
			{
				TestOne();
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
				TestTwo();
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
				TestThree();
			}
			
			timer.Stop();
			UnityEngine.Debug.Log("Time taken for the operation (3): " + timer.ElapsedMilliseconds + " milliseconds");
		}
		
		public abstract void TestOne();
		public abstract void TestTwo();
		public abstract void TestThree();
	}
}
