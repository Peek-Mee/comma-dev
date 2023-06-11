using System;
using System.Collections;
using UnityEngine;

namespace Comma.Global.SaveLoad
{
    [Serializable]
    public class InputSaveData : ICloneable
    {
        [SerializeField] private string _bindingOverride;

        public InputSaveData()
        {
            _bindingOverride = "";
        }

        public string GetBindingOverride()
        {
            return _bindingOverride;
        }

        public void ChangeBindingOverride(string newBindingOverride)
        {
            _bindingOverride = newBindingOverride;
        }

        public object Clone()
        {
            return (InputSaveData)this.MemberwiseClone();
        }
    }
}