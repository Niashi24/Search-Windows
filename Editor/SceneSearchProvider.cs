using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using Object = UnityEngine.Object;

namespace LS.SearchWindows.Editor
{
    public class SceneSearchProvider : SearchProviderBase
    {
        public override List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();

            PopulateSearchTree(list);

            if (list.Count == 1) return new List<SearchTreeEntry>() {new SearchTreeGroupEntry(new GUIContent("No Objects Found"))};

            return list;
        }

        private void PopulateSearchTree(List<SearchTreeEntry> list)
        {
            list.Add(new SearchTreeGroupEntry(new GUIContent("Select Object")));

            var foundObjects = FindObjectsInScene(assetType);
            foreach (var obj in foundObjects)
            {
                SearchTreeEntry entry;
                // if (obj is Component)
                    entry = new SearchTreeEntry(
                        new GUIContent($"{obj.name} ({obj.GetType().Name})", EditorGUIUtility.ObjectContent(obj, obj.GetType()).image)
                    );
                // else
                //     entry = null;
                entry.userData = obj;
                entry.level = 1; //level 1 is base layer
                list.Add(entry);
            }
        }

        private static List<Object> FindObjectsInScene(Type t)
        {
            if (t.IsInterface)
                return FindComponentsInScene(t).Cast<Object>().ToList();
            return FindObjectsOfType(t).ToList();
        }

        private static List<Component> FindComponentsInScene(Type t)
        {
            List<Component> objects = new List<Component>();
            GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootGameObject in rootGameObjects)
            {
                var components = rootGameObject.GetComponentsInChildren(t);

                foreach (var component in components)
                    objects.Add(component);
            }
            return objects;
        }
    }
}