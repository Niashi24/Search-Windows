# Search-Windows
A collection of attributes for adding search windows in your Unity inspectors.

<br> This package includes three different attributes to use: [AssetSearch], [SceneSearch], and [ObjectSearch]
<br> AssetSearch is used to search your assets, SceneSearch is used to search within the current scene, and ObjectSearch is a combination of both.

<br> Use these attributes on your serialized object fields to add a button that opens a search window for your assets/scene objects based on the type.
This can be used with Generic Objects (which usually can't use the quick search on the normal object field!) and interfaces (though there are some drawbacks to this).
You can also use a property name for a type.

<br> The below examples use the SceneSearch attribute but the syntax is the same for all three.
## Examples
### General Usage
``` cs
//...
using UnityEngine;
using LS.SearchWindows;

public class Example : MonoBehavior
{
  [SerializeField]
  [AssetSearch] //Creates a search window button that filters for Sprites in Assets
  Sprite sprite;
}
```
### Generics
``` cs
  public class AssetVariable<T> : ScriptableObject 
  {
    public T Value;
  }

  public class Example : MonoBehavior
  {
    [SerializeField]
    [AssetSearch] //Creates a search button that filters for AssetVariable<int>'s in Assets 
    AssetVariable<int> test;
  }
```
### Interfaces
``` cs
  public interface ITest 
  {
    void Do();
  }
  
  public class TestBase : ScriptableObject, ITest
  {
    public override void Do() 
    {
      Debug.Log("TestBase");
    }
  }
  
  public class Example : MonoBehavior
  {
    [SerializeField]
    [AssetSearch(typeof(ITest)]
    ScriptableObject ITestObject;
    
    void Start()
    {
      (ITestObject as ITest)?.Do();
    }
  }
```
Note: this WON'T work because interfaces aren't serialized
```cs
  public class Example : MonoBehavior
  {
    [SerializeField]
    [AssetSearch]
    ITest ITestObject;
    
    //do stuff with ITestObject
  }
```
### Property
``` cs
  public class GenericReference<T>
  {
    [SerializeField]
    [AssetSearch(typePropertyName: nameof(ReferenceType))] //will get type from the property ReferenceType 
    Object _value;
    
    Type ReferenceType => typeof(T);
    
    public T Value
    {
      get => (T)_value;
      set => _value = (Object)value;
    }
  }
  
  public interface ITest 
  {
    void Do();
  }
  
  public class Example : MonoBehavior
  {
    [SerializeField]
    GenericReference<ITest> variable;
    
    void Start()
    {
      variable.Value.Do();
    }
  }
```
## Install
Use the Unity Package Manager (UPM) to add this project from Git URL.
