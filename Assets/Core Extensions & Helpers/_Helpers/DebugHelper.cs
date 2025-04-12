using System;
using UnityEngine;

namespace Core.Extensions
{
    #region Benchmark
    public static partial class Helper
    {
        public struct Benchmark
        {
            string name;
            System.DateTime startTime;
            public Benchmark(string name, System.Action runAction)
            {
                this.name = name;
                startTime = System.DateTime.Now;
                runAction?.Invoke();
                TimeSpan d = System.DateTime.Now - startTime;
                Debug.Log($"{name} : Time(ms) : {d.Ticks * 0.0001f}");
            }
        }
    }
    #endregion
}