using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace PriestOfPlague.Source.Ingame
{
	public class UnitSpawner : MonoBehaviour
	{
		public TextAsset XmlDocumentText;

		IEnumerator Start ()
		{
			yield return null;
			var document = new XmlDocument ();
			document.LoadXml (XmlDocumentText.text);

			var unit = gameObject.AddComponent <Unit.Unit> ();
			yield return null;
			unit.LoadFromXML (document.GetElementsByTagName ("unit") [0]);
		}
	}
}
