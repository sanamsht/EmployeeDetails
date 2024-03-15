using System.Text;


namespace WorkingWithMultipleTable_Prod.Utility
{
    public class Helper 
    {
        public static string encryptText(string C)
        {
            string result = string.Empty;
            byte[] passcode = Encoding.ASCII.GetBytes(C);
            result = Convert.ToBase64String(passcode);

            return result;
        }



        public static string decryptText(string c)
        {
            byte[] hashPassword = Convert.FromBase64String(c);
            string passcode = Encoding.ASCII.GetString(hashPassword);

            return passcode;
        }
    }
}
