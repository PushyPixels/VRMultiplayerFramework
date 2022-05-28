using System.IO;
using UnityEngine;
using UnityEditor;

class GUIDAssetPostprocessor : AssetPostprocessor
{
    // This happens anytime the project changes... probably can find a better place for this.
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {
        string path = Path.Combine(Application.dataPath,"Plugins/Android/libs/eos-sdk.aar");
        Debug.Log("Doing something");
        // Not GUID related (so I should rename this class) but I also need to make sure that Plugins has been moved
        if(!File.Exists(path))
        {
            Debug.Log("ACTUALLY doing something");
            string sourcePath = Path.Combine(Application.dataPath, "EpicOnlineTransport/Plugins/");
            string destinationPath = Path.Combine(Application.dataPath, "Plugins");
            FolderCopyUtility.CopyDirectory(sourcePath,destinationPath,true);
        }

        // I chose StreamingAssets because it will guarentee your builds get the file
        path = Path.Combine(Application.streamingAssetsPath, "MultiplayerGUID.guid");

        // Early exit if we already have a GUID file
        if(File.Exists(path))
        {
            return;
        }
        else
        {
            // If StreamingAssets folder hasn't been created, make it
            if(!Directory.Exists(Application.streamingAssetsPath))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath);
            }

            // Generate guid and write it to 
            GUID guid = GUID.Generate();
            File.WriteAllText(path,guid.ToString());
        }
    }
}