using System;

namespace FazApp.SharedVariables
{
    public interface ISharedVariable
    {

    }

    public interface ISharedVariable<T> : ISharedVariable
    {
        public event Action<T> valueChanged; 

        public T Value { get; set; }
    }
}
