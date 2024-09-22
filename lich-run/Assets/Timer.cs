using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField] private Image fillImage;

    public int duration = 60;
    private int remainingTime;

    // Start is called before the first frame update
    void Start()
    {
        remainingTime = duration;
        StartCoroutine(UpdateTimer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator UpdateTimer()
    {
        while (duration >= 0)
        {
            fillImage.fillAmount = Mathf.InverseLerp(0, duration, remainingTime);
            remainingTime--;
            yield return new WaitForSeconds(1f);
        }
    }
}
