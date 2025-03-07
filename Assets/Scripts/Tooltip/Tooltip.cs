using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.SceneManagement;

namespace CoduckStudio
{
    public class Tooltip
    {
        public static GenericTooltip.Config GetCardConfig(CardDefinition cardDefinition, ActionContainer actionContainer)
        {
            GenericTooltip.Config config = new GenericTooltip.Config(cardDefinition.name, Color.black, CardTypeConfiguration.i.TypeToColor(actionContainer.cardType));

            config.descriptions.Add(new GenericTooltip.ConfigDescription($"Energy cost:", Color.black, $"-{cardDefinition.energyCost} <sprite=0>"));
            config.descriptions.Add(new GenericTooltip.ConfigDescription($"Score gain:", Color.black, $"+{cardDefinition.score} <sprite=1>"));
            config.descriptions.Add(new GenericTooltip.ConfigDescription($"Energy gain:", Color.black, $"+{cardDefinition.energyGain} <sprite=0>"));

            return config;
        }

        public static GenericTooltip.Config GetRelicConfig(RelicDefinition relicDefinition)
        {
            GenericTooltip.Config config = new GenericTooltip.Config(relicDefinition.name, Color.black, Color.white);

            config.descriptions.Add(new GenericTooltip.ConfigDescription(relicDefinition.description, Color.black));

            return config;
        }

        public static string GetStatColor(int baseStat, int actualStat, Color defaultColor)
        {
            if (baseStat == -1) {
                return "#" + defaultColor.ToHexString();
            }

            if (actualStat < baseStat) {
                return "red";
            }
            else if (actualStat > baseStat) {
                return "green";
            }

            return "#" + defaultColor.ToHexString();
        }
    }
}
