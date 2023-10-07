using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class Loader : MonoBehaviour
{
    [SerializeField] 
    private AssetReference m_Asset;
    
    [SerializeField] 
    private bool m_ClearCache;
    
    // Start is called before the first frame update
    void Start()
    {
        Addressables.InitializeAsync().Completed += handler =>
        {
            if (m_ClearCache)
                Caching.ClearCache();

            Invoke("LoadAsset", 5);
        };
    }

    private void LoadAsset()
    {
        m_Asset.InstantiateAsync().Completed += handle =>
        {
            var bundles = GetBundlesFromCatalog();
            ValidateCachedBundle(bundles);
        };
    }

    private List<CachedAssetBundle> GetBundlesFromCatalog()
    {
        List<CachedAssetBundle> catalogBundles = new List<CachedAssetBundle>();
        foreach (var resourceLocator in Addressables.ResourceLocators)
        {
            if (resourceLocator is not ResourceLocationMap) continue;
            var catalog = (ResourceLocationMap) resourceLocator;
            
            foreach (var location in catalog.Locations)
                if(LocationToCachedBundleData(location, out var cachedAssetBundle))
                    catalogBundles.Add(cachedAssetBundle);
        }

        return catalogBundles;
    }

    private bool LocationToCachedBundleData(KeyValuePair<object, IList<IResourceLocation>> location, out CachedAssetBundle cachedAssetBundle)
    {
        cachedAssetBundle = new CachedAssetBundle();
        if (location.Key.ToString().EndsWith(".bundle"))
        {
            if (location.Value[0].Data is not AssetBundleRequestOptions) return false;
                    
            var loadData = (AssetBundleRequestOptions) location.Value[0].Data;
            cachedAssetBundle.name = loadData.BundleName;
            cachedAssetBundle.hash = Hash128.Parse(loadData.Hash);
            return true;
        }

        return false;
    }
    
    private void ValidateCachedBundle(List<CachedAssetBundle> bundles)
    {
        var msj = new StringBuilder();
        foreach (var bundle in bundles)
        {
            msj.AppendLine($"name: {bundle.name}, hash: {bundle.hash}, cached: {Caching.IsVersionCached(bundle)}");
        }
        Debug.Log(msj);
    }
}
