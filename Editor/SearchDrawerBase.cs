using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.Experimental.GraphView;
using System.Reflection;
using System.Linq;

namespace LS.SearchWindows.Editor
{
    public abstract class SearchDrawerBase<SearchAttribute> : PropertyDrawer
        where SearchAttribute : SearchAttributeBase
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) 
        {
            var attrib = this.attribute as SearchAttribute;

            position.width -= attrib.buttonWidth;
            var fieldType = this.fieldInfo.FieldType;

            property.objectReferenceValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, fieldType, true);

            position.x += position.width;
            position.width = attrib.buttonWidth;

            if (GUI.Button(position, new GUIContent(attrib.label)))
            {
                Type t = GetType(
                    attrib, 
                    fieldType, 
                    property
                );
                SearchWindow.Open(
                    new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)),
                    GetProvider(t, property, attrib)
                );
            }
        }

        protected abstract SearchProviderBase GetProvider(Type t, SerializedProperty property, SearchAttribute attrib);

        public static Type GetType(SearchAttribute attribute, Type fieldType, SerializedProperty property) 
        {
            if (attribute.type != null)
                return attribute.type;
            if (attribute.typePropertyName != null)
            {
                var parent = GetParent(property);
                var typeProperty = GetValue(parent, attribute.typePropertyName);
                if (typeProperty != null)
                    if (typeProperty is Type)
                        return (Type)typeProperty;
                    else
                        Debug.LogError($"\"{attribute.typePropertyName}\" is not a Type property.");
                else    
                    Debug.LogError($"Couldn't find property \"{attribute.typePropertyName}\"");
            }
            return fieldType;
        }

        //Pulled from:
        //https://answers.unity.com/questions/425012/get-the-instance-the-serializedproperty-belongs-to.html
        #region Reflection Methods
        public static object GetParent(SerializedProperty prop)
        {
            var path = prop.propertyPath.Replace(".Array.data[", "[");
            object obj = prop.serializedObject.targetObject;
            var elements = path.Split('.');
            foreach(var element in elements.Take(elements.Length-1))
            {
                if(element.Contains("["))
                {
                    var elementName = element.Substring(0, element.IndexOf("["));
                    var index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[","").Replace("]",""));
                    obj = GetValue(obj, elementName, index);
                }
                else
                {
                    obj = GetValue(obj, element);
                }
            }
            return obj;
        }

        public static object GetValue(object source, string name)
        {
            if(source == null)
                return null;
            var type = source.GetType();
            var f = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if(f == null)
            {
                var p = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if(p == null)
                    return null;
                return p.GetValue(source, null);
            }
            return f.GetValue(source);
        }

        public static object GetValue(object source, string name, int index)
        {
            var enumerable = GetValue(source, name) as IEnumerable;
            var enm = enumerable.GetEnumerator();
            while(index-- >= 0)
                enm.MoveNext();
            return enm.Current;
        }
        
        #endregion

    }
}