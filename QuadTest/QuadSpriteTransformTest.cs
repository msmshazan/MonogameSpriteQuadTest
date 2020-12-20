using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace QuadTest
{
    public class QuadSpriteTransformTest : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private VertexBufferBinding[] _vertexBindings;
        private VertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private Effect _shader;

        public QuadSpriteTransformTest()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var verts = new List<SpriteTransformVertex>();
            verts.Add(new SpriteTransformVertex(new Vector3(120, 120, 1), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),Matrix.CreateRotationZ(MathHelper.PiOver4) *Matrix.CreateScale(20) * Matrix.Identity, Color.White));
            verts.Add(new SpriteTransformVertex(new Vector3(120, 200, 1), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 1.5f), Matrix.CreateScale(30) * Matrix.Identity, Color.White));
            verts.Add(new SpriteTransformVertex(new Vector3(200, 120, 1), new Vector2(0.5f, 0.5f), new Vector2(1.5f, 0.5f), Matrix.CreateScale(50) * Matrix.Identity, Color.White));
            verts.Add(new SpriteTransformVertex(new Vector3(200, 200, 1), new Vector2(0.5f, 0.5f), new Vector2(1.5f, 1.5f), Matrix.CreateRotationZ(MathHelper.PiOver4) * Matrix.CreateScale(70) * Matrix.Identity, Color.White));
            verts.Add(new SpriteTransformVertex(new Vector3(400, 150, 1), new Vector2(1), new Vector2(1), Matrix.CreateRotationZ(-MathHelper.PiOver4/2) * Matrix.CreateScale(100) * Matrix.Identity, Color.White));
            var indices = new List<short>();
            for (int i = 0; i < verts.Count; i++)
            {
                indices.Add((short)(i + 0));
                indices.Add((short)(i + 1));
                indices.Add((short)(i + 2));
                indices.Add((short)(i + 1));
                indices.Add((short)(i + 3));
                indices.Add((short)(i + 2));
            }
            _vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(SpriteTransformVertex), verts.Count, BufferUsage.None);
            _indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, indices.Count, BufferUsage.None);
            _indexBuffer.SetData(indices.ToArray());
            _vertexBuffer.SetData(verts.ToArray());
            _vertexBindings = new VertexBufferBinding[1];
            _vertexBindings[0] = new VertexBufferBinding(_vertexBuffer, 0, 1);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _shader = Content.Load<Effect>("SpriteShaderTransform");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 2, 2);
            _pixel.SetData(new[]
            {
                Color.White,  Color.Red,Color.Green,Color.Blue
            });
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Indices = _indexBuffer;
            GraphicsDevice.SetVertexBuffers(_vertexBindings);
            _shader.Parameters["Texture"].SetValue(_pixel);
            _shader.Parameters["TextureDim"].SetValue(new Vector4(_pixel.Width, _pixel.Height, 1.0f / _pixel.Width, 1.0f / _pixel.Height));
            _shader.Parameters["MatrixTransform"].SetValue(Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, -1));
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            foreach (var pass in _shader.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, 2, _vertexBuffer.VertexCount);
            }

            base.Draw(gameTime);
        }


    }
}