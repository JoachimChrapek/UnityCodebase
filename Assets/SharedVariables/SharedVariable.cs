using System;
using System.Collections.Generic;

namespace FazApp.SharedVariables
{
    [Serializable]
    public abstract partial class SharedVariable : ISharedVariable
    {
        internal abstract void Initialize();
    }
    
    [Serializable]
    public abstract partial class SharedVariable<T> : SharedVariable, ISharedVariable<T>
    {
        public event Action<T> valueChanged;

        protected T value;

        public T Value
        {
            get => value;
            set => TrySetNewValue(value);
        }

        protected virtual T InitialValue => default;
        
        internal sealed override void Initialize()
        {
            value = InitialValue;
        }

        protected virtual void TrySetNewValue(T newValue)
        {
            if (CanSetNewValue(newValue))
            {
                SetNewValue(newValue);
            }
        }

        protected virtual bool CanSetNewValue(T newValue)
        {
            return EqualityComparer<T>.Default.Equals(value, newValue) == false;
        }

        protected virtual void SetNewValue(T newValue)
        {
            value = newValue;
            NotifyValueChanged();
        }
        
        protected void NotifyValueChanged()
        {
            valueChanged?.Invoke(value);
        }
    }
}