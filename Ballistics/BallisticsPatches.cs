﻿using Aki.Reflection.Patching;
using EFT.InventoryLogic;
using EFT;
using System;
using System.Reflection;
using UnityEngine;
using Comfort.Common;
using System.Linq;
using HarmonyLib;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EFT.Ballistics;
using System.Drawing;
using BepInEx.Logging;
using UnityEngine.Rendering.PostProcessing;
using EFT.Quests;
using System.IO;
using HarmonyLib.Tools;
using System.Collections;
using EFT.Interactive;
using ShotClass = GClass2988;
using Diz.Skinning;
using EFT.Visual;

namespace RealismMod
{

    /*    public class SetSkinPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return typeof(PlayerBody).GetMethod("SetSkin", BindingFlags.Instance | BindingFlags.Public);
            }


            [PatchPostfix]
            private static void Prefix(PlayerBody __instance, KeyValuePair<EBodyModelPart, ResourceKey> part, Skeleton skeleton)
            {
                __instance.BodySkins[part.Key].Unskin();
            }
        }*/

    /*public class DamageInfoPatch : ModulePatch
      {
          protected override MethodBase GetTargetMethod()
          {
              return typeof(DamageInfo).GetConstructor(new Type[] { typeof(EDamageType), typeof(ShotClass) });
          }

          [PatchPrefix]
          private static void Prefix()
          {
              Logger.LogWarning("========Damage Info Prefix==========");
          }

          static UnityEngine.Color getColor(string colliderName) 
          {
              float opacity = Plugin.test2.Value;
              if (colliderName.Contains("SpineTopChest")) return new UnityEngine.Color(1, 0, 0, opacity);
              if (colliderName.Contains("SpineLowerChest")) return new UnityEngine.Color(0, 1, 0, opacity);
              if (colliderName.Contains("PelvisBack")) return new UnityEngine.Color(0, 0, 1, opacity);
              if (colliderName.Contains("SideChestDown")) return new UnityEngine.Color(0.5f, 1f, 0, opacity);
              if (colliderName.Contains("SideChestUp")) return new UnityEngine.Color(0, 0.5f, 1f, opacity);
              if (colliderName.Contains("HumanSpine2")) return new UnityEngine.Color(1f, 0f, 0.5f, opacity);
              if (colliderName.Contains("HumanSpine3")) return new UnityEngine.Color(1f, 1f, 1f, opacity);
              if (colliderName.Contains("HumanPelvis")) return new UnityEngine.Color(0.5f, 1f, 0.5f, opacity);
              if (colliderName.ToLower().Contains("leg")) return new UnityEngine.Color(0f, 0f, 1f, opacity);
              if (colliderName.ToLower().Contains("arm")) return new UnityEngine.Color(1f, 0f, 0f, opacity);
              if (colliderName.ToLower().Contains("eye")) return new UnityEngine.Color(0f, 1f, 0f, opacity);
              if (colliderName.ToLower().Contains("jaw")) return new UnityEngine.Color(0f, 1f, 0f, opacity);
              if (colliderName.ToLower().Contains("ear")) return new UnityEngine.Color(1f, 1f, 1f, opacity);
              if (colliderName.ToLower().Contains("head")) return new UnityEngine.Color(1f, 0f, 0f, opacity);
              return new UnityEngine.Color(0, 0, 1, opacity);
          }

          static void VisualizeSphereCollider(SphereCollider sphereCollider, string colliderName)
          {
              // Create a sphere primitive to represent the collider.
              GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

              // Disable the sphere's collider component.
              UnityEngine.Object.Destroy(sphere.GetComponent<Collider>());

              // Set the sphere's position to match the sphere collider.
              Transform colliderTransform = sphereCollider.transform;
              sphere.transform.position = colliderTransform.TransformPoint(sphereCollider.center);

              // Calculate the correct scale for the sphere. Unity's default sphere has a radius of 0.5 units.
              float actualScale = sphereCollider.radius / 0.5f;
              Vector3 scale = new Vector3(actualScale, actualScale, actualScale);

              // Apply global scale and additional scale factor if needed.
              sphere.transform.localScale = Vector3.Scale(colliderTransform.localScale, scale) * Plugin.test1.Value;

              // Set a transparent material to the sphere, so it doesn't obstruct the view.
              Material transparentMaterial = new Material(Shader.Find("Standard"));
              transparentMaterial.color = getColor(colliderName); // Set to desired semi-transparent color
              sphere.GetComponent<Renderer>().material = transparentMaterial;

              // Parent the sphere to the collider's GameObject to maintain relative positioning.
              sphere.transform.SetParent(colliderTransform, true);
          }

          static void VisualizeBoxCollider(BoxCollider boxCollider, string colliderName)
          {
              // Create a cube primitive to represent the collider.
              GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

              // Disable the cube's collider component.
              UnityEngine.Object.Destroy(cube.GetComponent<Collider>());

              // Set the cube's position and scale to match the box collider.
              Transform colliderTransform = boxCollider.transform;
              cube.transform.position = colliderTransform.TransformPoint(boxCollider.center);
              cube.transform.localScale = Vector3.Scale(colliderTransform.localScale, boxCollider.size) * Plugin.test1.Value;

              // Optionally, set the cube's rotation to match the collider's GameObject.
              cube.transform.rotation = colliderTransform.rotation;

              // Set a transparent material to the cube, so it doesn't obstruct the view.
              Material transparentMaterial = new Material(Shader.Find("Transparent/Diffuse"));
              transparentMaterial.color = getColor(colliderName); // Red semi-transparent
              cube.GetComponent<Renderer>().material = transparentMaterial;

              // Parent the cube to the collider's GameObject to maintain relative positioning.
              cube.transform.SetParent(colliderTransform, true);
          }

          static void VisualizeCapsuleCollider(CapsuleCollider capsuleCollider, string colliderName)
          {
              // Create a capsule primitive to represent the collider.
              GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);

              // Disable the capsule's collider component.
              UnityEngine.Object.Destroy(capsule.GetComponent<Collider>());

              // Set the capsule's position to match the capsule collider.
              Transform colliderTransform = capsuleCollider.transform;
              capsule.transform.position = colliderTransform.TransformPoint(capsuleCollider.center);

              // Calculate the correct scale for the capsule.
              float capsuleDefaultHeight = 2.0f; // Default Unity capsule height
              float capsuleDefaultRadius = 0.5f; // Default Unity capsule radius
              float actualScaleHeight = (capsuleCollider.height - 2 * capsuleCollider.radius) / capsuleDefaultHeight;
              float actualScaleRadius = capsuleCollider.radius / capsuleDefaultRadius;

              // Adjust the scale and rotation based on the collider's direction.
              Vector3 scale = Vector3.one;
              Quaternion rotation = Quaternion.identity;
              switch (capsuleCollider.direction)
              {
                  case 0: // x-axis
                      scale = new Vector3(actualScaleHeight, actualScaleRadius, actualScaleRadius);
                      rotation = Quaternion.Euler(0, 0, 90); // Rotate to align with x-axis
                      break;
                  case 1: // y-axis
                      scale = new Vector3(actualScaleRadius, actualScaleHeight, actualScaleRadius);
                      break;
                  case 2: // z-axis
                      scale = new Vector3(actualScaleRadius, actualScaleRadius, actualScaleHeight);
                      rotation = Quaternion.Euler(90, 0, 0); // Rotate to align with z-axis
                      break;
              }

              // Apply the rotation and scale.
              capsule.transform.rotation = colliderTransform.rotation * rotation;
              capsule.transform.localScale = Vector3.Scale(colliderTransform.localScale, scale) * Plugin.test1.Value;

              // Set a transparent material to the capsule, so it doesn't obstruct the view.
              Material transparentMaterial = new Material(Shader.Find("Transparent/Diffuse"));
              transparentMaterial.color = getColor(colliderName); // Red semi-transparent
              capsule.GetComponent<Renderer>().material = transparentMaterial;

              // Parent the capsule to the collider's GameObject to maintain relative positioning.
              capsule.transform.SetParent(colliderTransform, true);
          }

          [PatchPostfix]
          private static void PostFix(ref DamageInfo __instance, EDamageType damageType, ShotClass shot)
          {
  *//*            Logger.LogWarning(" base " + shot.HitCollider.GetType().BaseType);
              Logger.LogWarning(" type " + shot.HitCollider.GetType());
              Logger.LogWarning(" name " + shot.HitCollider.GetType().Name);*//*

              if (shot.HitCollider is BoxCollider) 
              {
                  VisualizeBoxCollider(shot.HitCollider as BoxCollider, __instance.HitCollider.name);
              }
              if (shot.HitCollider is CapsuleCollider)
              {
                  VisualizeCapsuleCollider(shot.HitCollider as CapsuleCollider, __instance.HitCollider.name);
              }
              if (shot.HitCollider is SphereCollider)
              {
                  VisualizeSphereCollider(shot.HitCollider as SphereCollider, __instance.HitCollider.name);
              }

              Logger.LogWarning("========Damage Info PostFix==========");
             *//* Logger.LogWarning("id " + shot.Ammo.Id);
              Logger.LogWarning("pen " + __instance.PenetrationPower);
              Logger.LogWarning("damage " + __instance.Damage);
              Logger.LogWarning("hit collider = " + __instance.HitCollider.name);
              Logger.LogWarning("ballistic collider = " + __instance.HittedBallisticCollider.name);*//*
              Logger.LogWarning("=================");
          }
      }*/



    public class IsShotDeflectedByHeavyArmorPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(Player).GetMethod("IsShotDeflectedByHeavyArmor", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static bool Prefix(bool __result)
        {
            __result = false;
            return false;
        }
    }

/*    public class IsPenetratedPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.Ballistics.BallisticCollider).GetMethod("IsPenetrated", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static void Prefix(EFT.Ballistics.BallisticCollider __instance, ShotClass shot, Vector3 hitPoint)
        {
            if (__instance.name == HitBox.LeftUpperArm || __instance.name == HitBox.RightUpperArm || __instance.name == HitBox.LeftForearm || __instance.name == HitBox.RightForearm)
            {
                __instance.PenetrationLevel = 10f;
                __instance.PenetrationChance = 0.5f;
            }
        }
    }*/

    public class DamageInfoPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(DamageInfo).GetConstructor(new Type[] { typeof(EDamageType), typeof(ShotClass) });
        }

        [PatchPrefix]
        private static bool Prefix(ref DamageInfo __instance, EDamageType damageType, ShotClass shot)
        {
            __instance.DamageType = damageType;
            __instance.Damage = shot.Damage;
            __instance.PenetrationPower = shot.PenetrationPower;
            __instance.HitCollider = shot.HitCollider;
            __instance.Direction = shot.Direction;
            __instance.HitPoint = shot.HitPoint;
            __instance.HitNormal = shot.HitNormal;
            __instance.HittedBallisticCollider = shot.HittedBallisticCollider;
            __instance.Player = shot.Player;
            __instance.Weapon = shot.Weapon;
            __instance.FireIndex = shot.FireIndex;
            if (__instance.DamageType == EDamageType.Blunt || __instance.DamageType == EDamageType.Bullet)
            {
                __instance.ArmorDamage = shot.VelocityMagnitude;
            }
            else
            {
                __instance.ArmorDamage = shot.ArmorDamage;
            }
            __instance.DeflectedBy = shot.DeflectedBy;
            __instance.BlockedBy = shot.BlockedBy;
            __instance.MasterOrigin = shot.MasterOrigin;
            __instance.IsForwardHit = shot.IsForwardHit;
            __instance.SourceId = shot.Ammo.TemplateId;

            BulletClass bulletClass;
            if ((bulletClass = (shot.Ammo as BulletClass)) != null)
            {
                __instance.StaminaBurnRate = bulletClass.StaminaBurnRate;
                __instance.HeavyBleedingDelta = bulletClass.HeavyBleedingDelta;
                __instance.LightBleedingDelta = bulletClass.LightBleedingDelta;
            }
            else
            {
                __instance.LightBleedingDelta = 0f;
                __instance.HeavyBleedingDelta = 0f;
                __instance.StaminaBurnRate = 0f;
                KnifeClass knifeClass;
                if ((knifeClass = (__instance.Weapon as KnifeClass)) != null)
                {
                    __instance.StaminaBurnRate = knifeClass.KnifeComponent.Template.StaminaBurnRate;
                }
            }
            __instance.DidBodyDamage = 0f;
            __instance.DidArmorDamage = 0f;
            __instance.OverDamageFrom = null;
            __instance.BodyPartColliderType = EBodyPartColliderType.None;
            __instance.BleedBlock = false;
            return false;
        }
    }

    public class ApplyDamageInfoPatch : ModulePatch
    {
        private static FieldInfo inventoryControllerField;
        private static PropertyInfo inventoryClassProperty;
        private static PropertyInfo equipmentClassProperty;

        private static List<ArmorComponent> preAllocatedArmorComponents = new List<ArmorComponent>(20);
        private static List<EBodyPart> bodyParts = new List<EBodyPart> { EBodyPart.RightArm, EBodyPart.LeftArm, EBodyPart.LeftLeg, EBodyPart.RightLeg, EBodyPart.Head, EBodyPart.Common, EBodyPart.Common };

        private static float GetBleedFactor(EBodyPart part)
        {
            switch (part)
            {
                case EBodyPart.Head:
                    return 0.4f;
                case EBodyPart.LeftLeg:
                case EBodyPart.RightLeg:
                    return 0.5f;
                case EBodyPart.LeftArm:
                case EBodyPart.RightArm:
                    return 0.25f;
                default:
                    return 1;
            }
        }

        private static void TryDoDisarm(Player player, float kineticEnergy, bool hitArmArmor, bool forearm)
        {
            Player.ItemHandsController itemHandsController = player.HandsController as Player.ItemHandsController;
            if (itemHandsController != null && itemHandsController.CurrentCompassState)
            {
                itemHandsController.SetCompassState(false);
                return;
            }

            if (player.MovementContext.StationaryWeapon == null && !player.HandsController.IsPlacingBeacon() && !player.HandsController.IsInInteractionStrictCheck() && player.CurrentStateName != EPlayerState.BreachDoor && !player.IsSprintEnabled)
            {
                int rndNumber = UnityEngine.Random.Range(0, 101);
                float kineticEnergyFactor = 1f + (kineticEnergy / 1000f);
                float hitArmArmorFactor = hitArmArmor ? 0.5f : 1f;
                float hitLocationModifier = forearm ? 1.5f : 1f;
                float totalChance = Mathf.Round(Plugin.DisarmBaseChance.Value * kineticEnergyFactor * hitArmArmorFactor * hitLocationModifier);

                if (rndNumber <= totalChance)
                {
                    InventoryControllerClass inventoryController = (InventoryControllerClass)inventoryControllerField.GetValue(player);
                    if (player.HandsController as Player.FirearmController != null)
                    {
                        Player.FirearmController fc = player.HandsController as Player.FirearmController;
                        if (fc.Item != null && inventoryController.CanThrow(fc.Item))
                        {
                            inventoryController.TryThrowItem(fc.Item, null, false);
                        }
                    }
                }
            }
        }

        private static void TryDoKnockdown(Player player, float kineticEnergy, bool bonusChance, bool isPlayer)
        {
            int rndNumber = UnityEngine.Random.Range(0, 101);
            float kineticEnergyFactor = 1f + (kineticEnergy / 1000f);
            float hitLocationModifier = bonusChance ? 2f : 1f;
            float totalChance = Mathf.Round(Plugin.FallBaseChance.Value * kineticEnergyFactor * hitLocationModifier);

            if (rndNumber <= totalChance)
            {
                player.ToggleProne();
                if ((isPlayer && Plugin.CanDisarmPlayer.Value) || (!isPlayer && Plugin.CanDisarmBot.Value))
                {
                    TryDoDisarm(player, kineticEnergy * 0.25f, false, false);
                }
            }
        }

        private static void playBodyHitSound(EBodyPart part, Vector3 pos, int rndNum)
        {
            float dist = CameraClass.Instance.Distance(pos);
            float volClose = Plugin.FleshHitSoundMulti.Value;
            float volDist = 4f * Plugin.FleshHitSoundMulti.Value;
            float distThreshold = 1f; //30f

            if (part == EBodyPart.Head)
            {
                Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips["headshot.wav"], dist, BetterAudio.AudioSourceGroupType.Impacts, 100, volClose * 0.6f, EOcclusionTest.Regular);
                return;
            }

            string audioClip = "flesh_dist_1.wav";
            if (dist >= distThreshold)
            {
                audioClip = rndNum == 0 ? "flesh_dist_1.wav" : rndNum == 1 ? "flesh_dist_2.wav" : "flesh_dist_2.wav";
            }
            else
            {
                audioClip = rndNum == 0 ? "flesh_1.wav" : rndNum == 1 ? "flesh_2.wav" : "flesh_3.wav";
            }

            Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips[audioClip], dist, BetterAudio.AudioSourceGroupType.Impacts, 100, dist >= distThreshold ? volDist : volClose, EOcclusionTest.Regular);
        }

        protected override MethodBase GetTargetMethod()
        {
            inventoryControllerField = AccessTools.Field(typeof(Player), "_inventoryController");
            inventoryClassProperty = AccessTools.Property(typeof(Player), "Inventory");
            equipmentClassProperty = AccessTools.Property(typeof(Player), "Equipment");

            return typeof(Player).GetMethod("ApplyDamageInfo", BindingFlags.Instance | BindingFlags.Public);
        }

        [PatchPrefix]
        private static void Prefix(Player __instance, ref DamageInfo damageInfo, EBodyPart bodyPartType)
        {
            Logger.LogWarning(damageInfo.HittedBallisticCollider.name);
            Logger.LogWarning(damageInfo.HittedBallisticCollider.TypeOfMaterial);
            Logger.LogWarning(damageInfo.BodyPartColliderType);

            if (__instance.IsYourPlayer && Plugin.RealHealthController.HasOverdosed && damageInfo.Damage > 10f) 
            {
                if (!__instance.IsInPronePose && Plugin.CanFellPlayer.Value)
                {
                    __instance.ToggleProne();
                }
                if (Plugin.CanDisarmPlayer.Value)
                {
                    TryDoDisarm(__instance, damageInfo.Damage * 50f, false, false);
                }
            }

            if (damageInfo.DamageType == EDamageType.Fall && damageInfo.Damage >= 15f)
            {
                if (!__instance.IsInPronePose) 
                {
                    __instance.ToggleProne();
                }
                if ((__instance.IsYourPlayer && Plugin.CanDisarmPlayer.Value) || (!__instance.IsYourPlayer && Plugin.CanDisarmBot.Value))
                {
                    TryDoDisarm(__instance, damageInfo.Damage * 50f, false, false);
                }
            }

            if (damageInfo.DamageType == EDamageType.Bullet || damageInfo.DamageType == EDamageType.Melee)
            {
                Logger.LogWarning("blunt or melee");

                EquipmentClass equipment = (EquipmentClass)equipmentClassProperty.GetValue(__instance);
                Inventory inventory = (Inventory)inventoryClassProperty.GetValue(__instance);

                bool hasArmArmor = false;
                bool hasLegProtection = false;
                int faceProtectionCount = 0;
                preAllocatedArmorComponents.Clear();
                inventory.GetPutOnArmorsNonAlloc(preAllocatedArmorComponents);
                ArmorComponent armor = null;
                foreach (ArmorComponent armorComponent in preAllocatedArmorComponents)
                {
                    if (armorComponent.Item.Id == damageInfo.BlockedBy || armorComponent.Item.Id == damageInfo.DeflectedBy)
                    {
                        armor = armorComponent;
                    }
                    if (armorComponent.Template.ArmorColliders.Any(x => BallisticsController.ArmCollidors.Contains(x))) 
                    {
                        hasArmArmor = true;
                    }
                    if (armorComponent.Template.ArmorColliders.Any(x => BallisticsController.LegSpallProtectionCollidors.Contains(x)))
                    {
                        hasLegProtection = true;
                    }
                    faceProtectionCount += armorComponent.Template.ArmorColliders.Count(x => BallisticsController.FaceSpallProtectionCollidors.Contains(x));
                }
                preAllocatedArmorComponents.Clear();

                if (!damageInfo.Blunt && Plugin.EnableHitSounds.Value && !__instance.IsYourPlayer)
                {
                    Logger.LogWarning("play sound");
                    playBodyHitSound(bodyPartType, damageInfo.HittedBallisticCollider.transform.position, UnityEngine.Random.Range(0, 2));
                }

                BallisticsController.ModifyDamageByHitZone(damageInfo.BodyPartColliderType, ref damageInfo);

                float KE = 1f;
                BulletClass ammo = null;
                AmmoTemplate ammoTemp = null;
                if (damageInfo.DamageType == EDamageType.Melee)
                {
                    Weapon weap = damageInfo.Weapon as Weapon;
                    bool isBayonet = !damageInfo.Player.IsAI && WeaponStats.HasBayonet && weap.WeapClass != "Knife" ? true : false;
                    float meleeDamage = isBayonet ? damageInfo.Damage : damageInfo.Damage * 2f;
                    KE = meleeDamage * 50f;
                }
                else
                {
                    ammoTemp = (AmmoTemplate)Singleton<ItemFactory>.Instance.ItemTemplates[damageInfo.SourceId];
                    ammo = new BulletClass("newAmmo", ammoTemp);
                    KE = (0.5f * ammo.BulletMassGram * damageInfo.ArmorDamage * damageInfo.ArmorDamage) / 1000f;
                }

                if (armor != null && damageInfo.DamageType != EDamageType.Melee)
                {
                    Logger.LogWarning("armor not null");

                    if (damageInfo.Blunt && GearStats.CanSpall(armor.Item) && (bodyPartType == EBodyPart.Chest || bodyPartType == EBodyPart.Stomach))
                    {
                        Logger.LogWarning("can do spalling");
                        damageInfo.BleedBlock = false;
                        bool isMetalArmor = armor.Template.ArmorMaterial == EArmorMaterial.ArmoredSteel || armor.Template.ArmorMaterial == EArmorMaterial.Titan ? true : false;
                        float bluntDamage = damageInfo.Damage;
                        float speedFactor = damageInfo.ArmorDamage / ammo.GetBulletSpeed;
                        float fragChance = ammo.FragmentationChance * speedFactor;
                        float lightBleedChance = damageInfo.LightBleedingDelta;
                        float heavyBleedChance = damageInfo.HeavyBleedingDelta;
                        float ricochetChance = ammo.RicochetChance * speedFactor;
                        float spallReduction = GearStats.SpallReduction(armor.Item);
                        float armorDamageActual = ammo.ArmorDamage * speedFactor;
                        float penPower = damageInfo.PenetrationPower;

                        float duraPercent = armor.Repairable.Durability / armor.Repairable.TemplateDurability;
                        float armorFactor = armor.ArmorClass * 10f * (Mathf.Min(1f, duraPercent * 2f));
                        float penDuraFactoredClass = Mathf.Max(1f, armorFactor - (penPower / 1.8f));
                        float penFactoredClass = Mathf.Max(1f, (armor.ArmorClass * 10f) - (penPower / 1.8f));
                        float maxPotentialDuraDamage = KE / penDuraFactoredClass;
                        float maxPotentialBluntDamage = KE / penFactoredClass;

                        float maxSpallingDamage = isMetalArmor ? maxPotentialBluntDamage - bluntDamage : maxPotentialDuraDamage - bluntDamage;
                        float factoredSpallingDamage = maxSpallingDamage * (fragChance + 1) * (ricochetChance + 1) * spallReduction * (isMetalArmor ? (1f - duraPercent) + 1f : 1f);
                        float splitSpallingDmg = factoredSpallingDamage / bodyParts.Count;

                        if (Plugin.EnableBallisticsLogging.Value)
                        {
                            Logger.LogWarning("===========SPALLING=============== ");
                            Logger.LogWarning("Spall Reduction " + spallReduction);
                            Logger.LogWarning("Dura Percent " + duraPercent);
                            Logger.LogWarning("Max Dura Factored Damage " + maxPotentialDuraDamage);
                            Logger.LogWarning("Max Blunt Damage " + maxPotentialBluntDamage);
                            Logger.LogWarning("Max Spalling Damage " + maxSpallingDamage);
                            Logger.LogWarning("Factored Spalling Damage " + factoredSpallingDamage);
                            Logger.LogWarning("Split Spalling Dmg " + splitSpallingDmg);
                        }

                        int rndNum = Mathf.Max(1, UnityEngine.Random.Range(1, bodyParts.Count + 1));
                        foreach (EBodyPart part in bodyParts.OrderBy(x => UnityEngine.Random.value).Take(rndNum))
                        {

                            if (part == EBodyPart.Common)
                            {
                                return;
                            }

                            float damage = splitSpallingDmg;
                            float bleedFactor = GetBleedFactor(part);

                            if (part == EBodyPart.Head)
                            {
                                float damageMulti = 1f - (faceProtectionCount / 5f);
                                damage = Mathf.Min(10, splitSpallingDmg * damageMulti);
                                bleedFactor = bleedFactor * damageMulti;
                            }

                            if ((part == EBodyPart.LeftArm || part == EBodyPart.RightArm) && hasArmArmor)
                            {
                                damage *= 0.5f;
                                bleedFactor *= 0.25f;
                            }

                            if ((part == EBodyPart.LeftLeg || part == EBodyPart.RightLeg) && hasLegProtection)
                            {
                                damage *= 0.5f;
                                bleedFactor *= 0.25f;
                            }

                            if (Plugin.EnableBallisticsLogging.Value)
                            {
                                Logger.LogWarning("Part Hit " + part);
                                Logger.LogWarning("Damage " + damage);
                                Logger.LogWarning("========================== ");
                            }
                            damageInfo.HeavyBleedingDelta = heavyBleedChance * bleedFactor;
                            damageInfo.LightBleedingDelta = lightBleedChance * bleedFactor;
                            __instance.ActiveHealthController.ApplyDamage(part, damage, damageInfo);
                        }
                    }
                }

                float hitPartHP = __instance.ActiveHealthController.GetBodyPartHealth(bodyPartType).Current;
                float toBeHP = hitPartHP - damageInfo.Damage;
                bool canDoKnockdown = !__instance.IsInPronePose && ((!__instance.IsYourPlayer && Plugin.CanFellBot.Value) || (__instance.IsYourPlayer && Plugin.CanFellPlayer.Value));
                bool canDoDisarm = ((!__instance.IsYourPlayer && Plugin.CanDisarmBot.Value) || (__instance.IsYourPlayer && Plugin.CanDisarmPlayer.Value));
                bool hitForearm = damageInfo.BodyPartColliderType == EBodyPartColliderType.LeftForearm || damageInfo.BodyPartColliderType == EBodyPartColliderType.RightForearm;
                bool hitCalf = damageInfo.BodyPartColliderType == EBodyPartColliderType.LeftCalf || damageInfo.BodyPartColliderType == EBodyPartColliderType.RightCalf;
                bool hitThigh= damageInfo.BodyPartColliderType == EBodyPartColliderType.LeftThigh || damageInfo.BodyPartColliderType == EBodyPartColliderType.RightThigh;

                if (hitForearm && toBeHP <= 0f && canDoDisarm)
                {
                    TryDoDisarm(__instance, KE, hasArmArmor, hitForearm);
                }

                bool doLegKnockdown = (hitCalf || hitThigh) && toBeHP <= 0f;
                bool doHeadshotKnockdown = bodyPartType == EBodyPart.Head && toBeHP > 0f && damageInfo.Damage >= 1;
                bool hasBonusChance = hitCalf || bodyPartType == EBodyPart.Head;

                if (canDoKnockdown && (doLegKnockdown || doHeadshotKnockdown))
                {
                    TryDoKnockdown(__instance, KE, hasBonusChance, __instance.IsYourPlayer);
                }

                ammo = null;
            }
        }
    }


    public class SetPenetrationStatusPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(ArmorComponent).GetMethod(nameof(ArmorComponent.SetPenetrationStatus), BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPrefix]
        private static bool Prefix(ShotClass shot, ArmorComponent __instance)
        {
            bool isSteelBodyArmor = __instance.Template.ArmorMaterial == EArmorMaterial.ArmoredSteel && !__instance.Template.ArmorColliders.Any(x => BallisticsController.HeadCollidors.Contains(x));
            if (__instance.Repairable.Durability <= 0f && !isSteelBodyArmor)
            {
                return false;
            }

            float penetrationPower = shot.PenetrationPower;
            float armorDuraPercent = __instance.Repairable.Durability / (float)__instance.Repairable.TemplateDurability * 100f;

            if (__instance.Template.ArmorMaterial == EArmorMaterial.ArmoredSteel)
            {
                armorDuraPercent = 100f;
            }
            else if (__instance.Template.ArmorMaterial == EArmorMaterial.Titan)
            {
                armorDuraPercent = Mathf.Min(100f, armorDuraPercent * 1.8f);
            }
            else
            {
                armorDuraPercent = Mathf.Min(100f, armorDuraPercent * 1.15f);
            }

            float armorResist = (float)Singleton<BackendConfigSettingsClass>.Instance.Armor.GetArmorClass(__instance.ArmorClass).Resistance;
            float secondaryArmorResist = armorResist <= 50 ? armorResist : Mathf.Clamp(armorResist - 40f, 35f, 45f);
            float armorFactor = (121f - 5000f / (45f + armorDuraPercent * 2f)) * armorResist * 0.01f;
            if (((armorFactor >= penetrationPower + 15f) ? 0f : ((armorFactor >= penetrationPower) ? (0.4f * (armorFactor - penetrationPower - 15f) * (armorFactor - penetrationPower - 15f)) : (100f + penetrationPower / (0.9f * armorFactor - penetrationPower)))) - shot.Randoms.GetRandomFloat(shot.RandomSeed) * 100f < 0f)
            {
                shot.BlockedBy = __instance.Item.Id;
                Debug.Log(">>> Shot blocked by armor piece");
                if (Plugin.EnableBallisticsLogging.Value)
                {
                    Logger.LogWarning("===========PEN STATUS=============== ");
                    Logger.LogWarning("Blocked");
                    Logger.LogWarning("========================== ");
                }
            }
            else
            {
                if (Plugin.EnableBallisticsLogging.Value == true)
                {
                    Logger.LogWarning("============PEN STATUS============== ");
                    Logger.LogWarning("Penetrated");
                    Logger.LogWarning("========================== ");
                }
            }

            return false;
        }
    }

    public class ApplyDamagePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(ArmorComponent).GetMethod(nameof(ArmorComponent.ApplyDamage), BindingFlags.Public | BindingFlags.Instance);
        }

        private static void playRicochetSound(Vector3 pos, int rndNum)
        {
            float dist = CameraClass.Instance.Distance(pos);
            string audioClip = rndNum == 0 ? "ric_1.wav" : rndNum == 1 ? "ric_2.wav" : "ric_3.wav";

            Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips[audioClip], dist, BetterAudio.AudioSourceGroupType.Impacts, 40, 4.25f, EOcclusionTest.Regular);
        }

        private static void playArmorHitSound(EArmorMaterial mat, Vector3 pos, bool isHelm, int rndNum)
        {
            float dist = CameraClass.Instance.Distance(pos);
            float volClose = 0.45f * Plugin.ArmorCloseHitSoundMulti.Value;
            float volDist = 4f * Plugin.ArmorFarHitSoundMulti.Value;
            float distThreshold = 30f;

            if (mat == EArmorMaterial.Aramid)
            {
                string audioClip = "aramid_1.wav";
                if (dist >= distThreshold)
                {
                    audioClip = rndNum == 0 ? "impact_dist_1.wav" : rndNum == 1 ? "impact_dist_2.wav" : "impact_dist_3.wav";
                }
                else
                {
                    if (!isHelm)
                    {
                        audioClip = rndNum == 0 ? "aramid_1.wav" : rndNum == 1 ? "aramid_2.wav" : "aramid_3.wav";
                    }
                    else
                    {
                        audioClip = rndNum == 0 ? "uhmwpe_1.wav" : rndNum == 1 ? "uhmwpe_2.wav" : "uhmwpe_3.wav";
                    }
                }

                Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips[audioClip], dist, BetterAudio.AudioSourceGroupType.Impacts, 100, dist >= distThreshold ? volDist : volClose, EOcclusionTest.Regular);
            }
            else if (mat == EArmorMaterial.Ceramic)
            {
                string audioClip = "ceramic_1.wav";
                if (dist >= distThreshold)
                {
                    audioClip = rndNum == 0 ? "ceramic_dist_1.wav" : rndNum == 1 ? "ceramic_dist_2.wav" : "ceramic_dist_3.wav";
                }
                else
                {
                    audioClip = rndNum == 0 ? "ceramic_1.wav" : rndNum == 1 ? "ceramic_2.wav" : "ceramic_3.wav";
                }

                Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips[audioClip], dist, BetterAudio.AudioSourceGroupType.Impacts, 100, dist >= distThreshold ? volDist : volClose, EOcclusionTest.Regular);
            }
            else if (mat == EArmorMaterial.UHMWPE || mat == EArmorMaterial.Combined)
            {
                string audioClip = "uhmwpe_1.wav";
                if (dist >= distThreshold)
                {
                    audioClip = rndNum == 0 ? "impact_dist_1.wav" : rndNum == 1 ? "impact_dist_2.wav" : "impact_dist_4.wav";
                }
                else
                {
                    audioClip = rndNum == 0 ? "uhmwpe_1.wav" : rndNum == 1 ? "uhmwpe_2.wav" : "uhmwpe_3.wav";
                }

                Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips[audioClip], dist, BetterAudio.AudioSourceGroupType.Impacts, 100, dist >= distThreshold ? volDist : volClose, EOcclusionTest.Regular);
            }
            else if (mat == EArmorMaterial.Titan || mat == EArmorMaterial.ArmoredSteel)
            {
                string audioClip = "metal_1.wav";
                if (dist >= distThreshold)
                {
                    audioClip = rndNum == 0 ? "metal_dist_1.wav" : rndNum == 1 ? "metal_dist_2.wav" : "metal_dist_3.wav";
                }
                else
                {
                    audioClip = rndNum == 0 ? "metal_1.wav" : rndNum == 1 ? "metal_2.wav" : "metal_3.wav";
                }

                Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips[audioClip], dist, BetterAudio.AudioSourceGroupType.Impacts, 100, dist >= distThreshold ? volDist * 0.5f : volClose * 0.75f, EOcclusionTest.Regular);
            }
            else if (mat == EArmorMaterial.Glass)
            {
                string audioClip = "glass_1.wav";
                if (dist >= distThreshold)
                {
                    audioClip = rndNum == 0 ? "impact_dist_3.wav" : rndNum == 1 ? "impact_dist_4.wav" : "impact_dist_2.wav";
                }
                else
                {
                    audioClip = rndNum == 0 ? "glass_1.wav" : rndNum == 1 ? "glass_2.wav" : "glass_3.wav";
                }
                Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips[audioClip], dist, BetterAudio.AudioSourceGroupType.Impacts, 100, dist >= distThreshold ? volDist : volClose, EOcclusionTest.Regular);
            }
            else
            {
                string audioClip = "impact_1.wav";
                if (dist >= distThreshold)
                {
                    audioClip = rndNum == 0 ? "impact_dist_4.wav" : rndNum == 1 ? "impact_dist_1.wav" : "impact_dist_3.wav";
                }
                else
                {
                    audioClip = rndNum == 0 ? "impact_1.wav" : rndNum == 1 ? "impact_2.wav" : "impact_3.wav";
                }
                Singleton<BetterAudio>.Instance.PlayAtPoint(pos, Plugin.LoadedAudioClips[audioClip], dist, BetterAudio.AudioSourceGroupType.Impacts, 100, dist >= distThreshold ? volDist : volClose, EOcclusionTest.Regular);
            }
        }

        [PatchPrefix]
        private static bool Prefix(ArmorComponent __instance, ref DamageInfo damageInfo, ref float __result, EBodyPartColliderType colliderType, EArmorPlateCollider armorPlateCollider, bool damageInfoIsLocal, List<ArmorComponent> armorComponents)
        {
            EDamageType damageType = damageInfo.DamageType;
            bool isHead = __instance.Template.ArmorColliders.Any(x => BallisticsController.HeadCollidors.Contains(x));
            bool isSteelBodyArmor = __instance.Template.ArmorMaterial == EArmorMaterial.ArmoredSteel && !isHead;
            bool roundPenetrated = damageInfo.BlockedBy != __instance.Item.Id && damageInfo.DeflectedBy != __instance.Item.Id;
            float speedFactor = 1f;
            float armorDamageActual = 1f;
            float KE = 1f;

            if (!damageType.IsWeaponInduced() && damageType != EDamageType.GrenadeFragment)
            {
                __result = 0f;
                return false;
            }

            Player player = (damageInfo.Player != null) ? Singleton<GameWorld>.Instance.GetAlivePlayerByProfileID(damageInfo.Player.iPlayer.ProfileId) : null;
            if (player != null)
            {
                __instance.TryShatter(player, damageInfoIsLocal);
            }

            if (__instance.Repairable.Durability <= 0f)
            {
                __result = 0f;
                return false;
            }

            if (damageType == EDamageType.Sniper || damageType == EDamageType.Landmine)
            {
                return true;
            }

            bool isPlayer = __instance.Item.Owner.ID.StartsWith("pmc") || __instance.Item.Owner.ID.StartsWith("scav");
            if (!isPlayer && Plugin.EnableHitSounds.Value) 
            {
                if (damageInfo.DeflectedBy == __instance.Item.Id)
                {
                    playRicochetSound(damageInfo.HittedBallisticCollider.transform.position, UnityEngine.Random.Range(0, 2));
                }
                else 
                {
                    playArmorHitSound(__instance.Template.ArmorMaterial, damageInfo.HittedBallisticCollider.transform.position, isHead, UnityEngine.Random.Range(0, 2));
                }
            }
            Logger.LogWarning("5");

            BulletClass ammo = null;
            AmmoTemplate ammoTemp = null;
            if (damageType != EDamageType.Melee)
            {
                ammoTemp = (AmmoTemplate)Singleton<ItemFactory>.Instance.ItemTemplates[damageInfo.SourceId];
                ammo = new BulletClass("newAmmo", ammoTemp);
                speedFactor = ammo.GetBulletSpeed / damageInfo.ArmorDamage;
                armorDamageActual = ammo.ArmorDamage * speedFactor;
                KE = (0.5f * ammo.BulletMassGram * damageInfo.ArmorDamage * damageInfo.ArmorDamage) / 1000f;
            }
            else
            {
                Weapon weap = damageInfo.Weapon as Weapon;
                bool isBayonet = !damageInfo.Player.IsAI && WeaponStats.HasBayonet && weap.WeapClass != "Knife" ? true : false;
                armorDamageActual = damageInfo.ArmorDamage;
                float meleeDamage = isBayonet ? damageInfo.Damage : damageInfo.Damage * 2f;
                KE = meleeDamage * 50f;
            }

            float bluntThrput = __instance.Template.BluntThroughput;
            float penPower = damageInfo.PenetrationPower;
            float duraPercent = __instance.Repairable.Durability / (float)__instance.Repairable.TemplateDurability;
            float armorResist = (float)Singleton<BackendConfigSettingsClass>.Instance.Armor.GetArmorClass(__instance.ArmorClass).Resistance;
            float secondaryArmorResist = armorResist <= 50 ? armorResist : Mathf.Clamp(armorResist - 40f, 35f, 45f);
            float armorDestructibility = Singleton<BackendConfigSettingsClass>.Instance.ArmorMaterials[__instance.Template.ArmorMaterial].Destructibility;
            float armorFactor = armorResist * (Mathf.Min(1f, duraPercent * 1.65f));
            float throughputDuraFactored = Mathf.Min(1f, bluntThrput * (1f + ((duraPercent - 1f) * -1f)));
            float penDuraFactoredClass = Mathf.Max(1f, armorFactor - (penPower / 1.8f));
            float penFactoredClass = Mathf.Max(1f, armorResist - (penPower / 1.8f));
            float maxPotentialDuraDamage = KE / penDuraFactoredClass;
            float maxPotentialBluntDamage = KE / penFactoredClass;
            float throughputFactoredDamage = Math.Min(damageInfo.Damage, maxPotentialDuraDamage * bluntThrput) * (armorDamageActual <= 2f ? 0.5f : 1f);
            float armorStatReductionFactor = Mathf.Max((1 - (penDuraFactoredClass / 100f)), 0.1f);

            if (damageInfo.DeflectedBy == __instance.Item.Id)
            {
                damageInfo.Damage *= 0.5f * armorStatReductionFactor;
                armorDamageActual *= 0.5f * armorStatReductionFactor;
                damageInfo.ArmorDamage *= 0.5f * armorStatReductionFactor;
                damageInfo.PenetrationPower *= 0.5f * armorStatReductionFactor;
            }

            if (__instance.Template.ArmorMaterial == EArmorMaterial.ArmoredSteel && !isHead)
            {
                throughputFactoredDamage = Math.Min(damageInfo.Damage, maxPotentialBluntDamage * bluntThrput) * (armorDamageActual <= 2f ? 0.1f : 1f);
            }
            if ((__instance.Template.ArmorMaterial == EArmorMaterial.ArmoredSteel || __instance.Template.ArmorMaterial == EArmorMaterial.Titan) && isHead)
            {
                armorDestructibility = 0.1f;
            }

            float durabilityLoss = 1f;
            if (damageType != EDamageType.Melee)
            {
                durabilityLoss = (maxPotentialBluntDamage / 24f) * Mathf.Clamp(ammo.BulletDiameterMilimeters / 7.62f, 1f, 2f) * armorDamageActual * armorDestructibility;
            }
            else
            {
                durabilityLoss = (maxPotentialBluntDamage / 24f) * Mathf.Clamp(9f / 7.62f, 1f, 2f) * armorDamageActual * armorDestructibility;
            }

            if (damageType == EDamageType.Melee)
            {
                if (damageInfo.PenetrationPower > armorFactor)
                {
                    if (Plugin.EnableBallisticsLogging.Value)
                    {
                        Logger.LogWarning("Melee Penetrated");
                    }
                    damageInfo.Damage *= armorStatReductionFactor;
                    damageInfo.PenetrationPower *= armorStatReductionFactor;
                }
                else
                {
                    if (Plugin.EnableBallisticsLogging.Value)
                    {
                        Logger.LogWarning("Melee Blocked");
                    }
                    if (!isHead)
                    {
                        damageInfo.Damage = throughputFactoredDamage + (damageInfo.Damage / 10f);
                    }
                    else
                    {
                        damageInfo.Damage = throughputFactoredDamage;
                    }

                    damageInfo.StaminaBurnRate = (throughputFactoredDamage / 100f) * 2f;
                    damageInfo.HeavyBleedingDelta = 0f;
                    damageInfo.LightBleedingDelta = 0f;

                }
            }
            else if (roundPenetrated)
            {
                durabilityLoss *= (1 - (penPower / 100f));
                damageInfo.Damage *= armorStatReductionFactor;
                damageInfo.PenetrationPower *= armorStatReductionFactor;
            }
            else
            {
                damageInfo.Damage = throughputFactoredDamage;
                damageInfo.StaminaBurnRate = (throughputFactoredDamage / 100f) * 2f;
            }

            if (!roundPenetrated)
            {
                damageInfo.HeavyBleedingDelta = 0f;
                damageInfo.LightBleedingDelta = 0f;
            }

            durabilityLoss = Math.Max(durabilityLoss, 0.05f);
            __instance.ApplyDurabilityDamage(durabilityLoss, armorComponents);
            __result = durabilityLoss;

            //BSG nullifies all damage on plate collidor hit for both penetration and blunt, to account for double hits
            //I do not believe this is necessary for blunt damge
            if (armorPlateCollider != (EArmorPlateCollider)0 && roundPenetrated)
            {
                damageInfo.Damage = 0f;
            }

            if (Plugin.EnableBallisticsLogging.Value)
            {
                Logger.LogWarning("===========ARMOR DAMAGE=============== ");
                Logger.LogWarning("KE " + KE);
                Logger.LogWarning("Pen " + penPower);
                Logger.LogWarning("Armor Damage " + armorDamageActual);
                Logger.LogWarning("Class " + armorResist);
                Logger.LogWarning("Throughput " + bluntThrput);
                Logger.LogWarning("Material Descructibility " + armorDestructibility);
                Logger.LogWarning("Dura percent " + duraPercent);
                Logger.LogWarning("Durability Loss " + durabilityLoss);
                Logger.LogWarning("Max potential blunt damage " + maxPotentialDuraDamage);
                Logger.LogWarning("Max potential dura damage " + maxPotentialBluntDamage);
                Logger.LogWarning("Throughput Facotred Damage " + throughputFactoredDamage);
                Logger.LogWarning("Damage " + damageInfo.Damage);
                Logger.LogWarning("========================== ");
            }
            ammo = null;
            ammoTemp = null;
            return false;
        }

    }

    public class CreateShotPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            var result = typeof(EFT.Ballistics.BallisticsCalculator).GetMethod("CreateShot", BindingFlags.Instance | BindingFlags.Public | BindingFlags.Public);

            return result;
        }

        [PatchPrefix]
        private static bool Prefix(EFT.Ballistics.BallisticsCalculator __instance, BulletClass ammo, Vector3 origin, Vector3 direction, int fireIndex, string player, Item weapon, ref ShotClass __result, float speedFactor, int fragmentIndex = 0)
        {
            int randomNum = UnityEngine.Random.Range(0, 512);
            float velocityFactored = ammo.InitialSpeed * speedFactor;
            float penChanceFactored = ammo.PenetrationChance * speedFactor;
            float damageFactored = ammo.Damage * speedFactor;
            float fragchanceFactored = Mathf.Max(ammo.FragmentationChance * speedFactor, 0);
            float penPowerFactored = EFT.Ballistics.BallisticsCalculator.GetAmmoPenetrationPower(ammo, randomNum, __instance.Randoms) * speedFactor;
            float bcFactored = Mathf.Max(ammo.BallisticCoeficient * speedFactor, 0.01f);

            __result = ShotClass.Create(ammo, fragmentIndex, randomNum, origin, direction, velocityFactored, velocityFactored, ammo.BulletMassGram, ammo.BulletDiameterMilimeters, (float)damageFactored, penPowerFactored, penChanceFactored, ammo.RicochetChance, fragchanceFactored, 1f, ammo.MinFragmentsCount, ammo.MaxFragmentsCount, EFT.Ballistics.BallisticsCalculator.DefaultHitBody, __instance.Randoms, bcFactored, player, weapon, fireIndex, null);
            return false;

        }
    }

    public class ApplyCorpseImpulsePatch : ModulePatch
    {
        private static FieldInfo lastDamField;
        private static FieldInfo corpseField;
        private static FieldInfo corpseAppliedForceField;

        protected override MethodBase GetTargetMethod()
        {
            lastDamField = AccessTools.Field(typeof(Player), "LastDamageInfo");
            corpseField = AccessTools.Field(typeof(Player), "Corpse");
            corpseAppliedForceField = AccessTools.Field(typeof(Player), "_corpseAppliedForce");

            return typeof(Player).GetMethod("ApplyCorpseImpulse", BindingFlags.Instance | BindingFlags.Public); ;
        }

        [PatchPrefix]
        private static bool Prefix(Player __instance)
        {
            DamageInfo lastDam = (DamageInfo)lastDamField.GetValue(__instance);
            Corpse corpse = (Corpse)corpseField.GetValue(__instance);

            float force;
            if (lastDam.DamageType == EDamageType.Bullet)
            {
                AmmoTemplate ammoTemp = (AmmoTemplate)Singleton<ItemFactory>.Instance.ItemTemplates[lastDam.SourceId];
                BulletClass ammo = new BulletClass("newAmmo", ammoTemp);
                float KE = ((0.5f * ammo.BulletMassGram * lastDam.ArmorDamage * lastDam.ArmorDamage) / 1000);
                force = (-Mathf.Max(1f, KE / 1000f)) * Plugin.RagdollForceModifier.Value;
                ammo = null;
                ammoTemp = null;
            }
            else if (lastDam.DamageType == EDamageType.Explosion)
            {
                force = 150f;
            }
            else
            {
                force = 5f;
            }

            corpseAppliedForceField.SetValue(__instance, force);
            corpse.Ragdoll.ApplyImpulse(lastDam.HitCollider, lastDam.Direction, lastDam.HitPoint, force);

            return false;
        }
    }
}
