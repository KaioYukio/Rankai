using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breath : MonoBehaviour
{
    public CharacterMovement characterMovement;
    public Health playerHealth;
    public GameObject blink;
    public PlayerSound playerAudio;


    public Material katanaMaterial;
    public float speedMultiplier;
    public float track;
    private Vector4 glowPos;
    public Color parryColor;
    public Color timingColor;
    public Color heavyColor;

    public float blinkGapTime;
    public bool isInBlinkGap;

    public float maxBreathPoints;
    public float breathPoints;
    public float pointsToRemove;
    public float pointsToAdd;
    public float breathPointsToSpecial;
    public float breathPointsToHeal;
    public float healAmount;
    private float timerKey;


    // Start is called before the first frame update
    void Start()
    {
        track = 0.68f;
        glowPos = katanaMaterial.GetVector("GlowPosition");
        katanaMaterial.SetVector("GlowPosition", new Vector4(track, glowPos.y, glowPos.z, 0));
        //Time.timeScale = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //speedMultiplier = 

        if (GameManager.instance.IsPlayable())
        {

            blink.SetActive(IsInBlinkGap());
            UseBreath();
        }


        //Time.timeScale = Mathf.Lerp(Time.deltaTime, 0.5f, 0.1f);


    }

    void UseBreath()
    {
        if (!GameManager.instance.isSlow)
        {
            if (Input.GetKey(KeyCode.F))
            {
                timerKey += Time.deltaTime;

                if (timerKey >= 0.5f)
                {
                    if (breathPoints >= breathPointsToSpecial)
                    {
                        GameManager.instance.DoSlowMotion();
                        //breathPoints -= breathPointsToSpecial;
                        Debug.Log("Use Special");
                    }
                    timerKey = 0;
                    return;
                }
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                if (breathPoints >= breathPointsToSpecial)
                {
                    breathPoints -= breathPointsToHeal;
                    playerHealth.health += healAmount;
                    Debug.Log("Use Heal");
                }
                timerKey = 0;
            }
        }
        else
        {
            ConsumeBreathPoint();
        }
       
    }

    void ConsumeBreathPoint()
    {
        breathPoints -= pointsToRemove * Time.deltaTime * GameManager.instance.gameSpeed;

        if (breathPoints <= 0)
        {
            GameManager.instance.DoSlowMotion();
        }

    }

    public void StartBreathing(float duration_)
    {
        if (!GameManager.instance.isSlow && characterMovement.isLockedOn)
        {
            StopAllCoroutines();
            if (track == 0.68f)
            {
                StartCoroutine(RunGlow());
            }
        }



    }

    public IEnumerator RunGlow()
    {

        while (track > -0.26f)
        {
            track -= speedMultiplier * Time.deltaTime;

            katanaMaterial.SetVector("GlowPosition", new Vector4(track, glowPos.y, glowPos.z, 0));
            yield return null;
        }

        track = 0.68f;
        katanaMaterial.SetVector("GlowPosition", new Vector4(track, glowPos.y, glowPos.z, 0));

        StartCoroutine(BlinkGap());
        playerAudio.PlayBlink();

    }

    public IEnumerator BlinkGap()
    {

        isInBlinkGap = true;

        yield return new WaitForSeconds(blinkGapTime);

        isInBlinkGap = false;


    }

    public void ResetBlinkGap()
    {
        StopAllCoroutines();
        track = 0.68f;
        katanaMaterial.SetVector("GlowPosition", new Vector4(track, glowPos.y, glowPos.z, 0));
        isInBlinkGap = false;
        //Debug.Log("Blink Reset");
    }

    public bool IsInBlinkGap()
    {
        return isInBlinkGap;
    }

    public void AddBreathPoints()
    {
        breathPoints += pointsToAdd;
    }

    public void HitStop()
    {

    }

    public void GlowParry()
    {
        katanaMaterial.SetColor("Color_Glow", parryColor);
        katanaMaterial.SetVector("GlowPosition", new Vector4(track, glowPos.y, 2, 0));
        katanaMaterial.SetVector("GlowIntensity", new Vector4(5, 0, 0, 0));
    }

    public void GlowHeavyAttack()
    {
        katanaMaterial.SetColor("Color_Glow", heavyColor);
        katanaMaterial.SetVector("GlowPosition", new Vector4(track, glowPos.y, 2, 0));
        katanaMaterial.SetVector("GlowIntensity", new Vector4(5, 0, 0, 0));
    }

    public void ResetGlow()
    {
        track = 0.68f;
        katanaMaterial.SetColor("Color_Glow", timingColor);
        katanaMaterial.SetVector("GlowPosition", new Vector4(track, glowPos.y, glowPos.z, 0));
        katanaMaterial.SetVector("GlowIntensity", new Vector4(5, 0, 0, 0));
    }

}
