using System;
using UnityEngine;

namespace NyanCat
{

    public class NyanSquare
    {
        //Static
        private static Texture2D[] textures = new Texture2D[10];
        private static Mesh squareMesh = new Mesh();

        static NyanSquare()
        {
            //Create mesh from verticies
            Vector3[] vertices = new Vector3[4];
            vertices[0] = new Vector3(0f, -0.5f, 0f);
            vertices[1] = new Vector3(0f, -0.5f, 1f);
            vertices[2] = new Vector3(0f, 0.5f, 0f);
            vertices[3] = new Vector3(0f, 0.5f, 1f);
            squareMesh.vertices = vertices;

            //Draw both sides of the square
            int[] triangles = new int[12];
            //Bottom left half clockwise
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
            //Top right half clockwise
            triangles[3] = 1;
            triangles[4] = 2;
            triangles[5] = 3;
            //Bottom left half counter-clockwise
            triangles[6] = 0;
            triangles[7] = 1;
            triangles[8] = 2;
            //Top right half counter-clockwise
            triangles[9] = 2;
            triangles[10] = 1;
            triangles[11] = 3;
            squareMesh.triangles = triangles;

            //Create UV co-ords
            Vector2[] uv = new Vector2[4];
            uv[0] = new Vector2(0f, 0f);
            uv[1] = new Vector2(1f, 0f);
            uv[2] = new Vector2(0f, 1f);
            uv[3] = new Vector2(1f, 1f);
            squareMesh.uv = uv;

            squareMesh.Optimize();
            squareMesh.RecalculateNormals();

            for (int textureID = 0; textureID < 10; textureID++)
            {
                //Draw texture
                float transparency = 1f - (textureID / 10f);
                textures[textureID] = new Texture2D(8, 8, TextureFormat.ARGB32, false);
                for (int x = 0; x < 8; x++)
                {
                    textures[textureID].SetPixel(x, 0, new Color(1f, 0.05f, 0.05f, transparency));
                    textures[textureID].SetPixel(x, 1, new Color(1f, 0.05f, 0.05f, transparency));
                    textures[textureID].SetPixel(x, 2, new Color(1f, 0.666f, 0.05f, transparency));
                    textures[textureID].SetPixel(x, 3, new Color(1f, 1f, 0.05f, transparency));
                    textures[textureID].SetPixel(x, 4, new Color(0.255f, 1f, 0.05f, transparency));
                    textures[textureID].SetPixel(x, 5, new Color(0.05f, 0.666f, 1f, transparency));
                    textures[textureID].SetPixel(x, 6, new Color(0.475f, 0.255f, 1f, transparency));
                    textures[textureID].SetPixel(x, 7, new Color(0.475f, 0.255f, 1f, transparency));
                }
                textures[textureID].Apply();
            }
        }

        //Instance
        private Vessel followVessel;
        private double latitude;
        private double longitude;
        private double altitude;
        private GameObject gameObject;
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;

        public NyanSquare(Vessel followVessel, double latitude, double longitude, double altitude)
        {
            this.followVessel = followVessel;
            this.latitude = latitude;
            this.longitude = longitude;
            this.altitude = altitude;
            gameObject = new GameObject();
            meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = squareMesh;
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material.color = Color.white;
            meshRenderer.material.mainTexture = textures[0];
            meshRenderer.material.shader = Shader.Find("Unlit/Transparent");
            gameObject.transform.localScale = new Vector3(1, 10, 5);
        }

        public void Update(NyanSquare nextSquare)
        {
            Vector3 startPos = followVessel.mainBody.GetWorldSurfacePosition(latitude, longitude, altitude);
            Vector3 endPos = followVessel.mainBody.GetWorldSurfacePosition(followVessel.latitude, followVessel.longitude, followVessel.altitude);
            if (nextSquare != null)
            {
                //Use current altitude so it remains level
                endPos = followVessel.mainBody.GetWorldSurfacePosition(nextSquare.latitude, nextSquare.longitude, nextSquare.altitude);
            }
            Vector3 direction = endPos - startPos;
            Vector3 up = FlightGlobals.getGeeForceAtPosition(startPos).normalized;
            gameObject.transform.position = startPos;
            gameObject.transform.rotation = Quaternion.LookRotation(direction.normalized, up);
            gameObject.transform.localScale = new Vector3(1, 10, direction.magnitude);
        }

        public void SetOpacity(int position)
        {
            meshRenderer.material.mainTexture = textures[position];
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
    }
}

