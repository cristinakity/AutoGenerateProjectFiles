namespace AutoGenerateProjectFiles.Console.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterUpper(this string value)
        {
            var arrayValue = value.ToArray();
            arrayValue[0] = arrayValue[0].ToString().ToUpper()[0];
            return new string(arrayValue);
        }
     }
}
