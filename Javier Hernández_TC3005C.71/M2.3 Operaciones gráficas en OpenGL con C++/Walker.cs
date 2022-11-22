using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Jesús Javier Hernández Delgado  A01658891.

public class Walker : MonoBehaviour
{
    public enum PARTS {
        RP_HEAP, RP_TORSO, RP_CHEST, RP_NECK, RP_HEAD,
        RP_RSHOULDER, RP_RFOREARM, RP_RELBOW, RP_RARM, RP_RHAND,
        RP_LSHOULDER, RP_LFOREARM, RP_LELBOW, RP_LARM, RP_LHAND,
        RP_RTHIG, RP_RKNEE, RP_RLEG, RP_RFOOT,
        RP_LTHIG, RP_LKNEE, RP_LLEG, RP_LFOOT
    };

    public struct BACK_FORTH {
        public float delta, dir, val, min, max;
        public BACK_FORTH(float _delta, float _dir, float _val, float _min, float _max) {
            delta = _delta;
            dir = _dir;
            val = _val;
            min = _min;
            max = _max;
        }

        public void Update() {
            val += delta * dir;
            if(val <= min || val >= max) dir = -dir;
        }
    };

    public static void INIT_PART(ref List<GameObject> parts, 
                                ref List<Matrix4x4> locations,
                                ref List<Matrix4x4> scales,
                                PrimitiveType type, 
                                int index, 
                                Color color, 
                                string name, 
                                Vector3 scale, 
                                Vector3 position) {
                        parts.Add(GameObject.CreatePrimitive(type));
                        //Debug.Log("Index: " + index);
                        parts[index].GetComponent<MeshRenderer>().material.SetColor("_Color",color);
                        parts[index].name = name;
                        parts[index].GetComponent<BoxCollider>().enabled = false;
                        scales.Add(Transformaciones.Scale(scale.x, scale.y, scale.z));
                        locations.Add(Transformaciones.Translate(position.x, position.y, position.z));
                    }

    List<GameObject> go_parts;
    List<Matrix4x4> m_locations;
    List<Matrix4x4> m_scales;
    Vector3[] v3_originals;
    BACK_FORTH rY;
    BACK_FORTH rX;
    BACK_FORTH jump;

    Arm leftArm;
    Arm rightArm;
    Leg leftLeg;
    Leg rightLeg;

    void Start() 
    {
        leftArm = gameObject.AddComponent<Arm>();
        rightArm = gameObject.AddComponent<Arm>();
        leftLeg = gameObject.AddComponent<Leg>();
        rightLeg = gameObject.AddComponent<Leg>();
        rY = new BACK_FORTH(0.02f, 1f, 0f, -3f, 3f);
        jump = new BACK_FORTH(0.0005f, 1f, 0f, -0.05f, 0.05f);
        rX = new BACK_FORTH(0.02f, 1f, 0f, 0f, 8f);
        go_parts = new List<GameObject>();
        m_scales = new List<Matrix4x4>();
        m_locations = new List<Matrix4x4>();
        //NEW HEAP
        INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_HEAP, Color.grey, "HEAP", new Vector3(1f, 0.5f, 1f), new Vector3(0f, 0f, 0f));
        v3_originals = go_parts[(int)PARTS.RP_HEAP].GetComponent<MeshFilter>().mesh.vertices;
        //NEW TORSO
        INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_TORSO, Color.white, "TORSO", new Vector3(1f, 0.75f, 1f), new Vector3(0f, 0.25f + 0.75f/2f, 0f));
        //NEW CHEST
        INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_CHEST, Color.red, "CHEST", new Vector3(1.2f, 0.4f, 1.2f), new Vector3(0f, 0.75f/2f + 0.2f, 0f));
        //NEW NECK
        INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_NECK, Color.white, "NECK", new Vector3(0.2f, 0.1f, 0.2f), new Vector3(0f, 0.2f + 0.1f/2f, 0f));
        //NEW HEAD
        INIT_PART(ref go_parts, ref m_locations, ref m_scales, PrimitiveType.Cube, (int)PARTS.RP_HEAD, Color.blue, "HEAD", new Vector3(0.5f, 0.5f, 0.5f), new Vector3(0f, 0.1f/2f + 0.5f/2f, 0f));
        
        leftArm.Init("RIGHT", ref go_parts, ref m_locations, ref m_scales);
        rightArm.Init("LEFT", ref go_parts, ref m_locations, ref m_scales);
        leftLeg.Init("RIGHT", ref go_parts, ref m_locations, ref m_scales);
        rightLeg.Init("LEFT", ref go_parts, ref m_locations, ref m_scales);
    }

    void Update() 
    {
        rY.Update();
        jump.Update();
        rX.Update();

        Matrix4x4 accumT = Matrix4x4.identity; //Para heredar las transformaciones en la jerarquía (excepto escala).
        Matrix4x4 accumChest = Matrix4x4.identity;
        Matrix4x4 accumHeap = Matrix4x4.identity;

        for(int i = (int)PARTS.RP_HEAP; i <= (int)PARTS.RP_HEAD; i++) {
            Matrix4x4 m = accumT * m_locations[i] * m_scales[i];
            if(i == (int)PARTS.RP_HEAP) {
                Matrix4x4 t = Transformaciones.Translate(0f, jump.val, 0f);
                m = accumT * m_locations[i] * t * m_scales[i];
                accumT *= m_locations[i] * t;
                accumHeap = accumT;
                leftLeg.Draw(ref accumHeap, ref go_parts, m_locations, m_scales, rY, v3_originals);
                rightLeg.Draw(ref accumHeap, ref go_parts, m_locations, m_scales, rY, v3_originals);
            }
            else if(i == (int)PARTS.RP_CHEST) {
                Matrix4x4 r = Transformaciones.RotateY(rY.val);
                m = accumT * m_locations[i] * r * m_scales[i];
                accumT *= m_locations[i] * r;
                accumChest = accumT;
                leftArm.Draw(ref accumChest, ref go_parts, m_locations, m_scales, rY, v3_originals);
                rightArm.Draw(ref accumChest, ref go_parts, m_locations, m_scales, rY, v3_originals);
            }
            else if(i == (int)PARTS.RP_NECK) {
                Matrix4x4 r = Transformaciones.RotateY(-rY.val);
                m = accumT * m_locations[i] * r * m_scales[i];
                accumT *= m_locations[i] * r;
            }
            else {
                accumT *= m_locations[i];
            }
            go_parts[i].GetComponent<MeshFilter>().mesh.vertices = Transformaciones.Transform(m, v3_originals);
        }  
    }
}
