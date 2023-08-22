using UnityEngine;

namespace FazApp.Core
{
    public class MinValueAttribute : PropertyAttribute
    {
        public double MinValue { get; }

        public MinValueAttribute(double minValue)
        {
            MinValue = minValue;
        }
    }
}