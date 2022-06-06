#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

using UnityObject = UnityEngine.Object;

namespace Unity.LiveCapture
{
    static class AssetDatabaseUtility
    {
        /// <summary>
        /// Returns the list of assets at given directory.
        /// </summary>
        /// <typeparam name="T">Type of the asset to load.</typeparam>
        /// <param name="directory">Path of the assets to load.</param>
        /// <returns>The list of assets.</returns>
        public static List<T> GetAssetsAtPath<T>(string directory) where T : UnityObject
        {
            if (string.IsNullOrEmpty(directory))
            {
                return new List<T>();
            }

            directory = Path.GetDirectoryName($"{directory}/");

            if (!Directory.Exists(directory))
            {
                return new List<T>();
            }

            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { directory });
            var paths = guids.Select(guid => AssetDatabase.GUIDToAssetPath(guid));

            return paths.Select(path => AssetDatabase.LoadAssetAtPath<T>(path)).ToList();
        }

        /// <summary>
        /// Returns the GUID of a given asset.
        /// </summary>
        /// <param name="asset">The asset to get the GUID from.</param>
        /// <returns>The string representation of the GUID of the asset.</returns>
        public static string GetAssetGUID(UnityObject asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);

            return AssetDatabase.AssetPathToGUID(path);
        }

        /// <summary>
        /// Returns the asset associated with a given GUID.
        /// </summary>
        /// <typeparam name="T">Type of the asset to load.</typeparam>
        /// <param name="guid">The string representation of a GUID.</param>
        /// <returns>The asset associated with the given GUID.</returns>
        public static T LoadAssetWithGuid<T>(string guid) where T : UnityObject
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);

            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        /// <summary>
        /// Returns the asset associated with a given GUID.
        /// </summary>
        /// <typeparam name="T">Type of the asset to load.</typeparam>
        /// <param name="guid">The GUID.</param>
        /// <returns>The asset associated with the given GUID.</returns>
        public static T LoadAssetWithGuid<T>(Guid guid) where T : UnityObject
        {
            return LoadAssetWithGuid<T>(guid.ToString("N"));
        }

        /// <summary>
        /// Returns the asset associated with a given SerializableGuid.
        /// </summary>
        /// <typeparam name="T">Type of the asset to load.</typeparam>
        /// <param name="guid">The SerializableGuid.</param>
        /// <returns>The asset associated with the given GUID.</returns>
        internal static T LoadAssetWithGuid<T>(SerializableGuid guid) where T : UnityObject
        {
            return LoadAssetWithGuid<T>(guid.ToString());
        }

        /// <summary>
        /// Returns the list of sub assets at given asset.
        /// </summary>
        /// <typeparam name="T">Type of the asset to load.</typeparam>
        /// <param name="asset">The main asset reference.</param>
        /// <returns>The list of assets.</returns>
        public static List<T> GetSubAssets<T>(UnityObject asset) where T : UnityObject
        {
            var assets = new List<T>();
            var path = AssetDatabase.GetAssetPath(asset);

            if (!string.IsNullOrEmpty(path))
            {
                var enumerable = AssetDatabase.LoadAllAssetsAtPath(path).Where(a => a is T).Cast<T>();

                assets.AddRange(enumerable);
            }

            return assets;
        }
    }
}
#endif
