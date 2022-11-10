using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Walker;

//Jesús Javier Hernández Delgado  A01658891.

public class Arm : MonoBehaviour
{
    string side;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(string _side, ref List<GameObject> go_parts, ref List<Matrix4x4> m_locations, ref List<Matrix4x4> m_scales) {
        if(_side == "LEFT") {
            side = "LEFT";
            //L_SHOULDER
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LSHOULDER, Color.red, "LSHOULDER", new Vector3(0.4f, 0.4f, 0.4f), new Vector3(-1.2f/2f - 0.4f/2f, 0f, 0f));
            //L_FOREARM
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LFOREARM, Color.white, "LFOREARM", new Vector3(0.4f, 0.5f, 0.4f), new Vector3(0f, -0.4f/2f - 0.5f/2f, 0f));
            //L_ELBOW
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LELBOW, Color.red, "LELBOW", new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0f, -0.5f/2f - 0.4f/2f, 0f));
            //L_ARM
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LARM, Color.red, "LARM", new Vector3(0.4f, 0.3f, 0.4f), new Vector3(0f, -0.4f/2f - 0.3f/2f, 0f));
            //L_HAND
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LHAND, Color.blue, "LHAND", new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0f, -0.3f/2f - 0.3f/2f, 0f));
        }
        else if(_side == "RIGHT") {
            side = "RIGHT";
            //R_SHOULDER
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RSHOULDER, Color.red, "RSHOULDER", new Vector3(0.4f, 0.4f, 0.4f), new Vector3(1.2f/2f + 0.4f/2f, 0f, 0f));
            //R_FOREARM
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RFOREARM, Color.white, "RFOREARM", new Vector3(0.4f, 0.5f, 0.4f), new Vector3(0f, -0.4f/2f - 0.5f/2f, 0f));
            //R_ELBOW
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RELBOW, Color.red, "RELBOW", new Vector3(0.4f, 0.4f, 0.4f), new Vector3(0f, -0.5f/2f - 0.4f/2f, 0f));
            //R_ARM
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RARM, Color.red, "RARM", new Vector3(0.4f, 0.3f, 0.4f), new Vector3(0f, -0.4f/2f - 0.3f/2f, 0f));
            //R_HAND
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RHAND, Color.blue, "RHAND", new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0f, -0.3f/2f - 0.3f/2f, 0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Draw(ref Matrix4x4 chestMatrix, ref List<GameObject> go_parts, List<Matrix4x4> m_locations, List<Matrix4x4> m_scales, BACK_FORTH rX, Vector3[] v3_originals) {
        Matrix4x4 accumT = Matrix4x4.identity;
        if(side == "LEFT") {
            for(int i = (int)PARTS.RP_LSHOULDER; i <= (int)PARTS.RP_LHAND; i++) {
                Matrix4x4 m = accumT * m_locations[i] * m_scales[i];
                if(i == (int)PARTS.RP_LSHOULDER) {
                    accumT = chestMatrix;
                    m = accumT * m_locations[i] * m_scales[i];
                    accumT *= m_locations[i];
                }
                else if(i == (int)PARTS.RP_LELBOW) {
                    Matrix4x4 t1 = Transformaciones.Translate(0f, -0.5f/2f, 0f);
                    Matrix4x4 t2 = Transformaciones.Translate(0f, -0.4f/2f, 0f);
                    Matrix4x4 r = Transformaciones.RotateX(-rX.val);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                else 
                    accumT *= m_locations[i];
                go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transformaciones.Transform(m, v3_originals);
            }
        }
        else if(side == "RIGHT") {
            for(int i = (int)PARTS.RP_RSHOULDER; i <= (int)PARTS.RP_RHAND; i++) {
                Matrix4x4 m = accumT * m_locations[i] * m_scales[i];
                if(i == (int)PARTS.RP_RSHOULDER) {
                    accumT = chestMatrix;
                    m = accumT * m_locations[i] * m_scales[i];
                    accumT *= m_locations[i];
                }
                else if(i == (int)PARTS.RP_RELBOW) {
                    Matrix4x4 t1 = Transformaciones.Translate(0f, -0.5f/2f, 0f);
                    Matrix4x4 t2 = Transformaciones.Translate(0f, -0.4f/2f, 0f);
                    Matrix4x4 r = Transformaciones.RotateX(rX.val);
                    m = accumT * t1 * r * t2 * m_scales[i];
                    accumT *= t1 * r * t2;
                }
                else 
                    accumT *= m_locations[i];
                go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transformaciones.Transform(m, v3_originals);
            }
        }
    }
}
