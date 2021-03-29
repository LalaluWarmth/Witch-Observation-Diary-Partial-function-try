using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class SpriteLight : MonoBehaviour
{
    public Light2D light2D;

    public VolumeProfile VolumeProfile;
    [Header("辉光强度")]
    public float bloomIntensity;

    private float _time;
    [Header("变化时间")]
    public float maxInternalTime;
    public float minInternalTime;
    [SerializeField]private float _internalTime;
    [Header("最小亮度")]
    public float maxLightIntensity1;
    public float minLightIntensity1;
    [SerializeField]private float _lightIntensity1;
    [Header("最大亮度")]
    public float maxLightIntensity2;
    public float minLightIntensity2;
    [SerializeField]private float _lightIntensity2;
    
    private bool _state;

    // Start is called before the first frame update
    void Start()
    {
        // var types = Assembly.GetExecutingAssembly().GetTypes();
        // var ctype = typeof(VolumeComponent);
        // foreach (var type in types)
        // {
        //     Debug.Log(type.FullName);
        // }
        //
        // var subClassList = Assembly.GetExecutingAssembly().GetTypes()
        //     .Where(x => x.IsSubclassOf(typeof(VolumeComponent))).ToList();
        // Debug.Log(subClassList.Count);
        // foreach (var subClass in subClassList)
        // {
        //     Debug.Log("子类:"+subClass.Name);
        // }

        // Predicate<VolumeComponent> predicate = i => i == ;
        // Debug.Log(VolumeProfile.components.Find(predicate).name);
        List<VolumeComponent> list = VolumeProfile.components;
        if (list[0].GetType() == typeof(Bloom))
        {
            var a = list[0] as Bloom;
            a.intensity.value = bloomIntensity;
            Debug.Log(a.intensity);
        }
        
        _state = false;
        _internalTime = Random.Range(minInternalTime, maxInternalTime);
        _lightIntensity1 = Random.Range(minLightIntensity1, maxLightIntensity1);
        _lightIntensity2 = Random.Range(minLightIntensity2, maxLightIntensity2);
    }

    // Update is called once per frame
    void Update()
    {
        if (_state)
        {
            if (_time > 1)
            {
                _time = 1;
                _state = false;
                _internalTime = Random.Range(minInternalTime, maxInternalTime);
                _lightIntensity1 = Random.Range(minLightIntensity1, maxLightIntensity1);
                _lightIntensity2 = Random.Range(minLightIntensity2, maxLightIntensity2);
            }

            light2D.intensity = Mathf.Lerp(_lightIntensity1, _lightIntensity2, _time);
            _time += Time.deltaTime * _internalTime;
        }
        else
        {
            if (_time < 0)
            {
                _time = 0;
                _state = true;
                _internalTime = Random.Range(minInternalTime, maxInternalTime);
                _lightIntensity1 = Random.Range(minLightIntensity1, maxLightIntensity1);
                _lightIntensity2 = Random.Range(minLightIntensity2, maxLightIntensity2);
            }

            light2D.intensity = Mathf.Lerp(_lightIntensity1, _lightIntensity2, _time);
            _time -= Time.deltaTime * _internalTime;
        }
    }
}