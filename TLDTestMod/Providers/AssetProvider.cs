using MelonLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TLDTestMod.Providers
{
    public class AssetProvider : IDisposable
    {
        private readonly string? _basePath;

        private readonly Dictionary<string, AssetBundle> _bundles;

        private readonly MelonLogger.Instance _logger;

        private bool _disposed = false;

        public AssetProvider(string? basePath, MelonLogger.Instance logger)
        {
            _logger = logger;
            _basePath = basePath;
            _bundles = new Dictionary<string, AssetBundle>();
        }

        public bool Register(string bundlePath, string bundleName)
        {
            if (_disposed)
            {
                _logger.Error("Cannot register bundle: Provider is already disposed.");
                return false;
            }

            if (_bundles.ContainsKey(bundleName))
            {
                _logger.Warning($"Bundle '{bundleName}' already registered. Skipping.");
                return false;
            }

            string fullPath;
            if (Path.IsPathRooted(bundlePath))
            {
                fullPath = bundlePath;
            }
            else if (!string.IsNullOrEmpty(_basePath))
            {
                fullPath = Path.Combine(_basePath, bundlePath);
            }
            else
            {
                fullPath = bundlePath;
            }

            if (!File.Exists(fullPath))
            {
                _logger.Error($"Asset bundle file not found: {fullPath}");
                return false;
            }

            AssetBundle? bundle = AssetBundle.LoadFromFile(fullPath);

            if (bundle == null)
            {
                _logger.Error($"Failed to load asset bundle (possibly corrupted or wrong version): {fullPath}");
                return false;
            }

            _bundles.Add(bundleName, bundle);
            _logger.Msg($"Successfully loaded bundle '{bundleName}' from {fullPath}");
            return true;
        }

        public T? Load<T>(string bundleName, string assetName) where T : UnityEngine.Object
        {
            if (_disposed)
            {
                _logger.Error("Cannot load asset: Provider is disposed.");
                return null;
            }

            if (!_bundles.TryGetValue(bundleName, out AssetBundle? bundle))
            {
                _logger.Error($"Bundle '{bundleName}' not found. Did you call Register()?");
                return null;
            }

            T? asset = bundle.LoadAsset<T>(assetName);

            if (asset == null)
            {
                _logger.Error($"Asset '{assetName}' not found in bundle '{bundleName}'. Available assets might differ.");
                return null;
            }

            return asset;
        }

        public bool Unload(string bundleName, bool unloadObjects = true)
        {
            if (_disposed)
            {
                _logger.Error("Cannot unload bundle: Provider is already disposed.");
                return false;
            }

            if (!_bundles.TryGetValue(bundleName, out AssetBundle? bundle))
            {
                _logger.Warning($"Bundle '{bundleName}' not found. Nothing to unload.");
                return false;
            }

            try
            {
                bundle.Unload(unloadObjects);
                _bundles.Remove(bundleName);
                _logger.Msg($"Bundle '{bundleName}' unloaded successfully (unloadAllLoadedObjects={unloadObjects})");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to unload bundle '{bundleName}': {ex.Message}");
                return false;
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return; 
            }

            _logger.Msg("Disposing AssetProvider and unloading all bundles...");

            foreach (var kvp in _bundles)
            {
                try
                {
                    kvp.Value.Unload(true);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Error unloading bundle '{kvp.Key}': {ex.Message}");
                }
            }

            _bundles.Clear();
            _disposed = true;
        }
    }
}