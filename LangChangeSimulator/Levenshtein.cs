using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Colexification
{
    class Levenshtein
    {
        // From https://gist.github.com/wickedshimmy/449595/a17ab0d689623f5e6730eeb1c8606ab771149819
        public static int EditDistance(string original, string modified)
        {
            if (original == modified)
                return 0;

            int len_orig = original.Length;
            int len_diff = modified.Length;
            if(len_orig == 0 || len_diff == 0)
        
            return len_orig == 0 ? len_diff : len_orig;

            var matrix = new int[len_orig + 1, len_diff + 1];

            for (int i = 1; i <= len_orig; i++)
            {
                matrix[i, 0] = i;
                for (int j = 1; j <= len_diff; j++)
                {
                    int cost = modified[j - 1] == original[i - 1] ? 0 : 1;
                    if (i == 1)
                        matrix[0, j] = j;

                    var vals = new int[] {
                    matrix[i - 1, j] + 1,
                    matrix[i, j - 1] + 1,
                    matrix[i - 1, j - 1] + cost
                };
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }

        public static double WeightedDistance(string original, string modified, double[,] weights)
        {
            //Weighted Levenhstein distance. Assumes each char in the strings is an index to the weight array

            if (original == modified)
                return 0;

            int len_orig = original.Length;
            int len_diff = modified.Length;
            if (len_orig == 0 || len_diff == 0)

                return len_orig == 0 ? len_diff : len_orig;

            var matrix = new double[len_orig + 1, len_diff + 1];

            for (int i = 1; i <= len_orig; i++)
            {
                matrix[i, 0] = i;
                for (int j = 1; j <= len_diff; j++)
                {
                    double cost = modified[j - 1] == original[i - 1] ? 0 : weights[(int)modified[j-1],(int)original[i-1]];
                    if (i == 1)
                        matrix[0, j] = j;

                    var vals = new double[] {
                        matrix[i - 1, j] + 1,
                        matrix[i, j - 1] + 1,
                        matrix[i - 1, j - 1] + cost
                    };
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && original[i - 1] == modified[j - 2] && original[i - 2] == modified[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }

    }
}
