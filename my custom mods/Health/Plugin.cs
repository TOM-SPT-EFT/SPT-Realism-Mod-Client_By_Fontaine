using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace BloodMod
{
    // BepInEx Plugin Info
    [BepInPlugin("com.yourname.bloodmod", "Blood Mod", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private static Harmony harmonyInstance;

        // Plugin initialization
        private void Awake()
        {
            // Initialize Harmony for patching
            harmonyInstance = new Harmony("com.yourname.bloodmod");
            harmonyInstance.PatchAll();

            // Log that the mod is loaded
            Logger.LogInfo("Blood Mod has been loaded!");

            // Add the HealthEffects and DamageTracker components to the player
            // Assuming you want them attached to the player GameObject or similar
            GameObject player = FindLocalPlayer();
            if (player != null)
            {
                player.AddComponent<HealthEffects>();
                player.AddComponent<DamageTracker>();
                Logger.LogInfo("HealthEffects and DamageTracker components added to player.");
            }
            else
            {
                Logger.LogWarning("Player object not found. Cannot attach HealthEffects or DamageTracker.");
            }
        }

        // Plugin unload logic
        private void OnDestroy()
        {
            // Unpatch all Harmony patches
            if (harmonyInstance != null)
            {
                harmonyInstance.UnpatchAll(harmonyInstance.Id);
                Logger.LogInfo("Blood Mod patches have been removed.");
            }
        }

        // Example method to find the local player in the scene
        private GameObject FindLocalPlayer()
        {
            // Replace this with your game's specific method to find the player object
            // For example, in EFT or Tarkov-like games, you may need a more advanced player lookup
            return GameObject.FindWithTag("Player"); // Assuming the player object has the "Player" tag
        }
    }
}
