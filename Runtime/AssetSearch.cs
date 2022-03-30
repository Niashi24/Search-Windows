using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LS.SearchWindows
{
    public class AssetSearch : SearchAttributeBase
    {
        public bool shortenPaths;
        public AssetSearch(string label = "Search", int buttonWidth = 60, bool shortenPaths = true) : base(label, buttonWidth)
        {
            this.shortenPaths = shortenPaths;
        }

        public AssetSearch(string typePropertyName, string label = "Search", int buttonWidth = 60, bool shortenPaths = true) 
            : base(typePropertyName, label, buttonWidth)
        {
            this.shortenPaths = shortenPaths;
        }

        public AssetSearch(Type type, string label = "Search", int buttonWidth = 60, bool shortenPaths = true)
            : base(type, label, buttonWidth)
        {
            this.shortenPaths = shortenPaths;
        }
    }
}