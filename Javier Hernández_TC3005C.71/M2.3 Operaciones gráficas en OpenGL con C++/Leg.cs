using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Walker;

//Jesús Javier Hernández Delgado  A01658891.

public class Leg : MonoBehaviour
{
    string side;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(string _side, ref List<GameObject> go_parts, ref List<Matrix4x4> m_locations, ref List<Matrix4x4> m_scales) {
        if(_side == "LEFT") {
            side = "LEFT";
            //L_THIG
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LTHIG, Color.white, "LTHIG", new Vector3(0.5f, 0.9f, 0.5f), new Vector3(-0.55f/2f, -0.7f, 0f));
            //L_KNEE
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LKNEE, Color.blue, "LKNEE", new Vector3(0.5f, 0.3f, 0.5f), new Vector3(0f, -0.7f/2f - 0.3f/2f, 0f));
            //L_LEG
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LLEG, Color.blue, "LLEG", new Vector3(0.5f, 0.8f, 0.5f), new Vector3(0f, -0.3f/2f - 0.8f/2f, 0f));
            //L_FOOT
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_LFOOT, Color.blue, "LFOOT", new Vector3(0.5f, 0.3f, 0.8f), new Vector3(0f, -0.8f/2f - 0.3f/2f, -0.15f));
        }
        else if(_side == "RIGHT") {
            side = "RIGHT";
            //R_THIG
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RTHIG, Color.white, "RTHIG", new Vector3(0.5f, 0.9f, 0.5f), new Vector3(0.55f/2f, -0.7f, 0f));
            //R_KNEE
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RKNEE, Color.blue, "RKNEE", new Vector3(0.5f, 0.3f, 0.5f), new Vector3(0f, -0.7f/2f - 0.3f/2f, 0f));
            //R_LEG
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RLEG, Color.blue, "RLEG", new Vector3(0.5f, 0.8f, 0.5f), new Vector3(0f, -0.3f/2f - 0.8f/2f, 0f));
            //R_FOOT
            INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_RFOOT, Color.blue, "RFOOT", new Vector3(0.5f, 0.3f, 0.8f), new Vector3(0f, -0.8f/2f - 0.3f/2f, -0.15f));
        } 
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Draw(ref Matrix4x4 heapMatrix, ref List<GameObject> go_parts, List<Matrix4x4> m_locations, List<Matrix4x4> m_scales, BACK_FORTH rX, Vector3[] v3_originals) {
        Matrix4x4 accumT = Matrix4x4.identity;

        if(side == "LEFT") {
            for(int i = (int)PARTS.RP_LTHIG; i <= (int)PARTS.RP_LFOOT; i++) {
                Matrix4x4 m = accumT * m_locations[i] * m_scales[i];
                if(i == (int)PARTS.RP_LTHIG) {
                    accumT = heapMatrix;
                    m = accumT * m_locations[i] * m_scales[i];
                    accumT *= m_locations[i];
                }
                else if(i == (int)PARTS.RP_LKNEE) {
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
            for(int i = (int)PARTS.RP_RTHIG; i <= (int)PARTS.RP_RFOOT; i++) {
                Matrix4x4 m = accumT * m_locations[i] * m_scales[i];
                if(i == (int)PARTS.RP_RTHIG) {
                    accumT = heapMatrix;
                    m = accumT * m_locations[i] * m_scales[i];
                    accumT *= m_locations[i];
                }
                else if(i == (int)PARTS.RP_RKNEE) {
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
