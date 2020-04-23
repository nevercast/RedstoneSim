using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSim
{

	class Mesh
	{

		public List<float> modelVertices = new List<float>();
		public List<uint> modelIndices = new List<uint>();

		public static Mesh GenerateMesh(World world)
		{
			List<float> verticeList = new List<float>();
			List<uint> indiceList = new List<uint>();

			Mesh mesh = new Mesh();

			float[] _mVertices;
			uint[] _mIndices;
			int chunkIndex = 0;
			int verticeIndex = 0;
			//
			foreach(var chunk in world.chunks)
			{
				for (int _y = 0; _y < chunk.Value.voxel.GetLength(1); _y++)
				{
					for (int _z = 0; _z < chunk.Value.voxel.GetLength(2); _z++)
					{
						for (int _x = 0; _x < chunk.Value.voxel.GetLength(0); _x++)
						{
							Blocks block = chunk.Value.voxel[_x, _y, _z];
							if (block.id > 0) {
								( _mVertices, _mIndices) = world.blockModel.model[block.id];
								for (int _vIndex = 0; _vIndex < _mVertices.Length / 5; _vIndex++)
								{
									verticeList.Add(_mVertices[_vIndex * 5 + 0] + _x);
									verticeList.Add(_mVertices[_vIndex * 5 + 1] + _y);
									verticeList.Add(_mVertices[_vIndex * 5 + 2] + _z);
									verticeList.Add(_mVertices[_vIndex * 5 + 3]);
									verticeList.Add(_mVertices[_vIndex * 5 + 4]);
								}
								for (int _iIndex = 0; _iIndex < _mIndices.Length; _iIndex++)
								{
									indiceList.Add((uint)(_mIndices[_iIndex] + verticeIndex));
								}
								verticeIndex += _mVertices.Length/5;
							}
						}
					}
				}
				chunkIndex++;
			}
			//
			mesh.modelVertices = verticeList;
			mesh.modelIndices = indiceList;

			return mesh;
		}
	}
}
