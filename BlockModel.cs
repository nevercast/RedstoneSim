using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSim
{
    public class BlockModel
    {

        private class Cube
        {
            public static float[] vertices =
            {
                //Front
                //Top Left
                -0.5f, 0.5f, 0.5f, 0f, 0f,
                //Top Right
                0.5f, 0.5f, 0.5f, 1f, 0f,
                //Bottom Left
                -0.5f, -0.5f, 0.5f, 0f, 1f,
                //Bottom Right
                0.5f, -0.5f, 0.5f, 1f, 1f,

                //Left
                //Back top Left
                -0.5f, 0.5f, -0.5f, 0f, 0f,
                //Top Left
                -0.5f, 0.5f, 0.5f, 1f, 0f,
                //Back bottom Left
                -0.5f, -0.5f, -0.5f, 0f, 1f,
                //Bottom Left
                -0.5f, -0.5f, 0.5f, 1f, 1f,

                //Top
                //Back top Left
                -0.5f, 0.5f, -0.5f, 0f, 0f,
                //Back top Right
                0.5f, 0.5f, -0.5f, 1f,  0f,
                //Top Left
                -0.5f, 0.5f, 0.5f, 0f, 1f,
                //Top Right
                0.5f, 0.5f, 0.5f, 1f, 1f,

                //Right
                //Top Right
                0.5f, 0.5f, 0.5f, 0f, 0f,
                //Back top Right
                0.5f, 0.5f, -0.5f, 1f,  0f,
                //Bottom Right
                0.5f, -0.5f, 0.5f, 0f, 1f,
                //Back botttom right
                0.5f, -0.5f, -0.5f, 1f, 1f,    


                //Back
                //Back top Left
                -0.5f, 0.5f, -0.5f, 1f, 0f,
                //Back top Right
                0.5f, 0.5f, -0.5f, 0f,  0f,
                //Back bottom Left
                -0.5f, -0.5f, -0.5f, 1f, 1f,
                //Back botttom right
                0.5f, -0.5f, -0.5f, 0f, 1f,    

                //Bottom
                //Bottom Left
                -0.5f, -0.5f, 0.5f, 1, 1,
                //Bottom Right
                0.5f, -0.5f, 0.5f, 1, 0,
                //Back bottom Left
                -0.5f, -0.5f, -0.5f, 0, 1,
                //Back botttom right
                0.5f, -0.5f, -0.5f, 0, 0,
            };

            public static uint[] indices =
            {
                //CCW
                //Front
                0, 2, 3,
                3, 1, 0,
                //Left
                4, 6, 7,
                7, 5, 4,
                //Top
                8, 10, 11,
                11, 9, 8,
                //Right
                12, 14, 15,
                15, 13, 12,
                //Back
                16, 19, 18,
                19, 16, 17,
                //Bottom
                    
                20, 22, 23,
                20, 23, 21,
            };
        }

        private class Torch
        {
            public static float[] vertices =
{
                //Front
                //Top Left
                -0.25f, 0.25f, 0.25f, 0f, 0f,
                //Top Right
                0.25f, 0.25f, 0.25f, 1f, 0f,
                //Bottom Left
                -0.25f, -0.25f, 0.25f, 0f, 1f,
                //Bottom Right
                0.25f, -0.25f, 0.25f, 1f, 1f,

                //Left
                //Back top Left
                -0.25f, 0.25f, -0.25f, 0f, 0f,
                //Top Left
                -0.25f, 0.25f, 0.25f, 1f, 0f,
                //Back bottom Left
                -0.25f, -0.25f, -0.25f, 0f, 1f,
                //Bottom Left
                -0.25f, -0.25f, 0.25f, 1f, 1f,

                //Top
                //Back top Left
                -0.25f, 0.25f, -0.25f, 0f, 0f,
                //Back top Right
                0.25f, 0.25f, -0.25f, 1f,  0f,
                //Top Left
                -0.25f, 0.25f, 0.25f, 0f, 1f,
                //Top Right
                0.25f, 0.25f, 0.25f, 1f, 1f,

                //Right
                //Top Right
                0.25f, 0.25f, 0.25f, 0f, 0f,
                //Back top Right
                0.25f, 0.25f, -0.25f, 1f,  0f,
                //Bottom Right
                0.25f, -0.25f, 0.25f, 0f, 1f,
                //Back botttom right
                0.25f, -0.25f, -0.25f, 1f, 1f,    


                //Back
                //Back top Left
                -0.25f, 0.25f, -0.25f, 1f, 0f,
                //Back top Right
                0.25f, 0.25f, -0.25f, 0f,  0f,
                //Back bottom Left
                -0.25f, -0.25f, -0.25f, 1f, 1f,
                //Back botttom right
                0.25f, -0.25f, -0.25f, 0f, 1f,    

                //Bottom
                //Bottom Left
                -0.25f, -0.25f, 0.25f, 1, 1,
                //Bottom Right
                0.25f, -0.25f, 0.25f, 1, 0,
                //Back bottom Left
                -0.25f, -0.25f, -0.25f, 0, 1,
                //Back botttom right
                0.25f, -0.25f, -0.25f, 0, 0,
            };

            public static uint[] indices =
            {
                //CCW
                //Front
                0, 2, 3,
                3, 1, 0,
                //Left
                4, 6, 7,
                7, 5, 4,
                //Top
                8, 10, 11,
                11, 9, 8,
                //Right
                12, 14, 15,
                15, 13, 12,
                //Back
                16, 19, 18,
                19, 16, 17,
                //Bottom
                    
                20, 22, 23,
                20, 23, 21,
            };
        }

        public Tuple<float[], uint[]>[] model =
        { 
            Tuple.Create(new float[0], new uint[0]),
            Tuple.Create(Cube.vertices, Cube.indices),
            Tuple.Create(Torch.vertices, Torch.indices)
        };

        public BlockModel()
        {
            
        }

    }
}
