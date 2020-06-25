using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{
    public CinemachineFreeLook lockCam;
    public CinemachineFreeLook freeCam;

    public Vector2 lockCamValue;
    public float lerpP;
    public float speedLerp;
    public float speedLerpBack;
    public Vector2 FOVLimits;
    public bool hasShake;
    public bool hasWait;

    public float parryLerpSpeed;
    public float parryLerpSpeedReturn;
    public Vector2 parryFOVLimits;

    public float heavyLerpSpeed;
    public float heavyLerpSpeedReturn;
    public Vector2 heavyFOVLimits;

    public bool zoomTrigger;
    public bool heavyTrigger;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CamParryZoom();
        //CamHeavyAttack();
    }

    public void LockYValue()
    {
        lockCam.m_YAxis.Value = lockCamValue.y;
    }

    public void StartScreenShake()
    {

        for (int i = 0; i < 3; i++)
        {
            lockCam.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.3f;
            freeCam.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0.3f;
        }
    }

    public void StopScreenShake()
    {
        for (int i = 0; i < 3; i++)
        {
            lockCam.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
            freeCam.GetRig(i).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        }
    }

    public void CamParryZoom()
    {
        if (zoomTrigger)
        {
            lerpP += Time.deltaTime * speedLerp;

            if (lerpP >= 1)
            {
                if (hasWait)
                {

                }
                else
                {
                    zoomTrigger = false;
                }

            }
            else if (lerpP >= 0.3f)
            {
                if (hasShake)
                {
                    StopScreenShake();
                }

            }
        }
        else
        {
            speedLerp = speedLerpBack;

            if (lerpP > 0)
            {
                lerpP -= Time.deltaTime * speedLerp;
            }
        }

        if (lockCam.isActiveAndEnabled)
        {
            lockCam.m_Lens.FieldOfView = Mathf.Lerp(FOVLimits.x, FOVLimits.y, lerpP);

        }
        else
        {
            freeCam.m_Lens.FieldOfView = Mathf.Lerp(FOVLimits.x, FOVLimits.y, lerpP);
        }


    }


    public void ParryZoom()
    {
        StartScreenShake();
        speedLerp = parryLerpSpeed;
        speedLerpBack = parryLerpSpeedReturn;
        FOVLimits = parryFOVLimits;
        //lerpP = 0;
        hasWait = false;
        hasShake = true;
        zoomTrigger = true;

        Invoke("StopScreenShake", 1.5f);
    }


    public void HeavyAttackZoom()
    {
        StopScreenShake();
        speedLerp = heavyLerpSpeed;
        speedLerpBack = heavyLerpSpeedReturn;
        FOVLimits = heavyFOVLimits;
        //lerpP = 0;
        hasWait = true;
        hasShake = false;
        zoomTrigger = true;
    }



    public void ReleaseHeavyCam()
    {
        zoomTrigger = false;
    }

}
