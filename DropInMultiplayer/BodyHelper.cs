﻿using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DropInMultiplayer
{
    public static class BodyHelper
    {
        private static readonly System.Random _rand = new System.Random();
        public static IEnumerable<SurvivorDef> _survivorBodies;

        public static IEnumerable<SurvivorDef> SurvivorBodies { 
            get
            {
                if (_survivorBodies == null)
                {
                    _survivorBodies = SurvivorCatalog.allSurvivorDefs
                        .Where(def => !def.hidden || (DropInMultiplayer.Instance.DropInConfig.AllowJoinAsHeretic && "Heretic".Equals(def.cachedName, StringComparison.InvariantCultureIgnoreCase)))
                        .ToArray();
                }
                return _survivorBodies;
            }
        }

        private static bool CompareNameStringsNoSpaces(string compareFrom, string compareTo)
        {
            compareFrom = compareFrom.Replace(" ", string.Empty);
            compareTo = compareTo.Replace(" ", string.Empty);
            return compareFrom.Equals(compareTo, StringComparison.InvariantCultureIgnoreCase);
        }

        public static SurvivorDef LookupSurvior(string name)
        {
            foreach (var survivor in SurvivorBodies)
            {
                var nameEqual = survivor.cachedName != null && CompareNameStringsNoSpaces(survivor.cachedName, name);
                var displayNameEqual = survivor.displayNameToken != null && CompareNameStringsNoSpaces(Language.GetString(survivor.displayNameToken), name);
                if (nameEqual || displayNameEqual)
                {
                    return survivor;
                }
            }

            return null;
        }

        public static IEnumerable<string> GetSurvivorDisplayNames()
        {
            return SurvivorBodies.Select(def => Language.GetString(def.displayNameToken).Replace(" ", string.Empty));
        }

        public static GameObject FindBodyPrefab(string characterName)
        {
            if (characterName.Equals("random", StringComparison.InvariantCultureIgnoreCase))
            {
                return SurvivorBodies.ElementAt(_rand.Next(SurvivorBodies.Count())).bodyPrefab;
            }

            return LookupSurvior(characterName)?.bodyPrefab;
        }
    }
}
