﻿using System;
using System.Collections.Generic;
using System.Xml;
using PriestOfPlague.Source.Core;
using UnityEngine;

namespace PriestOfPlague.Source.Unit
{
    public class LineagesContainer : MonoBehaviour
    {
        private Dictionary <int, Lineage> _lineagesList;
        public Dictionary <int, Lineage> LineagesList => _lineagesList;

        public void LoadFromXML (XmlNode input)
        {
            _lineagesList.Clear ();
            foreach (var lineageNode in XmlHelper.IterateChildren (input, "lineage"))
            {
                var lineage = Lineage.LoadFromXML (lineageNode);
                _lineagesList [lineage.ID] = lineage;
            }
        }

        private void Start ()
        {
            _lineagesList = new Dictionary <int, Lineage> ();
        }
    }
}