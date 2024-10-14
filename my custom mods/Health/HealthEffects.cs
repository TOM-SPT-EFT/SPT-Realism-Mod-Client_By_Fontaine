using UnityEngine;

namespace BloodMod
{
    public class HealthEffects : MonoBehaviour
    {
        private const float TotalBloodVolume = 7000f; // Total blood volume in ml
        private const float MaxSevereLacerationBloodLossPercent = 20f;

        public float CurrentBloodVolume { get; private set; } = TotalBloodVolume;

        // Method to handle treatment using bandages
        public void TreatLaceration(BloodTypes.EDamageType damageType, int numberOfBandages)
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

        // Get the total blood loss based on the bleeding type
        private float GetBleedAmount(BloodTypes.EDamageType damageType)
        {
            switch (damageType)
            {
                case BloodTypes.EDamageType.Scrape:
                    return TotalBloodVolume * 0.025f;
                case BloodTypes.EDamageType.MinorLaceration:
                    return TotalBloodVolume * 0.05f;
                case BloodTypes.EDamageType.Laceration:
                    return TotalBloodVolume * 0.10f;
                case BloodTypes.EDamageType.SevereLaceration:
                    return TotalBloodVolume * MaxSevereLacerationBloodLossPercent / 100;
                default:
                    return 0f;
            }
        }

        // Percentage of bleeding stopped by each bandage application
        private float GetBandageStopPercent(int bandageNumber)
        {
            switch (bandageNumber)
            {
                case 1:
                    return 0.10f; // First bandage stops 10%
                case 2:
                    return 0.25f; // Second bandage stops 25%
                case 3:
                    return 0.45f; // Third bandage stops 45%
                case 4:
                    return 0.20f; // Fourth bandage stops the rest
                default:
                    return 0f;
            }
        }

        // Apply the reduced bleed amount to the player's current blood volume
        private void ApplyBleedingReduction(float remainingBleed)
        {
            CurrentBloodVolume -= remainingBleed;

            if (CurrentBloodVolume <= 0)
            {
                HandlePlayerDeath();
            }
        }

        // Handles player death when blood volume reaches zero
        private void HandlePlayerDeath()
        {
            Debug.Log("Player has died due to severe blood loss.");
        }
    }
}
