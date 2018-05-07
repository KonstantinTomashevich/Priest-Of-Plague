using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using UnityEditor.Animations;
using UnityEngine;

namespace PriestOfPlague.Source.Core
{
	public class XmlHelper
	{
		public static bool HasAttribute (XmlNode xml, string attributeName)
		{
			return xml.Attributes [attributeName] != null;
		}

		public static float GetFloatAttribute (XmlNode xml, string attributeName)
		{
			return float.Parse (xml.Attributes [attributeName].InnerText, NumberFormatInfo.InvariantInfo);
		}

		public static int GetIntAttribute (XmlNode xml, string attributeName)
		{
			return int.Parse (xml.Attributes [attributeName].InnerText, NumberFormatInfo.InvariantInfo);
		}

		public static uint GetUIntAttribute (XmlNode xml, string attributeName)
		{
			return uint.Parse (xml.Attributes [attributeName].InnerText, NumberFormatInfo.InvariantInfo);
		}

		public static bool GetBoolAttribute (XmlNode xml, string attributeName)
		{
			return bool.Parse (xml.Attributes [attributeName].InnerText);
		}

		public static Vector2 GetVector2Attribute (XmlNode xml, string attributeNameX, string attributeNameY)
		{
			return new Vector2 (GetFloatAttribute (xml, attributeNameX), GetFloatAttribute (xml, attributeNameY));
		}

		public static Rect GetRectAttribute (XmlNode xml, string attributeName)
		{
			return Rect.MinMaxRect (GetFloatAttribute (xml, attributeName + "X0"), GetFloatAttribute (xml, attributeName + "Y0"),
				GetFloatAttribute (xml, attributeName + "X1"), GetFloatAttribute (xml, attributeName + "Y1"));
		}

		public static IEnumerable <XmlNode> IterateChildren (XmlNode node, string tag)
		{
			foreach (XmlNode child in node.ChildNodes)
			{
				if (child.LocalName == tag)
				{
					yield return child;
				}
			}
		}
		
		public static XmlNode FirstChild (XmlNode node, string tag)
		{
			return IterateChildren (node, tag).FirstOrDefault ();
		}
	}
}
