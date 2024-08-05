using System;
using UnityEditor;
using UnityEditor.Callbacks;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor.Build;
    using UnityEditor.Build.Reporting;
#endif

namespace _Tools.Editor
{
	public class BuildIncrementor : IPreprocessBuildWithReport
	{
		public int callbackOrder { get; } = 1;

		public void OnPreprocessBuild(BuildReport report)
		{
#if UNITY_EDITOR
			PlayerSettings.macOS.buildNumber = IncrementBuildIndexString(PlayerSettings.macOS.buildNumber);
			PlayerSettings.iOS.buildNumber = IncrementBuildIndexString(PlayerSettings.iOS.buildNumber);
			PlayerSettings.Android.bundleVersionCode++;
#endif
		}

		private string IncrementBuildIndexString(string index)
		{
			int.TryParse(index, out int indexToIncrement);
			indexToIncrement++;
			return indexToIncrement.ToString();
		}


		[PostProcessBuildAttribute(Int32.MaxValue)] //We want this code to run last!
		public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuildProject)
		{
#if UNITY_IOS
			if (buildTarget != BuildTarget.iOS)
				return; // Make sure its iOS build

			// Getting access to the xcode project file
			string projectPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
			PBXProject pbxProject = new PBXProject();
			pbxProject.ReadFromFile(projectPath);

			// Getting the UnityFramework Target and changing build settings
			string target = pbxProject.GetUnityFrameworkTargetGuid();
			pbxProject.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

			// After we're done editing the build settings we save it 
			pbxProject.WriteToFile(projectPath);
#endif
		}
		
		[PostProcessBuild]
		public static void AddToEntitlements(BuildTarget buildTarget, string buildPath)
		{
#if UNITY_IOS
			if (buildTarget != BuildTarget.iOS) return;
 
			// get project info
			string pbxPath = PBXProject.GetPBXProjectPath(buildPath);
			var proj = new PBXProject();
			proj.ReadFromFile(pbxPath);
			var guid = proj.GetUnityMainTargetGuid();
 
			// get entitlements path
			string[] idArray = Application.identifier.Split('.');
			var entitlementsPath = $"Unity-iPhone/{idArray[^1]}.entitlements";
 
			// create capabilities manager
			var capManager = new ProjectCapabilityManager(pbxPath, entitlementsPath, null, guid);
 
			// Add necessary capabilities
			capManager.AddPushNotifications(false);
 
			// Write to file
			capManager.WriteToFile();
#endif
		}
	}

}