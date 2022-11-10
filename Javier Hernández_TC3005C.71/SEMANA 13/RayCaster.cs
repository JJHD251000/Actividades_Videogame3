using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//librería para llamar a la función File.WriteAllBytes.
using System.IO;

//Jesús Javier Hernández Delgado.  A01658891.

public class RayCaster : MonoBehaviour
{
    public Vector3 Ka;
    public Vector3 Kd;
    public Vector3 Ks;
    public Vector3 Ia;
    public Vector3 Id;
    public Vector3 Is;
    public Vector3 PoI;
    public Vector3 N;
    public Vector3 LUZ;
    public Vector3 CAMARA;
    public float ALPHA;
    public float SR;
    public Vector3 SC;

    public GameObject RenderPlane;
    public Texture2D BackRender;
    public Vector2 CameraResolution;

    public Vector3 SecPoI;
    public Vector3 contact;
    
    Vector3 Cast(Vector3 coordenadas) {
        Camera came = Camera.main;

        float frustumHeight = 2.0f * came.nearClipPlane * Mathf.Tan(came.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * came.aspect;
        float pixelWidth = frustumWidth / 480;
        float pixelHeight = frustumHeight / 640;

        Vector3 center = FindTopLeftFrustrumNear();
        center += +(pixelWidth / 2f) * came.transform.right; Debug.Log(pixelWidth.ToString("F5"));
        center -= (pixelHeight / 2f) * came.transform.up; Debug.Log(pixelHeight.ToString("F5"));

        center += (pixelWidth) * came.transform.right * coordenadas.x;
        center -= (pixelHeight) * came.transform.up * coordenadas.y;

        return center;
    }

    // Start is called before the first frame update
    void Start()
    {
        var Texture = new Texture2D(Mathf.RoundToInt(CameraResolution.x), Mathf.RoundToInt(CameraResolution.y), TextureFormat.ARGB32, false);

        Vector3 i = Illumination(N);
        Debug.Log(i.ToString("F5"));

        GameObject esfera = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        esfera.transform.position = SC;
        esfera.transform.localScale = new Vector3(SR*2f, SR*2f, SR*2f);
        Renderer rend = esfera.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_Color", new Color(Kd.x, Kd.y, Kd.z));
        rend.material.SetColor("_SpecColor", new Color(Ks.x, Ks.y, Ks.z));

        GameObject luzPuntual = new GameObject("ThePointLight");
        Light lightComp = luzPuntual.AddComponent<Light>();
        lightComp.type = LightType.Point;
        lightComp.color = new Color(Id.x, Id.y, Id.z);
        lightComp.intensity = 20;
        Camera.main.transform.position = CAMARA;
        //Camera.main.transform.LookAt(SecPoI);

        //comprobar frustum
        GameObject esfera2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        esfera2.transform.position = FindTopLeftFrustrumNear();
        esfera2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //Renderer rend2 = esfera2.GetComponent<Renderer>();
        //rend2.material.shader = Shader.Find("Specular");
        //rend2.material.SetColor("_Color", new Color(1, 0, 1));
        //rend2.material.SetColor("_SpecColor", new Color(1, 1, 1));

        for (int y = 0; y < CameraResolution.y; y++) {
            for (int x = 0; x < CameraResolution.x; x++) {
                Color bg = BackRender.GetPixel(x, y);
                Texture.SetPixel(x, y, bg);
            }
        }
        Texture.Apply();

        for (int y = 0; y < CameraResolution.y; y++) {
            for (int x = 0; x < CameraResolution.x; x++) {
                Color color = GetPixel(new Vector3(x, y, 0f));
                if (color != Color.clear) 
                    Texture.SetPixel(x, -y, color);
            }
        }
        Texture.Apply();


        Texture.filterMode = FilterMode.Point;
        Renderer rend2 = RenderPlane.GetComponent<Renderer>();
        Shader shader = Shader.Find("Unlit/Texture");
        rend2.material.shader = shader;
        rend2.material.mainTexture = Texture;
        byte[] bytes = Texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/../render.png", bytes);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 c = Camera.main.transform.position;
        Debug.DrawLine(c, contact, Color.blue);

        Vector3 l = LUZ - SecPoI;
        Vector3 lp = N * Vector3.Dot(N.normalized, l);
        Vector3 lo = l - lp;
        Vector3 r = lp - lo;
        Vector3 v = CAMARA - SecPoI;

        Debug.DrawLine(PoI, l + PoI, Color.red);
        Debug.DrawLine(PoI, r + PoI, Color.magenta);
        Debug.DrawLine(PoI, N + PoI, Color.green);
        Debug.DrawLine(PoI, v + PoI, Color.blue);
    }

    Vector3 Illumination(Vector3 SecPoI)
    {
        Vector3 A = new Vector3(Ka.x * Ia.x, Ka.y * Ia.y, Ka.z * Ia.z);
        Vector3 D = new Vector3(Kd.x * Id.x, Kd.y * Id.y, Kd.z * Id.z);
        Vector3 S = new Vector3(Ks.x * Is.x, Ks.y * Is.y, Ks.z * Is.z);

        Vector3 l = LUZ - SecPoI;
        Vector3 v = CAMARA - SecPoI;
        float dotNuLu = Vector3.Dot(N.normalized, l.normalized);
        float dotNuL = Vector3.Dot(N.normalized, l);

        N = SecPoI - SC;

        Vector3 lp = N * dotNuL;
        Vector3 lo = l - lp;
        Vector3 r = lp - lo;
        D *= dotNuLu;
        S *= Mathf.Pow(Vector3.Dot(v.normalized,r.normalized),ALPHA);
        return A + D + S;
    }

    Color GetPixel(Vector3 coordenadas) {
        
        Vector3 center = Cast(coordenadas);

        Vector3 u = (center - CAMARA);
        u = u.normalized;
        Vector3 oc = CAMARA - SC;

        float NABLA = (Vector3.Dot(u, oc) * Vector3.Dot(u, oc)) - ((oc.magnitude * oc.magnitude) - (SR * SR));

        if (NABLA < 0) {
            return Color.clear;
        }

        float dpos = -1 * Vector3.Dot(u, oc) + Mathf.Sqrt(NABLA);
        float dneg = -1 * Vector3.Dot(u, oc) - Mathf.Sqrt(NABLA);

        Vector3 color = new Vector3();

        if (Mathf.Abs(dpos) < Mathf.Abs(dneg)) {
            color = Illumination(CAMARA + dpos * u);
        }
        else {
            color = Illumination(CAMARA + dneg * u);
        }

        return new Color(color.x, color.y, color.z);
    }

    Vector3 FindTopLeftFrustrumNear() {
        Camera came = Camera.main;
        //localizar camara
        Vector3 o = came.transform.position;
        //mover hacia adelante
        Vector3 p = o + came.transform.forward * came.nearClipPlane;
        //obtener dimenciones del frustum
        float frustumHeight = 2.0f * came.nearClipPlane * Mathf.Tan(came.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustumWidth = frustumHeight * came.aspect;
        //mover hacia arriba, media altura
        p += came.transform.up * frustumHeight / 2.0f;
        //mover a la izquierda, medio ancho
        p += came.transform.right * frustumWidth / 2.0f;
        return p;
    }
}
