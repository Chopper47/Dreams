using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class s_CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    public float shakeDuration = 1f;
    public float shakeIntensity;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin; 
    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }
    private void OnEnable()
    {
        StartCoroutine(ShakeCoroutine());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shake (float intensity)
    {
        if(cinemachineBasicMultiChannelPerlin)
        {
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        }

    }

    public void StopShake()
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }
    public IEnumerator ShakeCoroutine()
    {
        Debug.Log("Started coroutine");
        Shake(shakeIntensity);
        yield return new WaitForSeconds(shakeDuration);
        Debug.Log("Ended coroutine");
        StopShake();
        this.enabled = false;

    }



    
}
