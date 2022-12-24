using System;
using System.Collections;
using UnityEngine;

namespace Comma.Global.SaveLoad
{
    public class InputSaveData : ICloneable
    {
        public object Clone()
        {
            return (InputSaveData) this.MemberwiseClone();
        }
    }
}