using PriestOfPlague.Source.Hubs;
using PriestOfPlague.Source.Items;
using PriestOfPlague.Source.Spells;
using PriestOfPlague.Source.Unit;
using UnityEngine;

namespace PriestOfPlague.Source.Core
{
    public static class GameEngineCoreUtils
    {
        public static void GetCoreInstances (
            out UnitsHub unitsHub,
            out ItemsRegistrator itemsRegistrator,
            out ItemTypesContainer itemsTypesContainer,
            out SpellsContainer spellsContainer,
            out CharacterModifiersContainer characterModifiersContainer,
            out LineagesContainer lineagesContainer
        )
        {
            var gameEngineCore = GameObject.FindGameObjectWithTag ("GameEngineCore");
            unitsHub = gameEngineCore.GetComponent <UnitsHub> ();
            itemsRegistrator = gameEngineCore.GetComponent <ItemsRegistrator> ();
            itemsTypesContainer = gameEngineCore.GetComponent <ItemTypesContainer> ();
            spellsContainer = gameEngineCore.GetComponent <SpellsContainer> ();
            characterModifiersContainer = gameEngineCore.GetComponent <CharacterModifiersContainer> ();
            lineagesContainer = gameEngineCore.GetComponent <LineagesContainer> ();
        }
    }
}