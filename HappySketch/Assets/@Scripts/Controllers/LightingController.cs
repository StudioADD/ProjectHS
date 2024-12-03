using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public static class LightingController
{
    public static void InitLighting()
    {
        RenderSettings.skybox = null;
        RenderSettings.fog = false;
        RenderSettings.fogColor = new Color(128, 128, 128);
        RenderSettings.fogDensity = 0.01f;
    }

    public static void SetStageLighting(EStageType stageType)
    {
        switch (stageType)
        {
            case EStageType.SharkAvoidance:
                {
                    string path = $"{LoadPath.MATERIALS_PATH}/{StageSkyBoxName.SHARKAVOIDANCE_NAME}";
                    RenderSettings.skybox = Managers.Resource.Load<Material>(path);

                    RenderSettings.ambientIntensity = 1f;
                    RenderSettings.reflectionIntensity = 1f;
                    
                    RenderSettings.fog = true;
                    RenderSettings.fogColor = Util.HexColor("#1697DD");
                    RenderSettings.fogMode = FogMode.ExponentialSquared;
                    RenderSettings.fogDensity = 0.002f;
                    break;
                }
            case EStageType.CollectingCandy:
                {
                    string path = $"{LoadPath.MATERIALS_PATH}/{StageSkyBoxName.COLLECTINGCANDY_NAME}";
                    RenderSettings.skybox = Managers.Resource.Load<Material>(path);

                    RenderSettings.ambientIntensity = 1f;
                    RenderSettings.reflectionIntensity = 1f;

                    RenderSettings.fog = false;
                    break;
                }
            case EStageType.CrossingBridge:
                {
                    string path = $"{LoadPath.MATERIALS_PATH}/{StageSkyBoxName.CROSSINGBRIDGE_NAME}";
                    RenderSettings.skybox = Managers.Resource.Load<Material>(path);

                    RenderSettings.ambientIntensity = 1f;
                    RenderSettings.reflectionIntensity = 0.5f;

                    RenderSettings.fog = false;
                    break;
                }
            default:
                InitLighting();
                break;
        }

        DynamicGI.UpdateEnvironment();
    }
}
