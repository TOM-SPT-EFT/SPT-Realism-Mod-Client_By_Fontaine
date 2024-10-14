using Comfort.Common;
using EFT;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.UI.Health;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
using static EFT.HealthSystem.ActiveHealthController;
using static RealismMod.Attributes;
using Color = UnityEngine.Color;
using ExistanceClass = GClass2470;
using HealthStateClass = GClass2430<EFT.HealthSystem.ActiveHealthController.GClass2429>;
using MedUseStringClass = GClass1244;
using SetInHandsMedsInterface = GInterface142;

namespace BloodMod
{
  public class BloodTypes BleedTypes : MonoBehaviour 
  {
    public enum EDamageType
    {
        HeavyBleeding,
        LightBleeding,
        Fall,
        Barbed,
        Dehydration,
        Exhaustion,
        Poison,
        Melee,
        Explosion,
        Bullet,
        Blunt,
        Scrape,               // Light Bleed 1: Can worsen
        MinorLaceration,      // Light Bleed 2
        Laceration,           // Light Bleed 3
        SevereLaceration,     // Light Bleed 4
        VenousLaceration,     // Heavy Bleed 1
        ArterialBleed,        // Heavy Bleed 2
        CriticalBleed         // Heavy Bleed 3
    }
    public class DamageHandler
    {
        public EDamageType DetermineBleedingType(EDamageType baseDamageType, float mass, float velocity, float distance, string bodyPart)
        {
            float kineticEnergy = (mass * velocity) / distance;
  
            if (baseDamageType == EDamageType.LightBleeding)
            {
                if (kineticEnergy < 0.5f && distance < 80f)
                {
                    return AssignLightBleedingType(bodyPart);
                }
            }
            else if (baseDamageType == EDamageType.HeavyBleeding)
            {
                return AssignHeavyBleedingType(bodyPart);
            }
    
            return baseDamageType;
        }
  
        private EDamageType AssignLightBleedingType(string bodyPart)
        {
            switch (bodyPart.ToLower())
            {
                case "forearm":
                case "shin":
                    return EDamageType.Scrape;
                case "neck":
                case "thigh":
                    return EDamageType.Laceration;
                default:
                    return EDamageType.MinorLaceration;
            }
        }
    
        private EDamageType AssignHeavyBleedingType(string bodyPart)
        {
            switch (bodyPart.ToLower())
            {
                case "neck":
                case "chest":
                    return EDamageType.VenousLaceration;
                case "leg":
                    return EDamageType.ArterialBleed;
                default:
                    return EDamageType.CriticalBleed;
            }
        }
    }
}
  
  
