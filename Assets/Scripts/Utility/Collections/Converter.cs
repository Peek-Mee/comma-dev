namespace Comma.Utility.Collections
{
    public class Converter
    {
        /// <summary>
        /// Convert bit mask into an integer layer value
        /// </summary>
        /// <param name="bitMask">integer</param>
        /// <returns>Layer value in integer</returns>
        public static int BitToLayer(int bitMask)
        {
            int res = bitMask > 0 ? 0 : 31;
            while(bitMask > 1)
            {
                bitMask >>= 1;
                res++;
            }
            return res;
        }
        
    }
}