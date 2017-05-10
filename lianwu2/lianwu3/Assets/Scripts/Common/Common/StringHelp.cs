
namespace LoveDance.Client.Common
{
    public class StringHelp
    {

        // The class constructor is called when the class instance is created
        public StringHelp()
        {

        }
        public static bool DepartString(string curline, ref string key, ref string value, string split)
        {
            if (curline.Length > 0)
            {
                string[] result = curline.Split(split.ToCharArray());
                if (result.Length == 2)
                {
                    key = result[0];
                    value = result[1];
                    return true;
                }
            }
            return false;
        }
    }
}