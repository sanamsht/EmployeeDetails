using System.Text;

namespace WorkingWithMultipleTable_Prod.Utility
{
    public class Utilities
    {
        public static string EveryStringFirstCapital(string input)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(input))
            {
                var data = input.Split(' ');
                foreach (var i in data)
                {
                    sb.Append(i.First().ToString().ToUpper() + i.Substring(1) + " ");
                }
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}
