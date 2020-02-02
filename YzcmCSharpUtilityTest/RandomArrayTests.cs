using Microsoft.VisualStudio.TestTools.UnitTesting;
using YzcmCSharpUtility;
using System;
using System.Linq;
using System.Collections.Generic;

namespace YzcmCSharpUtilityTest
{
    [TestClass]
    public class RandomArrayTests
    {
        [TestMethod]
        public void RandomArray_01()
        {
            var result = new List<string>();
            var testData = new string[] { "tokyo    ", "singapore", "usa      ", "france   ", "africa   " };
            var original = new RandomArray<string>(testData);
            for (int i = 0; i<100000; ++i)
            {
                var shuffled = original.Shuffle();

                string joined = string.Join(",", shuffled);
                result.Add(joined);
            }

            foreach(var x in result.GroupBy(x => x.Substring(0, 9)).OrderByDescending(x => x.Count()).Select(x => x.Key + " : " + x.Count()))
            {
                Console.WriteLine(x);
            }
        }
    }
}
