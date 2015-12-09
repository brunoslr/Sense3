using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spectrum : MonoBehaviour
{
    //private float average = 0;
    public Camera mainCamera;
    public GameObject prefab;
    public int numberOfObjects;
    public float radius = 5f;
    public GameObject[] cubes;
    public GameObject parent;
    public int channel;
    public FFTWindow window;
    public List<Material> cubeMat;
    public float[] spectrum;
    Vector3 pos;
    public GameObject spectrumObj;
    void Start()
    {
        //  Instantiate(parent);
        for (int i = 0; i < numberOfObjects; i++)
        {
            //float angle = i * Mathf.PI * 2 / numberOfObjects;
            //   Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            pos = new Vector3(i+8, 0, 0);
            GameObject temp = Instantiate(prefab, pos, Quaternion.identity) as GameObject;
            temp.transform.SetParent(parent.transform);
            temp.gameObject.layer = 8;
            cubeMat.Add(temp.gameObject.GetComponent<Renderer>().material);
        }
        parent.transform.position = new Vector3(0, 0, 100);
        cubes = GameObject.FindGameObjectsWithTag("Cubes");
        InvokeRepeating("ChangeColorPatterns", 0.1f, 1);

    }

    void Update()
    {
        float sum = 0;
       spectrum = AudioListener.GetSpectrumData(1024, 0, window);
        for (int i = 0; i < numberOfObjects; i++)
        {
                Vector3 previousScale = cubes[i].transform.localScale;
                previousScale.y = Mathf.Lerp(previousScale.y, Mathf.Clamp(spectrum[i] * (30 + i * i), 0, 10), Time.deltaTime * 15);
                cubes[i].transform.localScale = previousScale;
                cubes[i].transform.position = new Vector3(cubes[i].transform.position.x, previousScale.y / 2 + 1, cubes[i].transform.position.z);
                sum += spectrum[i];
        }

         spectrumObj.GetComponent<DynamicTexture>().FFT[0]= spectrum[0];
         spectrumObj.GetComponent<DynamicTexture>().FFT[1] = spectrum[1];
         spectrumObj.GetComponent<DynamicTexture>().FFT[2] = spectrum[2];
         spectrumObj.GetComponent<DynamicTexture>().FFT[3] = spectrum[3];

         mainCamera.gameObject.GetComponent<ForeGroundController>().setBloom(spectrum[3]);
         //this.gameObject.GetComponent<backGroundScript>().setTwirlAngle(spectrum[2]);
         //this.gameObject.GetComponent<backGroundScript>().setTwirlRadius(spectrum[1]);
        
        //sum /= numberOfObjects;
        //setSpectrumAndAverage(sum);    
    }

    //void setSpectrumAndAverage(float sum)
    //{
    //    spectrumObj.GetComponent<DynamicTexture>().FFT = Mathf.Sin((sum) * 100.0f);
    //    average = (average + sum) / 2.0f;
    //}

    void ChangeColorPatterns()
    {
        for (int i = 0; i < cubeMat.Count; i++)
        {
            float rNum = Random.Range(1, 10);
            float rNum1 = Random.Range(1, 10);
            float rNum2 = Random.Range(1, 10);
            float rCol = (float) (rNum / 10);
            float rCol2 = (float)(rNum1 / 10);
            float rCol3 = (float)(rNum2 / 10);


            cubeMat[i].color = new Color(rCol2, rCol, rCol3, 0.5f);
        }
    }
}
