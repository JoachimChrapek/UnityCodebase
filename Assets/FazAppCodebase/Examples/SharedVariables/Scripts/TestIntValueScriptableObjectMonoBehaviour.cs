using FazApp.EditorExtensions;
using FazApp.SharedVariables;
using UnityEngine;

namespace FazApp.Examples.SharedVariables
{
    public class TestIntValueScriptableObjectMonoBehaviour : MonoBehaviour
    {
        [SerializeField]
        private IntSharedVariableScriptableObject testIntVariable;
        
        [ReadOnly]
        public int TestIntValue;
        [Header("Use context menu to set new value")]
        public int NewValue;
        
        private void UpdateValue()
        {
            TestIntValue = testIntVariable.Value;
        }
        
        [ContextMenu("Set New Value")]
        private void SetValue()
        {
            testIntVariable.Value = NewValue;
        }
        
        private void OnValueChanged(int value)
        {
            UpdateValue();
        }
        
        private void Awake()
        {
            testIntVariable.valueChanged += OnValueChanged;
            UpdateValue();
        }
    }
}
