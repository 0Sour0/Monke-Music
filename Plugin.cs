using BepInEx;
using BepInEx;
using System;
using UnityEngine;
using Utilla;
using System.Reflection;
using System.IO;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using TMPLoader;

namespace MonkeMusic
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        TMP_Text disText;

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        string rootPath;
        public static string playerpath;
        public static string[] files;
        public static string[] fileName;
        public static string[] page;

        void OnGameInitialized(object sender, EventArgs e)
        {
            rootPath = Directory.GetCurrentDirectory();

            playerpath = Path.Combine(rootPath, "BepInEx", "Plugins", "MonkeMusic", "Songs");

            if (!Directory.Exists(playerpath))
            {
                Directory.CreateDirectory(playerpath);
            }

            files = Directory.GetFiles(playerpath);//cant Path.GetFileName - cant convert string[] to string
            foreach (var file in files)
            {
                Debug.Log(file);
            }
            fileName = new string[files.Length]; //creating new array with same length as files array
            for (int i = 0; i < fileName.Length; i++)
            {
                fileName[i] = Path.GetFileName(files[i]); //getting file names from directories
                Debug.Log("Found " + fileName[i] + " Song");
            }

            /* Code here runs after the game initializes (i.e. GorillaLocomotion.Player.Instance != null) */
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("MonkeMusic.Assets.dj");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            Debug.Log("loaded from str");
            GameObject asset = bundle.LoadAsset<GameObject>("dj");
            Instantiate(asset);
            Debug.Log("stand loaded");

            GameObject stand = GameObject.Find("dj_stand");
            stand.transform.position = new Vector3(-69.4201f, 2.9199f, -71.8201f);
            stand.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            GameObject left = GameObject.Find("DiskL");
            GameObject play = GameObject.Find("Play");
            GameObject right = GameObject.Find("DiskR");
            GameObject mute = GameObject.Find("mute");

            IntButtonToggle(mute);
            IntButton(left);
            IntButton(play);
            IntButton(right);

        }

        public void IntButton(GameObject button)
        {
            button.layer = 18;
            button.AddComponent<ButtonTrigger>();
            Debug.Log(button + "Initalised!");
        }
        public void IntButtonToggle(GameObject button)
        {
            button.layer = 18;
            button.AddComponent<ButtonToggle>();
            Debug.Log(button + "Initalised!");
        }

        bool rightflag = false;
        bool leftflag = false;
        bool playflag = false;
        bool muteflag = false;
        public int assignedIndex = 0;
        public int playerIndex = 0;
        void FixedUpdate()
        {
            disText = GameObject.Find("DisplayText").GetComponent<TMP_Text>();

            disText.text = fileName[playerIndex];

            

            GameObject left = GameObject.Find("DiskL");
            GameObject play = GameObject.Find("Play");
            GameObject right = GameObject.Find("DiskR");
            GameObject mute = GameObject.Find("mute");

            if (left.GetComponent<ButtonTrigger>().Pressed == true)
            {
                if (leftflag == true)
                {
                    leftflag = false;
                    playerIndex--;
                    if (playerIndex < 0)
                    {
                        playerIndex = fileName.Length - 1;//10 items but starts from 0 so 0 to 9 = 10 items
                    }


                    Debug.Log("Button Pressed");
                }
            }
            else
            {
                leftflag = true;
            }

            if (right.GetComponent<ButtonTrigger>().Pressed == true)
            {
                if (rightflag == true)
                {
                    rightflag = false;
                    playerIndex++;
                    if (playerIndex > fileName.Length - 1)
                    {
                        playerIndex = 0;
                    }


                    Debug.Log("Button Pressed");
                }
            }
            else
            {
                rightflag = true;
            }

            if (play.GetComponent<ButtonTrigger>().Pressed == true)
            {
                if (playflag == true)
                {
                    playflag = false;
                    AssetBundle songbundle = AssetBundle.LoadFromFile(Path.Combine(playerpath, fileName[playerIndex]));
                        GameObject assetsong = songbundle.LoadAsset<GameObject>(fileName[playerIndex]);
                            Instantiate(assetsong);
                    songbundle.Unload(false);
                    assignedIndex = playerIndex;
                            Debug.Log("Button Pressed");
                }
            }
            else
            {
                playflag = true;
            }

            if (mute.GetComponent<ButtonToggle>().Toggled == true)
            {
                if (muteflag == true)
                {
                    muteflag = false;
                    GameObject songToUnload = GameObject.Find(fileName[assignedIndex] + "(Clone)");
                    Destroy(songToUnload);

                    Debug.Log("Button Pressed");
                }
            }
            else
            {
                muteflag = true;
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
