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
  public class BloodTypes : MonoBehaviour 
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
        public class Player
        {
            private const float TotalBloodVolume = 7000f; // Total blood volume in ml
            private const float MaxSevereLacerationBloodLossPercent = 20f; // Blood loss percentage for severe laceration
    
            public float CurrentBloodVolume { get; private set; } = TotalBloodVolume;
    
        // Method to handle treatment using bandages
            public void TreatLaceration(EDamageType damageType, int numberOfBandages)
            {
                float remainingBleed = GetBleedAmount(damageType);
    
            // Bandages stop an increasing percentage of bleeding
                for (int i = 1; i <= numberOfBandages; i++)
                {
                    float stopPercent = GetBandageStopPercent(i);
                    remainingBleed -= remainingBleed * stopPercent;
        
                    if (remainingBleed <= 0)
                    {
                        remainingBleed = 0;
                        break;
                    }
                }
                ApplyBleedingReduction(remainingBleed);
            }
            private float GetBleedAmount(EDamageType damageType)
                {
                    switch (damageType)
                    {
                    case EDamageType.Scrape:
                        return TotalBloodVolume * 0.025f;
                    case EDamageType.MinorLaceration:
                        return TotalBloodVolume * 0.05f;
                    case EDamageType.Laceration:
                        return TotalBloodVolume * 0.10f;
                    case EDamageType.SevereLaceration:
                        return TotalBloodVolume * 0.20f;
                    default:
                        return 0f;
                    }
                }
                private float GetBandageStopPercent(int bandageNumber)
                {
                    switch (bandageNumber)
                    {
                        case 1:
                            return 0.10f; // First bandage stops 10% of remaining bleed
                        case 2:
                            return 0.25f; // Second stops 25% of the remaining
                        case 3:
                            return 0.45f; // Third stops 45%
                        case 4:
                            return 0.20f; // Fourth bandage stops the rest
                        default:
                            return 0f;
                    }
                }
                private void ApplyBleedingReduction(float remainingBleed)
                {
            // Apply the reduced bleed amount to the player's current blood volume
                    CurrentBloodVolume -= remainingBleed;
    
                    if (CurrentBloodVolume <= 0)
                    {
                        HandlePlayerDeath();
                    }
                }
                private void HandlePlayerDeath()
                {
                    public static void player Player = IsYourPlayer()
                    {
                        if (Player.IsALive(bool(true)) return true;
                        else if (!Player.IsAlive = false) return false; break;
                    }
                    Debug.Log("Player has died due to severe blood loss.");
                }
            }
        }
    }
}
  
  
