using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz1 : MonoBehaviour
{
    // Carlos Eduardo Cordoba Hilton A01658948
    // Jesus Javier Hernandez Delgado A01658891
    
    GameObject LINK1;
    GameObject LINK2;
    GameObject LINK3;

    Vector3[] vLINK1;
    Vector3[] vLINK2;
    Vector3[] vLINK3;

    Matrix4x4 tr1;
    Matrix4x4 tm1;
    Matrix4x4 ts1;
    
    Matrix4x4 tr2;
    Matrix4x4 tm2;
    Matrix4x4 ts2;

    Matrix4x4 tr3;
    Matrix4x4 tm3;
    Matrix4x4 ts3;

    public float rotZ;
    bool isUP = false;
    // Start is called before the first frame update
    void Start()
    {
        rotZ = 0f;
        LINK1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vLINK1 = LINK1.GetComponent<MeshFilter>().mesh.vertices;

        LINK2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vLINK2 = LINK2.GetComponent<MeshFilter>().mesh.vertices;

        LINK3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        vLINK3 = LINK3.GetComponent<MeshFilter>().mesh.vertices;

    }
    // Update is called once per frame
    void Update()
    {

        tr1 = Transformaciones.RotateZ(rotZ* 0.7f);
        tm1 = Transformaciones.Translate(0.5f, 0, 0);
        ts1 = Transformaciones.Scale(1f, 0.5f, 0.5f);
        LINK1.GetComponent<MeshFilter>().mesh.vertices = Transformaciones.Transform(tr1 * tm1 * ts1,  vLINK1);
        LINK1.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        tr2 = Transformaciones.RotateZ(rotZ * 0.7f);
        tm2 = Transformaciones.Translate(0.5f, 0, 0);
        ts2 = Transformaciones.Scale(1f, 0.5f, 0.5f);
        LINK2.GetComponent<MeshFilter>().mesh.vertices = Transformaciones.Transform(tr1 * tm1 * tm2 *tr2 * tm2 * ts1,  vLINK2);
        LINK2.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        tr3 = Transformaciones.RotateZ(rotZ * 0.7f);
        tm3 = Transformaciones.Translate(0.5f, 0, 0);
        ts3 = Transformaciones.Scale(1f, 0.5f, 0.5f);
        LINK3.GetComponent<MeshFilter>().mesh.vertices = Transformaciones.Transform(tr1 * tm1 * tm2 *tr2 * tm2 * tm3 * tr3 * tm3 * ts1,  vLINK3);
        LINK3.GetComponent<MeshFilter>().mesh.RecalculateNormals();

        if(rotZ < 45.0f && isUP == false)
        {
            rotZ += 0.5f;
        }
        else if(rotZ <= 45.0f && isUP == true)
        {
            rotZ -=0.5f;
        }
        else if(rotZ >= 45.0f )
        {
            isUP = true;
        }
        else if(rotZ <= -45.0f)
        {
            isUP = false;
        }
    }
}
