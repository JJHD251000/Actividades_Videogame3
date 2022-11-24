using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuboRubik : MonoBehaviour
{
    public List<MeshFilter> Cubes = new List<MeshFilter>();
    public List<Vector3> positions = new List<Vector3>();

    Vector3[] originals;
    public List<Matrix4x4> mOriginals = new List<Matrix4x4>();
    public List<Matrix4x4> matrixes = new List<Matrix4x4>();

    float rotZ;
    float rotX;
    float rotY;

    // Start is called before the first frame update
    void Start()
    {
        int doub = 0;
        for(int i = 0; i < 8; i++){
            Vector3 y = doub < 2? Vector3.up : Vector3.down;
            Vector3 z = i < 4? Vector3.forward : Vector3.back;

            if(i%2 == 0) {
            positions.Add((Vector3.left + y + z)/2);
            }
            else if(i%2 != 0) {
                positions.Add((Vector3.right + y + z)/2);
            }

            doub++;
            if(doub >= 4) {
                doub = 0;
            }

            GameObject Cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Cubes.Add(Cube.GetComponent<MeshFilter>());
            mOriginals.Add(Transformaciones.Translate(positions[i].x, positions[i].y, positions[i].z));
            matrixes.Add(mOriginals[i]);
        }

        originals = Cubes[0].mesh.vertices;
    }

    // Update is called once per frame
    void Update()
    {
        if(rotZ < 360.0f * 2f) {
            rotZ += 0.1f;
            for(int i=4; i<8; i++){
                matrixes[i] = Transformaciones.RotateZ(rotZ)* mOriginals[i];
            }
        }
        else if(rotY < 360.0f) {
            rotY += 0.1f;
            matrixes[2] = Transformaciones.RotateY(rotY) * mOriginals[2];
            matrixes[3] = Transformaciones.RotateY(rotY) * mOriginals[3];
            matrixes[6] = Transformaciones.RotateY(rotY) * mOriginals[6];
            matrixes[7] = Transformaciones.RotateY(rotY) * mOriginals[7];
        }
        else {
            rotZ = rotY = 0;
            for(int i=4; i<8; i++){
                matrixes[i] = mOriginals[i];
            }
        }

        for(int i=0; i<8; i++)
            Cubes[i].mesh.vertices = Transformaciones.Transform(matrixes[i],originals);
    }
}
