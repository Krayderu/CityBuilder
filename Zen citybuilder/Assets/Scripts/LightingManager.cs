using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //references
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPresets Preset;
    //Variables
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private void Update()
    {
        if (Preset == null)
            return;

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime*0.5f;
            TimeOfDay %= 24; //Clamp between 0-24
            UpdateLighting(TimeOfDay / 24f);
        }
        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }
    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f)-90f,170f,0));
        }

        Light[] lights = GameObject.FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.type != LightType.Directional)
            {
                if (light.GetComponent<LightSource>() != null)
                {
                    LightSource lightSource = light.GetComponent<LightSource>();
                    if (lightSource.startTime <= TimeOfDay && lightSource.endTime >= TimeOfDay)
                    {
                        light.enabled = true;
                    }
                    else
                    {
                        light.enabled = false;
                    }
                }
            }
        }
    }
    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;
        if(RenderSettings.sun != null)
        { 
            DirectionalLight = RenderSettings.sun; 
        }
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if(light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
        
    }
}
