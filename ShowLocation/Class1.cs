using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using UnityEngine;
using HarmonyLib;


namespace ShowLocation
{
    [BepInPlugin("ShowLocation", "ShowLocation", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class ShowLocation : BaseUnityPlugin
    {

        private void Awake()
        {
            Debug.Log("ShowLocation Loaded");
            this.harmony.PatchAll();
        }

        // Token: 0x06000002 RID: 2 RVA: 0x0000206A File Offset: 0x0000026A
        private void OnDestroy()
        {
            this.harmony.UnpatchSelf();
        }

        private readonly Harmony harmony = new Harmony("ShowLocation");

        /*
        [HarmonyPatch(typeof(ZNet), "Awake")]
        public static class ZNetScene_Awake_Patch
        {
            public static void Postfix(ZNetScene __instance, ref bool ___m_publicReferencePosition)
            {
                List<Player> allPlayers = Player.GetAllPlayers();
                if (allPlayers != null)
			    {
                    foreach (Player player in allPlayers)
				    {
                        ___m_publicReferencePosition = true;
                    }
                }
                
            }
        }
        */
        public static class Client
        {
            public static ZPackage Serialize(string SteamID)
            {
                ZPackage zpackage = new ZPackage();

                zpackage.Write(Client.PositionEnforce);

                return zpackage;
            }

            public static bool PositionEnforce = true;

            public static void Deserialize(ZPackage data)
            {
                Client.PositionEnforce = data.ReadBool();
            }

            public static void RPC(ZRpc rpc, ZPackage data)
            {
                Debug.Log("S2C Client (RPC Call)");
                Debug.Assert(!ZNet.instance.IsServer());
                Client.Deserialize(data);
            }



        }

    }

}
