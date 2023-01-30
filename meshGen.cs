using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class meshGen : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3[] vertices;
    int[] triangles;
    Color[] colors;
    Mesh mesh;
    private MeshCollider meshCollider;
    public int xSize = 10;
    public int zSize = 10;
    public int x=0;
    public int z=0;
    public Gradient gradient;
    public float alpha = 0.25f;
    public float n = 1;
    public float freq = 1;
    public float temp = 1;
    public float p = 1f;
    public float q= 1f;
    public float speed = 100;
    public float gap = 1f;
    public bool redo =false;
    float MinTerrainHeight;
    float MaxTerrainHeight;
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        x = -xSize / 2;
        z = -zSize / 2;

        createShape();

        


    }
    private void Update()
    {
        meshCollider = GetComponent<MeshCollider>();
        n +=0.01f*Time.deltaTime/speed;

        StartCoroutine(updateShape());
   
        
        if(redo==true)
        {
            createShape();
        }
        if (redo == false)
        {
            StartCoroutine(updateShape());
        }
        UpdateMesh();

    }
    int coord(int x,int y)
    {

        return (x+((xSize + 1)/2)) + ((xSize + 1) ) * ((zSize/2)+y);
    }

    IEnumerator updateShape()
    {
        
        {
            if (x > ((xSize + 1) / 2) || x < -((xSize + 1) / 2))
            {
                x = (x / Mathf.Abs(x)) * ((xSize + 1) / 2);
            }
            if (z > ((zSize + 1) / 2) || z < -((zSize + 1) / 2))
            {
                z = (z / Mathf.Abs(z)) * ((zSize + 1) / 2);
            }
        }
        
        for (int i = x; i <= (xSize) / 2; i++)
        {
            for (int j = z; j <= (zSize) / 2; j++)
            {
                if(i== (xSize+1) / 2 || j== (zSize+1) / 2 || i==-(xSize + 1) / 2||j== -(zSize+1) / 2)
                {
                    vertices[coord(i, j)].y = temp;
                }
                else
                {
                    vertices[coord(i, j)].y = ((1-4*alpha)* vertices[coord(i, j)].y)+ (vertices[coord(i-1, j)].y + vertices[coord(i+1, j)].y + vertices[coord(i, j-1)].y + vertices[coord(i, j+1)].y)*alpha;
                }






                {
                    if (vertices[coord(i, j)].y > MaxTerrainHeight)
                    {
                        MaxTerrainHeight = vertices[coord(i, j)].y;
                    }
                    if (vertices[coord(i, j)].y < MinTerrainHeight)
                    {
                        MinTerrainHeight = vertices[coord(i, j)].y;
                    }
                }
                
            }
           
        }
        yield return new WaitForSeconds(speed);
        colors = new Color[vertices.Length];
        for (int z = 0, i = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {

                float height = Mathf.InverseLerp(MinTerrainHeight, MaxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
        
      
    }
    
    void createShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int z = 0,i=0; z < zSize+1; z++)
        {
            for(int  x=0; x < xSize+1; x++)
            {

                
                vertices[i] = new Vector3(x,0,z);
                vertices[i].y= gap * Mathf.Sin(p+freq*vertices[i].x) * Mathf.Cos(q+freq * vertices[i].z);
                i++;
            } 
        }


        triangles = new int[xSize * zSize * 2*3];
        int verts = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[0 + tris] = verts + 0;
                triangles[1 + tris] = verts + xSize + 1;
                triangles[2 + tris] = verts + 1;
                triangles[3 + tris] = verts + 1;
                triangles[4 + tris] = verts + xSize + 1;
                triangles[5 + tris] = verts + xSize + 2;
                verts++;
                tris += 6;

            }
            verts++;
        }
        colors = new Color[vertices.Length];
        for (int z = 0, i = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {

                float height = Mathf.InverseLerp(MinTerrainHeight, MaxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }


    }



    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(vertices[0], 1f);
        Gizmos.DrawSphere(vertices[vertices.Length-1], 1f);
        Gizmos.color = Color.red;
    }

}
