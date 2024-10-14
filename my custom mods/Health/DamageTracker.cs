using UnityEngine;

namespace BloodMod
{
    public class DamageTracker : MonoBehaviour
    {
        // Method to determine bleeding type based on damage, velocity, mass, etc.
        public BloodTypes.EDamageType DetermineBleedingType(BloodTypes.EDamageType baseDamageType, float mass, float velocity, float distance, string bodyPart)
        {
            float kineticEnergy = (mass * velocity); // Simplified
            if (baseDamageType == BloodTypes.EDamageType.LightBleeding)
            {
                if (kineticEnergy < 0.5f && distance < 80f)
                {
                    return AssignLightBleedingType(bodyPart);
                }
            }
            else if (baseDamageType == BloodTypes.EDamageType.HeavyBleeding)
            {
                return AssignHeavyBleedingType(bodyPart);
            }
            return baseDamageType;
        }

        // Assigns specific light bleeding type based on body part
        private BloodTypes.EDamageType AssignLightBleedingType(string bodyPart)
        {
            switch (bodyPart.ToLower())
            {
                case "forearm":
                case "shin":
                    return BloodTypes.EDamageType.Scrape;
                case "neck":
                case "thigh":
                    return BloodTypes.EDamageType.Laceration;
                default:
                    return BloodTypes.EDamageType.MinorLaceration;
            }
        }

        // Assigns specific heavy bleeding type based on body part
        private BloodTypes.EDamageType AssignHeavyBleedingType(string bodyPart)
        {
            switch (bodyPart.ToLower())
            {
                case "neck":
                case "chest":
                    return BloodTypes.EDamageType.VenousLaceration;
                case "leg":
                    return BloodTypes.EDamageType.ArterialBleed;
                default:
                    return BloodTypes.EDamageType.CriticalBleed;
            }
        }
    }
}
