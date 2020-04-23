using System;
using System.Collections.Generic;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common.Input;

namespace RedstoneSim
{
	public class World
	{
		public BlockModel blockModel;

		public Dictionary<Vector3i, Chunk> chunks;

		public World()
		{
			 this.blockModel = new BlockModel();
			//Vector3i is position in CHUNK basis
			 this.chunks = new Dictionary<Vector3i, Chunk>();
		}

		public static Vector3i ChunkPosition(Vector3i position)
		{
			int log = Math.ILogB(Chunk.rootSize);
			return new Vector3i(position.X >> log , position.Y >> log, position.Z >> log);
		}


		public bool AddChunk(Chunk chunk)
		{
			if (chunk != null){
				if (chunks.ContainsKey(chunk.Position))
				{
					return false;
				}
				else
				{
					chunks.Add(chunk.Position, chunk);
					return true;
				}
			}
			return false;
		}

		public bool ChunkExists(Vector3i position)
		{
			Chunk _chunk;
			return this.chunks.TryGetValue(ChunkPosition(position), out _chunk);
		}

		public bool BreakBlock(Vector3i position)
		{
			Vector3i _cp = ChunkPosition(position);
			Vector3i _bp = Chunk.BlockPositionInChunk(position);
			if (ChunkExists(_cp))
			{
				Chunk _chunk = this.chunks[_cp];
				_chunk.BreakBlock(_bp);
				return true;
			}
			return false;
		}

		public bool PlaceBlock(Vector3i position, Blocks block)
		{
			Vector3i _cp = ChunkPosition(position);
			Vector3i _bp = Chunk.BlockPositionInChunk(position);
			if (ChunkExists(_cp))
			{
				Chunk _chunk = this.chunks[_cp];
				return _chunk.PlaceBlock(_bp, block);
			}
			return false;
		}
	}
}
