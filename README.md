# UnityCodebase

A collection of tools to make it easier to create projects in Unity. A base package added to a new project, it has logic that is common to multiple projects.

## Modules

### Shared Variables [WIP]

A simple way to share simple data in an application. Each Shared Variable has a value that can be read at any time, plus it provides events when the value changes.

To create a new Shared Variable, create a class that inherits from <b>SharedVariable\<T\></b> - where T is the type of variable to be stored in the Shared Variable.
```
public class TestBoolVariable : SharedVariable<bool> { }
```

To use a Shared Variable in your application you need to invoke the SV.Get method
```
// read value
return SV.Get<TestBoolVariable>().Value;

// set new value
SV.Get<TestBoolVariable>().Value = newValue;

// subscribe to value changed event
SV.Get<TestIntVariable>().valueChanged += OnValueChanged;
```

#### Shared Variables Scriptable Objects

They provide Variables in the form of scriptable objects that can be used, for example, in the inspector.

Each such scriptable object has a value type that corresponds to the value type in the Shared Variable.

For example: 
```
public class TestIntVariable : SharedVariable<int> { }
``` 
can be made available in the editor in the form of 
```
public class StringSharedVariableScriptableObject : SharedVariableScriptableObject<string> { }
```
![image](https://github.com/JoachimChrapek/UnityCodebase/assets/33358648/e5f7c5bd-9a50-459d-943b-4e6098aed997)

This makes it possible to create components using an architecture based on scriptable objects and facilitates the work of non-technical people such as designers or artists.

For example, you may have several similar Shared Variables with an integer value - PlayerHealth, PlayerArmor and PlayerMana. 
Each of these can have a scriptable object, also with a value of type int. 
To display these values on the UI you don't need to create 3 different functions/components just one common controller:
```
public class IntText : MonoBehaviour
    {
        // here you can attach Health, Armor, Mana and everything with int value
        [SerializeField]
        private IntSharedVariableScriptableObject sharedVariableScriptableObject;

        [SerializeField]
        private Text uiTextComponent;
        
        private void Awake()
        {
            UpdateText(sharedVariableScriptableObject.Value);
            sharedVariableScriptableObject.valueChanged += UpdateText;
        }

        private void UpdateText(int newValue)
        {
            uiTextComponent.text = newValue.ToString();
        }
    }
```
This way, the person doing the UI does not have to ask the programmer to create the logic reading these values into the code.

#### Editor tools

This module provides an editor window in which Shared Variables can be managed. It has two variants for Editor when application is not playing and runtime.

##### Editor
When the application is not running, the Shared Variables created can be displayed. Here you can view and create scriptable objects for each Shared Variable.
![image](https://github.com/JoachimChrapek/UnityCodebase/assets/33358648/09459b84-2c76-47d6-80ae-a224f81c02d2)

Note that scritpable object can not be created for Vector2 - this is because no class corresponding to this type has been created. In this case, just add the following:
```
public class Vector2SharedVariableScriptableObject : SharedVariableScriptableObject<Vector2> { }
```
After that scriptable object can be created and used.

##### Runtime
When the application is launched, the Shared Variables window changes. Here, the values of the Shared Variables used can be viewed and forced to be changed. This makes it easier to test and debug the application.
![image](https://github.com/JoachimChrapek/UnityCodebase/assets/33358648/fa966487-2e8b-4dae-8fdf-89907d7fe769)

### Editor Extensions [WIP]

Provides logic and additional tools to extend Unity Editor
