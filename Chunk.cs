using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace RedstoneSim
{
    public class Chunk
    {
        public Vector3i Position;
        public static Blocks _air = new Blocks(0, 0, 0);
        public static int rootSize = 16;

        public Blocks[,,] voxel = new Blocks[rootSize, rootSize, rootSize];


        public Chunk(Vector3i position)
        {
            this.Position = position;
            for (int _x = 0; _x < rootSize; _x++)
            {
                for (int _y = 0; _y < rootSize; _y++)
                {
                    for (int _z = 0; _z < rootSize; _z++)
                    {
                        this.voxel[_x, _y, _z] = new Blocks(0, 0, 0);
                    }
                }
            }
            
        }
        public static Vector3i BlockPositionInChunk(Vector3i position)
        {
            return new Vector3i(position.X % Chunk.rootSize, position.Y % Chunk.rootSize, position.Z % Chunk.rootSize);
        }

        public bool BreakBlock(Vector3i position)
        {
            this.voxel[position.X, position.Y, position.Z] = _air;
            return true; 
        }

        public bool PlaceBlock(Vector3i position, Blocks meta)
        {
            this.voxel[position.X, position.Y, position.Z] = meta;
            return true;
        }

        public bool BlockIsAir(Vector3i position)
        {
            return this.voxel[position.X, position.Y, position.Z].id == 0;
        }
    }
}
