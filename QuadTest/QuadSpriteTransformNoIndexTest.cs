﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mat3x2 = System.Numerics.Matrix3x2;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Vector4 = Microsoft.Xna.Framework.Vector4;

namespace QuadTest
{
    public class QuadSpriteTransformNoIndexTest : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private VertexBufferBinding[] _vertexBindings;
        private VertexBuffer _vertexBuffer;
        private VertexBuffer _instanceData;
        private IndexBuffer _indexBuffer;
        private Effect _shader;

        public QuadSpriteTransformNoIndexTest()
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
            verts.Add(new SpriteTransformVertex( new Vector2(0, 0), new Vector2(1,1), Mat3x2.CreateScale(20) * Mat3x2.CreateRotation(MathHelper.PiOver4)*Mat3x2.CreateTranslation(120, 120), Color.White));
            verts.Add(new SpriteTransformVertex( new Vector2(0, 1), new Vector2(1,2), Mat3x2.CreateScale(30) * Mat3x2.CreateTranslation(120, 200) , Color.White));
            verts.Add(new SpriteTransformVertex( new Vector2(1, 0), new Vector2(2, 1), Mat3x2.CreateScale(50) * Mat3x2.CreateTranslation(200, 120) , Color.White));
            verts.Add(new SpriteTransformVertex( new Vector2(1, 1), new Vector2(2, 2), Mat3x2.CreateScale(70) * Mat3x2.CreateRotation(MathHelper.PiOver4) * Mat3x2.CreateTranslation(200, 200)  , Color.White));
            verts.Add(new SpriteTransformVertex( new Vector2(0,0), new Vector2(2,2), Mat3x2.CreateScale(200) * Mat3x2.CreateRotation(-MathHelper.PiOver4 / 2) * Mat3x2.CreateTranslation(400, 150) , Color.White));
            var elements = new []
            {
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 3),
            };
            var instanceVertexDeclaration = new VertexDeclaration(elements); 
            _instanceData = new VertexBuffer(GraphicsDevice,instanceVertexDeclaration,6,BufferUsage.None);
            _instanceData.SetData(new[]
            {
                new Vector2(-1, 1),
                new Vector2( 1, 1),
                new Vector2(-1,-1),
                new Vector2( 1,-1),
            });
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
            _vertexBindings = new VertexBufferBinding[2];
            _vertexBindings[0] = new VertexBufferBinding(_vertexBuffer, 0, 1);
            _vertexBindings[1] = new VertexBufferBinding(_instanceData, 0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _shader = Content.Load<Effect>("SpriteShaderTransformNoIndex");
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
            _shader.Parameters["Z"].SetValue(1.0f);
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