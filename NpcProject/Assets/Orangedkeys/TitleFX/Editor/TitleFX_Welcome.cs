﻿ using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEditor;
using System;
using System.Net.Mail;
using System.Net;

using UnityEngine.Networking;

using UnityEngine.Events;
using UnityEngine.UI;

using System.Text;


namespace Orangedkeys.TitleFX
{
    public class WelcomeTitleFX : EditorWindow
    {

        //sUBSCRIBE
        private const string kGFormBaseURL = "https://docs.google.com/forms/d/e/1FAIpQLSfC-ISb57A-WfUnZPXx0eSXBHaWPAML4StnOwSGelPuyosMxA/";
        private const string kGFormEntryID = "entry.837402547";
        private const string kGFormEntryID2 = "entry.1451810422";
        private const string kGFormEntryID3 = "entry.1123184567";

        // links
        public const string WEB_URL = "https://www.orangedkeys.com";
        public const string YOUTUBE_URL = "https://www.youtube.com/channel/UC68I9tTol5hAoyVVAVedPag";

        private string email;
        private string ownername;
        private EditorGUILayout Label;

        private static readonly int WelcomeWindowWidth = 512;
        private static readonly int WelcomeWindowHeight = 512;

        private static Texture2D tex;

        private static GUIStyle _largeTextStyle;
        public static GUIStyle LargeTextStyle
        {
            get
            {
                if (_largeTextStyle == null)
                {
                    _largeTextStyle = new GUIStyle(UnityEngine.GUI.skin.label)
                    {
                        richText = true,
                        wordWrap = true,
                        fontStyle = FontStyle.Bold,
                        fontSize = 14,
                        alignment = TextAnchor.MiddleLeft,
                        padding = new RectOffset() { left = 0, right = 0, top = 0, bottom = 0 }
                    };
                }
                _largeTextStyle.normal.textColor = new Color32(200, 100, 0, 255);
                return _largeTextStyle;
            }
        }

        private static GUIStyle _regularTextStyle;
        public static GUIStyle RegularTextStyle
        {
            get
            {
                if (_regularTextStyle == null)
                {
                    _regularTextStyle = new GUIStyle(UnityEngine.GUI.skin.label)
                    {
                        richText = true,
                        wordWrap = true,
                        fontStyle = FontStyle.Normal,
                        fontSize = 12,
                        alignment = TextAnchor.MiddleLeft,
                        padding = new RectOffset() { left = 0, right = 0, top = 0, bottom = 0 }
                    };
                }
                return _regularTextStyle;
            }
        }

        private static GUIStyle _footerTextStyle;
        public static GUIStyle FooterTextStyle
        {
            get
            {
                if (_footerTextStyle == null)
                {
                    _footerTextStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
                    {
                        alignment = TextAnchor.LowerCenter,
                        wordWrap = true,
                        fontSize = 12
                    };
                }

                return _footerTextStyle;
            }
        }

        [MenuItem("Tools/OrangedKeys/TitleFX_Pack/About ", false, 0)]
        public static void ShowWindow()
        {
            EditorWindow editorWindow = GetWindow(typeof(WelcomeTitleFX), false, " About ", true);
            editorWindow.autoRepaintOnSceneChange = true;
            editorWindow.titleContent.image = EditorGUIUtility.IconContent("animationdopesheetkeyframe").image;
            editorWindow.maxSize = new Vector2(WelcomeWindowWidth, WelcomeWindowHeight);
            editorWindow.minSize = new Vector2(WelcomeWindowWidth, WelcomeWindowHeight);
            editorWindow.position = new Rect(Screen.width / 2 + WelcomeWindowWidth / 2, Screen.height / 2, WelcomeWindowWidth, WelcomeWindowHeight);
            editorWindow.Show();
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                this.ShowNotification(new GUIContent("Compiling Scripts", EditorGUIUtility.IconContent("BuildSettings.Editor").image));
            }
            else
            {
                this.RemoveNotification();
            }



            /*
            tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, new Color(0.55f, 0.55f, 0.55f));
            tex.Apply();
            
            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), tex, ScaleMode.StretchToFill);
            */

            // Add The Banner
            Texture2D welcomeImage = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Orangedkeys/TitleFX/Resources/TitleFX_PACK.png", typeof(Texture2D));
            Rect welcomeImageRect = new Rect(0, 0, 512, 128);
            UnityEngine.GUI.DrawTexture(welcomeImageRect, welcomeImage);

            GUILayout.Space(20);

            GUILayout.BeginArea(new Rect(EditorGUILayout.GetControlRect().x + 10, 200, WelcomeWindowWidth - 20, WelcomeWindowHeight));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Have fun with ''TITLE FX PACK'' !! \n", LargeTextStyle);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Join my Circle for exclusive updates and Cool STUFF!! \n", RegularTextStyle);
            EditorGUILayout.Space();




            // subscribe
            ownername = EditorGUILayout.TextField("Name : ", ownername, GUILayout.MaxWidth(480f));
            email = EditorGUILayout.TextField("E-mail : ", email, GUILayout.MaxWidth(480f));
            EditorGUILayout.Space();
            if (GUILayout.Button(new GUIContent(" JOIN! ", EditorGUIUtility.IconContent("d_orangeLight").image), GUILayout.MaxWidth(480)))
            {

                SendGFormData(email,ownername);
                
            }

            // WEBSITE

            if (GUILayout.Button(new GUIContent("  www.OrangedKeys.com  ", EditorGUIUtility.IconContent("BuildSettings.Web.Small").image), GUILayout.MaxWidth(480)))
            {
                Application.OpenURL(WEB_URL);

            }

            // YOUTUBE

            if (GUILayout.Button(new GUIContent("  YouTube Channel  ", EditorGUIUtility.IconContent("Animation.Record").image), GUILayout.MaxWidth(480)))
            {
                Application.OpenURL(YOUTUBE_URL);

            }





            GUILayout.EndArea();

            Rect areaRect = new Rect(0, WelcomeWindowHeight - 20, WelcomeWindowWidth, WelcomeWindowHeight - 20);
            GUILayout.BeginArea(areaRect);
            EditorGUILayout.LabelField("Copyright © 2022 OrangedKeys", FooterTextStyle);
            GUILayout.EndArea();

        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }







        public void SendGFormData(string Useremail, string ownername)
        {


            string example = Useremail;
            string assetname = "TitleFX,";
            WWWForm form = new WWWForm();

            form.AddField(kGFormEntryID, example);
            form.AddField(kGFormEntryID2, assetname);
            form.AddField(kGFormEntryID3, ownername);

            string urlGFormResponse = kGFormBaseURL + "formResponse";



            UnityWebRequest www = UnityWebRequest.Post(urlGFormResponse, form);

            www.SendWebRequest();

            if (www == null)
            {
                Debug.Log("Subscribe error: can't build request");

            }
            else
            {
                //yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.Log("Subscribe success");


                }
                else
                {
                    Debug.Log("Subscribe error: " + www.error);

                }
            }




        }
    }
}
