using UnityEngine;
using UnityEditor;
using System;

namespace LS.SearchWindows.Editor
{
    [CustomPropertyDrawer(typeof(AssetSearch))]
    public class AssetSearchAttributeDrawer : SearchDrawerBase<AssetSearch>
    {
        protected override SearchProviderBase GetProvider(Type t, SerializedProperty property, AssetSearch attrib) 
            => ScriptableObject.CreateInstance<AssetSearchProvider>().Init(t, property, attrib.shortenPaths);
    }
}