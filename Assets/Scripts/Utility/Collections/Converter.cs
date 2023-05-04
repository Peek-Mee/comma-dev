using UnityEngine;

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
        /// <summary>
        /// Get the normalized value based on Min-Max normalization
        /// </summary>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <param name="value">Value to be converted</param>
        /// <returns>(float) normalized value</returns>
        public static float MinMaxNormalizer(float min, float max, float value)
        {
            float result;
            result = (value - min) / (max - min) * 1.0f;
            return result;
        }
        /// <summary>
        /// Convert number to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns>(bool) converted number</returns>
        public static bool NumToBool(float value)
        {
            return (Mathf.CeilToInt(value) >= 0);
        }
        /// <summary>
        /// Convert number to boolean
        /// </summary>
        /// <param name="value"></param>
        /// <returns>(bool) converted number</returns>
        public static bool NumToBool(int value)
        {
            return (Mathf.CeilToInt(value) >= 0);
        }
    }
}