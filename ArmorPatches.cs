﻿using Aki.Reflection.Patching;
using Aki.Reflection.Utils;
using System;
using System.Reflection;
using System.Linq;
using EFT.InventoryLogic;
using System.Collections.Generic;
using static RealismMod.Attributes;

namespace RealismMod
{
    public class ArmorPatches
    {
        public class ArmorClassDisplayPatch : ModulePatch
        {

            private static Type _targetType;
            private static MethodInfo _method_4;

            public ArmorClassDisplayPatch()
            {
                _targetType = PatchConstants.EftTypes.Single(IsType);
                _method_4 = _targetType.GetMethod("method_4", BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Instance);
            }

            protected override MethodBase GetTargetMethod()
            {
                return _method_4;
            }

            public static bool IsType(Type type)
            {
                return type.GetField("armorComponent_0") != null && type.GetField("item") != null;
            }

            [PatchPrefix]
            private static bool Prefix(ref string __result, ref EFT.InventoryLogic.ArmorComponent ___armorComponent_0)
            {

                __result = ArmorProperties.ArmorClass(___armorComponent_0.Item);

                return false;
            }
        }
        public class ArmorComponentPatch : ModulePatch
        {

            protected override MethodBase GetTargetMethod()
            {
                return typeof(EFT.InventoryLogic.ArmorComponent).GetConstructor(new Type[] { typeof(Item), typeof(GInterface227), typeof(RepairableComponent), typeof(BuffComponent) });
            }


            [PatchPostfix]
            private static void PatchPostfix(ArmorComponent __instance)
            {
                List<ItemAttributeClass> bluntAtt = __instance.Item.Attributes;
                ItemAttributeClass bluntAttClass = new ItemAttributeClass(Attributes.ENewItemAttributeId.BluntThroughput);
                bluntAttClass.Name = ENewItemAttributeId.BluntThroughput.GetName();
                bluntAttClass.StringValue = () => Math.Round(__instance.BluntThroughput * 100).ToString() + " %";
                bluntAttClass.DisplayType = () => EItemAttributeDisplayType.Compact;
                bluntAtt.Add(bluntAttClass);
            }
        }
    }
}
