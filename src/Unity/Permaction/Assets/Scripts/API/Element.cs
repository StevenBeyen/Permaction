using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace API
{
    [System.Serializable]
    public class Element : IComparable<Element>
    {
        public int id;
        public string name;
        public string category;
        public Coordinate[] coordinates;
        public int counter;
        // TODO Use coordinates array as fixed elements data

        private int minX, minZ, maxX, maxZ;
        private float minY, maxY;
        private float midY;

        public Element(int id)
        {
            this.id = id;
            this.counter = 1;
        }

        public void Sync()
        {
            float height;
            minX = coordinates[0].x;
            maxX = minX;
            minZ = coordinates[0].y;
            maxZ = minZ;
            minY = float.Parse(UserData.terrain_heightmap[minX][minZ]);
            maxY = minY;
            foreach (Coordinate c in coordinates)
            {
                if (c.x < minX)
                    minX = c.x;
                if (c.x > maxX)
                    maxX = c.x;
                if (c.y < minZ)
                    minZ = c.y;
                if (c.y > maxZ)
                    maxZ = c.y;
                height = float.Parse(UserData.terrain_heightmap[c.x][c.y]);
                if (height < minY)
                    minY = height;
                if (height > maxY)
                    maxY = height;
            }
            midY = minY + (maxY - minY) / 2;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(minX, midY, minZ);
        }

        public Vector3 GetScale()
        {
            float heightScale = ((maxX - minX + 1) + (maxZ - minZ + 1)) / 2;
            return new Vector3((maxX - minX + 1), heightScale, (maxZ - minZ + 1));
        }

        public float[,] GetHeights()
        {
            int xRange = maxX - minX + 2;
            int zRange = maxZ - minZ + 2;
            float[,] heights = new float[zRange,xRange];
            for (int z = 0; z < zRange; ++z)
            {
                for (int x = 0; x < xRange; ++x)
                {
                    heights[z,x] = midY;
                }
            }
            return heights;
        }

        int IComparable<Element>.CompareTo(Element element)
        {
            if (this.category == element.category)
                return this.name.CompareTo(element.name);
            else
                return this.category.CompareTo(element.category);
        }

        public override string ToString()
        {
            return "id: " + id + ", name: " + name + ", category: " + category; 
        }
    }
}
