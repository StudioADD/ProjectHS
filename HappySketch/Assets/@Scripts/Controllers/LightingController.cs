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
        switch(stageType)
        {
            case EStageType.SharkAvoidance:
                {
                    string path = $"{LoadPath.MATERIALS_PATH}/{SkyBoxMaterialName.SHARKAVOIDANCESTAGE_NAME}";
                    RenderSettings.skybox = Managers.Resource.Load<Material>(path);

                    RenderSettings.fog = true;
                    RenderSettings.fogColor = new Color(22, 151, 221);
                    RenderSettings.fogDensity = 0.002f;
                    break;
                }
            default: // 기본 세팅
                {
                    InitLighting();
                    break;
                }
        }
        
    }
}
