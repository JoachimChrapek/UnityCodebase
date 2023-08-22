using FazApp.SharedVariables;
using UnityEngine;

namespace FazApp
{
    public class TestIntVariable : SharedVariable<int>
    {
        protected override int InitialValue => 69;
    }
    
    
    
    public class TestFloatVariable : SharedVariable<float> { }
    public class TestDoubleVariable : SharedVariable<double> { }
    public class TestStringVariable : SharedVariable<string> { }
    public class TestBoolVariable : SharedVariable<bool> { }
    
    public class TestVector2Variable : SharedVariable<Vector2> { }
    public class TestVector3Variable : SharedVariable<Vector3> { }
}
