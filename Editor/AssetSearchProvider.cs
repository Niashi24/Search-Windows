using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;

namespace LS.SearchWindows.Editor
{
    public class AssetSearchProvider : SearchProviderBase
    {
        bool shortenPaths;

        public SearchProviderBase Init(
            Type assetType, 
            SerializedProperty serializedProperty,
            bool shortenPaths
        ) {
            this.shortenPaths = shortenPaths;
            return Init(assetType, serializedProperty);
        }

        public override List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> list = new List<SearchTreeEntry>();

            List<string> paths = GetPathsOfAssetsWithType(assetType);

            var basePath = shortenPaths ? (FindOverlappingPath(paths) + "/") : "";

            PopulateSearchTree(list, paths, basePath);

            if (list.Count == 1)
                return new List<SearchTreeEntry>() {new SearchTreeGroupEntry(new GUIContent("No Assets Found"))};

            return list;
        }

        private static void PopulateSearchTree(List<SearchTreeEntry> list, List<string> paths, string basePath)
        {
            //First element in List is the title of the search window
            list.Add(new SearchTreeGroupEntry(new GUIContent($"Select Asset - ({basePath})")));

            List<string> groups = new List<string>();
            for (int i = 0; i < paths.Count; i++)
            {
                string item = paths[i];
                item = Remove(item, basePath);

                string[] entryTitle = item.Split('/');
                string groupName = "";
                for (int j = 0; j < entryTitle.Length - 1; j++)
                {
                    groupName += entryTitle[j];
                    if (!groups.Contains(groupName))
                    {
                        list.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[j]), j + 1));
                        groups.Add(groupName);
                    }
                    groupName += "/";
                }

                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(basePath + item);
                if (obj is null) continue;
                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last(), EditorGUIUtility.ObjectContent(obj, obj.GetType()).image));
                entry.level = entryTitle.Length;
                entry.userData = obj;
                list.Add(entry);
            }
        }

        #region Loading Assets

        private static string[] GetAllAssets() => AssetDatabase.FindAssets("t:object", new string[] { "Assets" });

        private static List<string> GetPathsOfAssetsWithType(Type assetType) {
            string[] assetGuids = GetAllAssets();
            
            List<string> paths = new List<string>();
            foreach (string guid in assetGuids)
                paths.Add(AssetDatabase.GUIDToAssetPath(guid));

            paths = paths.Where(x => PathIsCorrectType(x, assetType)).OrderBy(x => x).ToList();
            return paths;
        }

        private static bool PathIsCorrectType(string path, Type type) {
            return AssetDatabase.LoadAssetAtPath(path,type) != null;
        }

        #endregion
        
        #region Shortening Path
        private static string FindOverlappingPath(List<string> paths)
        {
            if (paths.Count == 0) return "";
            if (paths.Count == 1)
            {
                var path = paths[0].Split('/');
                return Join(SubArray(path, 0, path.Length - 1), "/");
            }
            string[] simplestPath = paths[0].Split('/');

            for (int i = 1; i < paths.Count; i++)
            {
                string[] path = paths[i].Split('/');
                simplestPath = FindOverlap(simplestPath, path);
                if (simplestPath.Length == 0) return "";
            }

            return Join(simplestPath, "/");
        }

        private static string[] FindOverlap(string[] a, string[] b)
        {
            int maxIndex = FindMaxEqualItemIndex(a, b);
            if (maxIndex == 0) return new string[0];

            string[] output = new string[maxIndex];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = a[i];
            }

            return output;
        }

        private static int FindMaxEqualItemIndex(string[] a, string[] b)
        {
            int maxIndex = 0;
            while (maxIndex < a.Length && maxIndex < b.Length)
            {
                if (a[maxIndex] != b[maxIndex])
                    break;
                maxIndex++;
            }

            return maxIndex;
        }

        private static string Join(string[] s, string divider = "")
        {
            string message = "";
            for (int i = 0; i < s.Length; i++)
            {
                message += (i == 0 ? "" : divider) + s[i];
            }
            return message;
        }

        private static T[] SubArray<T>(T[] arr, int begin, int end)
        {
            if (end < begin) return new T[0];
            if (end >= arr.Length) Debug.LogError("end >= arr.length");

            T[] output = new T[end - begin];

            for (int i = begin; i < end; i++)
            {
                output[i - begin] = arr[i];
            }

            return output;
        }

        private static string Remove(string original, string key)
        {
            if (key == "") return original;
            var index = original.IndexOf(key);
            while (index != -1)
            {
                if (index == 0)
                    original = original.Substring(index + key.Length);
                else
                    original = original.Substring(0, index) + original.Substring(index + key.Length);
                index = original.IndexOf(key);
            }
            return original;
        }
        #endregion
    }
}