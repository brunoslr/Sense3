using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class MineObstacle : MonoBehaviour
{
    public float freq = 20.0f;
    private float playerDist;
    private float endPos;
    private float totalDist;
    private float ratio;
    private State state;

    Coroutine activeCoroutine;

    enum State{ NEW, LOW, MED, HIGH};


	// Use this for initialization
	void Start () 
    {
        endPos = transform.position.z + transform.localScale.z / 2.0f;	
        state = State.NEW;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        totalDist = Mathf.Abs(endPos - other.gameObject.transform.position.z);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        playerDist = Mathf.Abs(endPos - other.gameObject.transform.position.z);
        ratio = playerDist / totalDist;
        switch (state)
        {
            case State.NEW:
                state = State.LOW;
                activeCoroutine = StartCoroutine(SetVibrationPWM(0.2f, freq));
                break;
            case State.LOW:
                if (ratio > 0.6f)
                {
                    state = State.MED;
                    StopCoroutine(activeCoroutine);
                    activeCoroutine = StartCoroutine(SetVibrationPWM(0.2f, freq));
                }
                break;
            case State.MED:
                if (ratio < 0.6f && ratio > 0.3f)
                {
                    state = State.HIGH;
                    StopCoroutine(activeCoroutine);
                    activeCoroutine = StartCoroutine(SetVibrationPWM(0.2f, freq));
                }
                break;
            case State.HIGH:
                if (ratio < 0.3f)
                {
                    StopCoroutine(activeCoroutine);
                }
                break;
            default:
                break;
        }



        return;

        if (other.gameObject.tag == "Player")
        {
            playerDist = Mathf.Abs(endPos - other.gameObject.transform.position.z);

            ratio = playerDist / totalDist;

            if(ratio > 0.6f)
            {
                StartCoroutine(StartVibration(0.5f));
            }

            else if(ratio < 0.6f && ratio > 0.3f)
            {
                StartCoroutine(StartVibration(0.5f));
            }

            else
            {
                StartCoroutine(StartVibration(0.0f));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            StopVibration();
    }


    IEnumerator SetVibrationPWM(float timeActive, float freq)
    {
        float PWMDur = 1 / freq;
        timeActive = 
    }

    IEnumerator StartVibration()
    {
        GamePad.SetVibration(0, 1.0f, 1.0f);
    }

    void StopVibration()
    {
        GamePad.SetVibration(0, 0.0f, 0.0f);
    }

    void OnDestroy()
    {
        StopVibration();
    }
}
