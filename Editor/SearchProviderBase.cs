using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;

namespace LS.SearchWindows.Editor
{
    public abstract class SearchProviderBase : ScriptableObject, ISearchWindowProvider
    {
        public Type assetType;
        public SerializedProperty serializedProperty;
        
        public abstract List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context);

        public virtual SearchProviderBase Init(Type assetType, SerializedProperty serializedProperty)
        {
            this.assetType = assetType;
            this.serializedProperty = serializedProperty;

            return this;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context) {
            serializedProperty.objectReferenceValue = (UnityEngine.Object)searchTreeEntry.userData;
            serializedProperty.serializedObject.ApplyModifiedProperties();
            return true;
        }
    }
}