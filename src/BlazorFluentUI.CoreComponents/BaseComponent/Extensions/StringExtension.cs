using System;
using System.Collections.Generic;
using System.Text;

namespace BlazorFluentUI
{
    public static class StringExtension
    {
        private static readonly Random rnd = new Random();

        public static string GetRandomHashCodeString(this string str)
        {
            uint i = (uint)rnd.Next();
            return i.ConvertToBase64Arithmetic();
        }

        public static string ConvertToBase64Arithmetic(this uint i)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            uint length = (uint)alphabet.Length;
            StringBuilder sb = new StringBuilder();
            int pos = 0;
            do
            {
                sb.Append(alphabet[(int)(i % length)]);
                i /= length;
                pos++;
                if (pos == 4)
                {
                    pos = 0;
                    if (i != 0)
                    {
                        sb.Append('-');
                    }
                }
            } while (i != 0);
            return sb.ToString();
        }
    }
}
