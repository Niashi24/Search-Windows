using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.SearchWindows
{
    public class ObjectSearch : SearchAttributeBase
    {
        public ObjectSearch(string label = "Search", int buttonWidth = 60) : base(label, buttonWidth)
        {
            
        }

        public ObjectSearch(string typePropertyName, string label = "Search", int buttonWidth = 60) : base(typePropertyName, label, buttonWidth)
        {
            
        }

        public ObjectSearch(Type type, string label = "Search", int buttonWidth = 60) : base (type, label, buttonWidth)
        {
            
        }
    }
}