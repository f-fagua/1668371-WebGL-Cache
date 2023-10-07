using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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

    private void GetCachedVersions()
    {
        foreach (var resourceLocator in Addressables.ResourceLocators)
        {
            foreach (var key in resourceLocator.Keys)
            {
                Debug.Log(key);
            }
        }
    }

    private void LoadAsset()
    {
        m_Asset.InstantiateAsync().Completed += handle =>
        {
            GetCachedVersions();    
        };
    }
}
