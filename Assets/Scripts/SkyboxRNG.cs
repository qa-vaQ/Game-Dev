using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    public Material[] skyboxMaterials; 

    void Start()
    {
        RandomizeSkybox();
    }

    void RandomizeSkybox()
    {
        if (skyboxMaterials.Length > 0)
        {
            int index = Random.Range(0, skyboxMaterials.Length);
            
            RenderSettings.skybox = skyboxMaterials[index];

            DynamicGI.UpdateEnvironment();
        }
    }
}
