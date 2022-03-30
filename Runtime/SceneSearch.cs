using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LS.SearchWindows
{
    public class SceneSearch : SearchAttributeBase
    {
        public SceneSearch(string label = "Search", int buttonWidth = 60) : base(label, buttonWidth)
        {
            
        }

        public SceneSearch(string typePropertyName, string label = "Search", int buttonWidth = 60) : base(typePropertyName, label, buttonWidth)
        {
            
        }

        public SceneSearch(Type type, string label = "Search", int buttonWidth = 60) : base(type, label, buttonWidth)
        {
            
        }
    }
}