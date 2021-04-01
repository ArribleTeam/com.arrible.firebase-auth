using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace ARRTEditor.Firebase.AuthorizationManagement
{
    public class FirebaseAuthPostprocessor : AssetPostprocessor
    {
        private const string m_FileName = "Firebase.Auth.dll";
        private const string m_Define = "USE_FIREBASE_AUTH";

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            DetectFirebaseDeletion(deletedAssets);
            DetectFirebaseImport(importedAssets);
        }

        private static void DetectFirebaseImport(string[] importedAssets)
        {
            foreach (string assetPath in importedAssets)
            {
                if (Path.GetFileName(assetPath).Equals(m_FileName))
                {
                    DefineManager.TryAddDefine(m_Define, EditorUserBuildSettings.selectedBuildTargetGroup);
                }
            }
        }

        private static void DetectFirebaseDeletion(string[] deletedAssets)
        {
            foreach (string assetPath in deletedAssets)
            {
                if (Path.GetFileName(assetPath).Equals(m_FileName))
                {
                    DefineManager.TryRemoveDefine(m_Define, EditorUserBuildSettings.selectedBuildTargetGroup);
                }
            }
        }
    }
}
