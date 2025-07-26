namespace ClassLibrary
{
    public static class StringExtension
    {
        public static string Invert(this string str)
        {
            string result = string.Empty;

            for(int index = str.Length - 1; index >= 0; index--)
            {
                result = result + str[index];
            }

            return result;
        }

        public static int CharCount(this string str, char character)
        {
            int count = 0;

            for(int index = 0; index < str.Length; index++)
            {
                if(str[index] == character)
                {
                    count = count + 1;
                }
            }

            return count;
        }
    }
}