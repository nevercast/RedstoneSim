using System;
using OpenToolkit.Graphics.OpenGL;
using OpenToolkit.Mathematics;
using OpenToolkit.Windowing.Common;
using OpenToolkit.Windowing.Common.Input;
using OpenToolkit.Windowing.Desktop;
using OpenToolkit.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using System.Threading;
using RedstoneSim;
using System.Collections.Generic;
using MouseButton = OpenToolkit.Windowing.Common.Input.MouseButton;
using System.Linq;

namespace Redstonesim
{
	public class Engine : GameWindow
	{
		float[] _vertices = new float[16*16*16*24*5];
		uint[] _indices = new uint[16*16*16*36*3];

		//
		public List<int> BlockBreakEvents = new List<int>();
		public List<Vector3i> BlockUpdateEvents = new List<Vector3i>();
		public bool UpdateMesh = true;

		public World world = new World();
		public Physics physics = new Physics();

		public int _elementBufferObject;
		public int _vertexBufferObject;
		public int _vertexArrayObject;




		private Shader _shader;

		private Texture _texture;
		private Texture _texture2;

		private Camera _camera;

		private bool _firstMove = true;

		private Vector2 _lastPos;

		private double _time;

		public static Chunk GenerateChunk(Vector3i position)
		{
			Chunk _chunk = new Chunk(position);
			for (int _y = 0; _y < 1; _y++)
			{
				for (int _z = 0; _z < 16; _z++)
				{
					for (int _x = 0; _x < 16; _x++)
					{
						ushort id = 1;
						_chunk.voxel[_x, _y, _z].id = id;
					}
				}
			}
			return _chunk;
		}

		public bool ClearBuffers()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.BindVertexArray(0);
			GL.DeleteBuffer(_vertexBufferObject);
			GL.DeleteVertexArray(_vertexArrayObject);
			return true;
		}

		public Tuple<int, int, int> SetupBuffers(float[] vertices, uint[] indices)
		{

			int _elementBufferObject;
			int _vertexBufferObject;
			int _vertexArrayObject;

			_vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

			_elementBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

			_vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayObject);

			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArrayObject);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

			_shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
			_shader.Use();

			_texture = new Texture("Resources/white_wool.png");
			_texture.Use();

			//_texture2 = new Texture("Resources/awesomeface.png");
			//_texture2.Use(TextureUnit.Texture1);

			_shader.SetInt("texture0", 0);
			//_shader.SetInt("texture1", 1);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			//_vertexArrayObject = GL.GenVertexArray();
			//GL.BindVertexArray(_vertexArrayObject);

			//GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArrayObject);
			//GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

			var vertexLocation = _shader.GetAttribLocation("aPosition");
			GL.EnableVertexAttribArray(vertexLocation);
			GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

			var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
			GL.EnableVertexAttribArray(texCoordLocation);
			GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));


			return Tuple.Create(_elementBufferObject, _vertexBufferObject, _vertexArrayObject);
		}


		public Engine(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
			: base(gameWindowSettings, nativeWindowSettings)
		{
		}

		protected override void OnLoad()
		{
			//Load world

			//For now we generate chunk first
			Chunk _chunk = GenerateChunk(new Vector3i(0, 0, 0));
			world.AddChunk(_chunk);
			Mesh mesh = Mesh.GenerateMesh(world);
			UpdateMesh = false;

			_vertices = mesh.modelVertices.ToArray();
			Console.WriteLine(_vertices.Length);


			_indices = mesh.modelIndices.ToArray();
			Console.WriteLine(_indices.Length);


			GL.LoadBindings(new GLFWBindingsContext());

			GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
			GL.Clear(ClearBufferMask.AccumBufferBit | ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.FrontFace(FrontFaceDirection.Ccw);
			GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);


			(_elementBufferObject, _vertexBufferObject, _vertexArrayObject) = SetupBuffers(_vertices, _indices);
			//_vertexBufferObject = GL.GenBuffer();
			//GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			//GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

			//_elementBufferObject = GL.GenBuffer();
			//GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
			//GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

			//_shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
			//_shader.Use();

			//_texture = new Texture("Resources/white_wool.png");
			//_texture.Use();

			////_texture2 = new Texture("Resources/awesomeface.png");
			////_texture2.Use(TextureUnit.Texture1);

			//_shader.SetInt("texture0", 0);
			////_shader.SetInt("texture1", 1);

			//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			//GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

			////_vertexArrayObject = GL.GenVertexArray();
			////GL.BindVertexArray(_vertexArrayObject);

			////GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexArrayObject);
			////GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);

			//var vertexLocation = _shader.GetAttribLocation("aPosition");
			//GL.EnableVertexAttribArray(vertexLocation);
			//GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

			//var texCoordLocation = _shader.GetAttribLocation("aTexCoord");
			//GL.EnableVertexAttribArray(texCoordLocation);
			//GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

			// We initialize the camera so that it is 3 units back from where the rectangle is
			// and give it the proper aspect ratio
			_camera = new Camera(Vector3.UnitZ * 5, Size.X / (float)Size.Y);

			// We make the mouse cursor invisible so we can have proper FPS-camera movement
			CursorVisible = false;

			base.OnLoad();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			//Console.WriteLine(1.0f / e.Time);

			ClearBuffers();
			if (UpdateMesh)
			{
				UpdateMesh = false;
				Mesh mesh = Mesh.GenerateMesh(world);

				_vertices = mesh.modelVertices.ToArray();
				Console.WriteLine(_vertices.Length);


				_indices = mesh.modelIndices.ToArray();
				Console.WriteLine(_indices.Length);
			}
			(_elementBufferObject, _vertexBufferObject, _vertexArrayObject) = SetupBuffers(_vertices, _indices);


			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.BindVertexArray(_vertexArrayObject);

			_texture.Use();
			//_texture2.Use(TextureUnit.Texture1);
			//_shader.Use();

			var model = Matrix4.Identity; // * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time / 100));
			_shader.SetMatrix4("model", model);
			_shader.SetMatrix4("view", _camera.GetViewMatrix());
			_shader.SetMatrix4("projection", _camera.GetProjectionMatrix());


			GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

			SwapBuffers();

			base.OnRenderFrame(e);
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			if (!IsFocused)
			{
				return;
			}

			//For every block break event
			foreach(var _break in BlockBreakEvents)
			{
				Console.WriteLine("Breaking.1");
				List<Tuple<float, Vector3i>> distanceTo_brokenBlock = Physics.RayBlockIntersection(_camera.Position, _camera.Forward);
				//Order by smallest length til intersection
				distanceTo_brokenBlock = distanceTo_brokenBlock.OrderBy(i => i.Item1).ToList();
				foreach (var block in distanceTo_brokenBlock)
				{
					Console.WriteLine("Breaking.2");
					Vector3i position = block.Item2;
					if (world.ChunkExists(position))
					{
						Console.WriteLine("Breaking.3");
						Chunk chunk = world.chunks[World.ChunkPosition(position)];
						if(chunk.BlockIsAir(position))
						{
							continue;
						}
						else
						{
							Console.WriteLine("Breaking.4");
							chunk.BreakBlock(Chunk.BlockPositionInChunk(position));
							BlockUpdateEvents.Add(position);
							UpdateMesh = true;
							break;
						}
					}
				}
			}
			BlockBreakEvents = new List<int>();

			var input = KeyboardState;

			if (input.IsKeyDown(Key.Escape))
			{
				//Close();
				string consoleInput = Console.ReadLine();
				if(consoleInput.ToLower() == "exit")
				{
					Close();
				}
				//Console.WriteLine(consoleInput);
			}

			const float cameraSpeed = 0.005f;
			const float sensitivity = 0.2f;

			if (input.IsKeyDown(Key.W))
			{
				_camera.Position += _camera.Forward * cameraSpeed * (float)e.Time; // Forward
			}
			if (input.IsKeyDown(Key.S))
			{
				_camera.Position -= _camera.Forward * cameraSpeed * (float)e.Time; // Backwards
			}
			if (input.IsKeyDown(Key.A))
			{
				_camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
			}
			if (input.IsKeyDown(Key.D))
			{
				_camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
			}
			if (input.IsKeyDown(Key.Space))
			{
				_camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
			}
			if (input.IsKeyDown(Key.LShift))
			{
				_camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
			}

			var mouse = MousePosition;

			if (_firstMove)
			{
				_lastPos = new Vector2(mouse.X, mouse.Y);
				_firstMove = false;
			}
			else
			{
				var deltaX = mouse.X - _lastPos.X;
				var deltaY = mouse.Y - _lastPos.Y;
				_lastPos = new Vector2(mouse.X, mouse.Y);

				_camera.Yaw += deltaX * sensitivity;
				_camera.Pitch -= deltaY * sensitivity;
			}

			base.OnUpdateFrame(e);
		}

		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			if (e.Button == MouseButton.Left)
			{
				Console.WriteLine("Click");
				BlockBreakEvents.Add(1);
			}

			base.OnMouseDown(e);
		}

		protected override void OnMouseLeave()
		{
			MousePosition = (Size.X / 2f, Size.Y / 2f);
			_lastPos = (Size.X / 2f, Size.Y / 2f);

			base.OnMouseLeave();
		}

		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			_camera.Fov -= e.OffsetY;
			base.OnMouseWheel(e);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			GL.Viewport(0, 0, Size.X, Size.Y);
			_camera.AspectRatio = Size.X / (float)Size.Y;
			base.OnResize(e);
		}

		protected override void OnUnload()
		{
			//Remove buffers
			ClearBuffers();

			GL.UseProgram(0);
			GL.DeleteProgram(_shader.Handle);
			GL.DeleteTexture(_texture.Handle);
			//GL.DeleteTexture(_texture2.Handle);

			base.OnUnload();
		}
	}
}