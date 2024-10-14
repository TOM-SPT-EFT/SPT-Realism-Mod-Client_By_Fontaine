using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BloodMod
{
    public class Wound = _HealthPathcer()
    {
        public EDamageType BleedType { get; private set; }
        public string BodyPart { get; private set; }
        public float TimeSinceInjury { get; private set; } = 0f;
        public bool IsTreated { get; private set; } = false;
        public float WorseningThreshold { get; private set; }

        public Wound(EDamageType bleedType, string bodyPart)
        {
            BleedType = bleedType;
            BodyPart = bodyPart;
            WorseningThreshold = GetWorseningThreshold(bleedType);
        }

        public void UpdateWound(float deltaTime)
        {
            if (!IsTreated)
            {
                TimeSinceInjury += deltaTime;
                CheckForWorsening();
            }
        }

        private void CheckForWorsening()
        {
            if (TimeSinceInjury > WorseningThreshold)
            {
                WorsenWound();
            }
        }

        private float GetWorseningThreshold(EDamageType bleedType)
        {
            switch (bleedType)
            {
                case EDamageType.Scrape: return 60f;
                case EDamageType.MinorLaceration: return 45f;
                case EDamageType.Laceration: return 30f;
                case EDamageType.SevereLaceration: return 20f;
                default: return 9999f;
            }
        }

        private void WorsenWound()
        {
            switch (BleedType)
            {
                case EDamageType.Scrape:
                    BleedType = EDamageType.MinorLaceration;
                    Debug.Log($"{BodyPart} scrape worsened to minor laceration.");
                    break;
                case EDamageType.MinorLaceration:
                    BleedType = EDamageType.Laceration;
                    Debug.Log($"{BodyPart} minor laceration worsened to laceration.");
                    break;
                case EDamageType.Laceration:
                    BleedType = EDamageType.SevereLaceration;
                    Debug.Log($"{BodyPart} laceration worsened to severe laceration.");
                    break;
                default:
                    break;
            }
        }

        public void TreatWound()
        {
            IsTreated = true;
            Debug.Log($"{BodyPart} wound has been treated.");
        }
    }

    public class PlayerHealthManager
    {
        private List<Wound> wounds = new List<Wound>();

        public void ApplyDamage(EDamageType damageType, string bodyPart)
        {
            Wound existingWound = wounds.Find(w => w.BodyPart == bodyPart);

            if (existingWound != null)
            {
                existingWound.UpdateWound(1f);
            }
            else
            {
                wounds.Add(new Wound(damageType, bodyPart));
                Debug.Log($"New {damageType} injury to {bodyPart}");
            }
        }

        public void UpdateWounds(float deltaTime)
        {
            foreach (var wound in wounds)
            {
                wound.UpdateWound(deltaTime);
            }
        }

        public void TreatWound(string bodyPart)
        {
            Wound woundToTreat = wounds.Find(w => w.BodyPart == bodyPart);
            if (woundToTreat != null)
            {
                woundToTreat.TreatWound();
            }
        }

        public void TreatWoundWithBandage(string bodyPart, int bandageCount)
        {
            Wound woundToTreat = wounds.Find(w => w.BodyPart == bodyPart);
            if (woundToTreat == null)
            {
                Debug.Log("No wound found on this body part.");
                return;
            }

            switch (woundToTreat.BleedType)
            {
                case EDamageType.Scrape:
                    if (bandageCount >= 1)
                    {
                        woundToTreat.TreatWound();
                        Debug.Log("Scrape has been treated with one bandage.");
                    }
                    break;
                case EDamageType.MinorLaceration:
                    if (bandageCount >= 1)
                    {
                        woundToTreat.TreatWound();
                        Debug.Log("Minor laceration treated with one bandage.");
                    }
                    break;
                case EDamageType.Laceration:
                    if (bandageCount >= 2)
                    {
                        woundToTreat.TreatWound();
                        Debug.Log("Laceration treated with two bandages.");
                    }
                    else
                    {
                        Debug.Log("More bandages are required to treat this laceration.");
                    }
                    break;
                case EDamageType.SevereLaceration:
                    if (bandageCount >= 4)
                    {
                        woundToTreat.TreatWound();
                        Debug.Log("Severe laceration fully treated with four bandages.");
                    }
                    else
                    {
                        Debug.Log($"You need {4 - bandageCount} more bandages to fully treat the severe laceration.");
                    }
                    break;
                default:
                    Debug.Log("This type of injury cannot be treated with bandages.");
                    break;
            }
        }
    }
}
