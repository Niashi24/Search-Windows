using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace LS.SearchWindows.Editor
{
    public class JointSearchProvider : SearchProviderBase
    {
        (SearchProviderBase,string)[] providers;
        
        public JointSearchProvider Init(IEnumerable<(SearchProviderBase, string)> providers, SerializedProperty serializedProperty)
        {
            this.providers = providers.ToArray();
            this.serializedProperty = serializedProperty;
            return this;
        }

        public override List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var entries = providers.Select(x => x.Item1.CreateSearchTree(context)).ToList();
            List<SearchTreeEntry> output = new List<SearchTreeEntry>();
            output.Add(new SearchTreeGroupEntry(new GUIContent("Select Object")));

            for (int i = 0; i < entries.Count; i++)
            {
                var (provider, name) = providers[i];
                output.Add(new SearchTreeGroupEntry(new GUIContent(name), 1));

                foreach(var item in entries[i])
                {
                    if (item.level == 0) continue;
                    item.level++;
                    output.Add(item);
                }
            }
            
            return output;
        }
    }
}