using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentUI.Demo.Shared.Models
{
    public static class LoremUtils
    {
        static string[] LOREM_IPSUM = (
          "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut " +
          "labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut " +
          "aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore " +
          "eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt " +
          "mollit anim id est laborum"
        ).Split(' ');

        public static string Lorem(int wordCount)
        {
            return string.Join(' ', LOREM_IPSUM.Take(wordCount));
        }

        public static string Lorem(int startWord, int wordCount)
        {
            return string.Join(' ', LOREM_IPSUM.Skip(startWord).Take(wordCount));
        }
    }
}
