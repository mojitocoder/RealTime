using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedServerClient
{
    public static class ExtensionUntils
    {
        public static T GetRandom<T>(this IEnumerable<T> sequence, Random random)
        {
            return sequence.ElementAt(random.Next(sequence.Count()));
        }
    }
}
