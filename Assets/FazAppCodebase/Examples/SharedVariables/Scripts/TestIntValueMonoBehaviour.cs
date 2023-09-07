using FazApp.EditorExtensions;
using FazApp.SharedVariables;
using UnityEngine;

namespace FazApp.Examples.SharedVariables
{
    public class TestIntValueMonoBehaviour : MonoBehaviour
    {
        [ReadOnly]
        public int TestIntValue;
        [Header("Use context menu to set new value")]
        public int NewValue;

        private void UpdateValue()
        {
            TestIntValue = SV.Get<TestIntVariable>().Value;
        }

        [ContextMenu("Set Value")]
        private void SetValue()
        {
            SV.Get<TestIntVariable>().Value = NewValue;
        }

        private void OnValueChanged(int value)
        {
            UpdateValue();
        }
    
        private void Awake()
        {
            UpdateValue();
            SV.Get<TestIntVariable>().valueChanged += OnValueChanged;
        }
    }
}