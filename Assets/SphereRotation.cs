using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRotation : MonoBehaviour
{
    public GameObject gObject;
    Material material;
    // Start is called before the first frame update
    GameObject[,] positions = new GameObject[16, 8];
    float xRot = 0, yRot = 0;

    void Start()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 16; x++)
            {
                float yVal = Mathf.PI / 9 * (y + 1);
                float xVal = Mathf.PI / 16 * x;
                float f = Mathf.Sin(yVal);
                positions[x, y] = Instantiate(gObject, transform);
                Vector3 pos = new Vector3(Mathf.Cos(xVal * 2) * 0.5f * f, Mathf.Cos(yVal) * 0.5f, Mathf.Sin(xVal * 2) * 0.5f * f);
                positions[x, y].transform.localPosition = pos;
            }
        }
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = -Input.GetAxis("Horizontal");
        float vertical = -Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Rotate(new Vector3(vertical, horizontal) * Time.deltaTime * 35);
        }
        else
        {
            Vector2 values = material.GetTextureOffset("_MainTex");
            material.SetTextureOffset("_MainTex", new Vector2(values.x + horizontal * Time.deltaTime, values.y + vertical * Time.deltaTime));
            xRot += horizontal * Time.captureDeltaTime;
            yRot += vertical * Time.deltaTime;
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    float yVal = Mathf.PI / 9 * (y + 1 + yRot);
                    float xVal = Mathf.PI / 16 * x;
                    float f = Mathf.Sin(yVal);
                    Vector3 pos = new Vector3(Mathf.Cos(xVal * 2) * 0.5f * f, Mathf.Cos(yVal) * 0.5f, Mathf.Sin(xVal * 2) * 0.5f * f);
                    positions[x, y].transform.localPosition = pos;
                }
            }
        }
    }
}
