using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LS.SearchWindows
{
    public abstract class SearchAttributeBase : PropertyAttribute
    {
        public Type type;
        public string typePropertyName;
        public string label;
        public int buttonWidth;
        public SearchAttributeBase(string label = "Search", int buttonWidth = 60)
        {
            this.label = label;
            this.buttonWidth = buttonWidth;
        }

        public SearchAttributeBase(string typePropertyName, string label = "Search", int buttonWidth = 60)
        {
            this.typePropertyName = typePropertyName;
            this.label = label;
            this.buttonWidth = buttonWidth;
        }

        public SearchAttributeBase(Type type, string label = "Search", int buttonWidth = 60)
        {
            this.type = type;
            this.label = label;
            this.buttonWidth = buttonWidth;
        }
    }
}