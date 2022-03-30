using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using UnityEditor.Experimental.GraphView;

namespace LS.SearchWindows.Editor
{
    [CustomPropertyDrawer(typeof(ObjectSearch))]
    public class ObjectSearchAttributeDrawer : SearchDrawerBase<ObjectSearch> 
    {
        protected override SearchProviderBase GetProvider(Type t, SerializedProperty property, ObjectSearch attrib)
        {
            return ScriptableObject.CreateInstance<JointSearchProvider>().Init(
                new (SearchProviderBase, string)[] {
                    (ScriptableObject.CreateInstance<SceneSearchProvider>().Init(t, property), "Scene"),
                    (ScriptableObject.CreateInstance<AssetSearchProvider>().Init(t, property, true), "Assets")
                },
                property
            );
        }
    }
}