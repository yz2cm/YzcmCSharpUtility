using System;
using System.Security.Cryptography;
using System.Linq;
using System.Collections.Generic;

namespace YzcmCSharpUtility
{
    public class RandomArray<T>
    {
        public RandomArray(IReadOnlyList<T> array)
        {
            this.array = array.ToArray();
            this.keys = new byte[this.array.Count()];
        }

        public IReadOnlyList<T> Shuffle()
        {
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(this.keys);
                Array.Sort(this.keys, this.array);

                return this.array;
            }
        }

        private T[] array;
        private byte[] keys;
    }
}
