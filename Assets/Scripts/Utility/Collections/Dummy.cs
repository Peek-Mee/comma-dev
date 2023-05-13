

using System.Collections;
using UnityEngine;

namespace Comma.Utility.Collections
{
    public class Dummy
    {
        public static void VoidAction(object obj)
        {
            return;
        }
        
        public static IEnumerator DisableInputForSeconds(float time)
        {

            yield return new WaitForSeconds(time);
        }
    }
    
}
