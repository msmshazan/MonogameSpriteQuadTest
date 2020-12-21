using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using System;
using Mat3x2 =  System.Numerics.Matrix3x2;
using Microsoft.Xna.Framework.Graphics;

namespace QuadTest
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SpriteTransformVertex : IVertexType
    {
        public Vector3 Pos;
        public Vector2 Dim;
        public Vector2 UV;
        public Mat3x2 Transform;
        public Color Color;
        public static readonly VertexDeclaration VertexDeclaration;
        public SpriteTransformVertex(Vector3 position, Vector2 dimension, Vector2 uv, Mat3x2 transform, Color color)
        {
            Pos = position;
            Dim = dimension;
            UV = uv;
            Transform = transform;
            Color = color;
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Pos.GetHashCode();
                hashCode = (hashCode * 397) ^ Dim.GetHashCode();
                hashCode = (hashCode * 397) ^ UV.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"Pos: {Pos} , UV : {UV} , Transform: {Transform},Dim : {Dim},Color : {Color}";
        }

        public static bool operator ==(SpriteTransformVertex left, SpriteTransformVertex right)
        {
            return left.Pos == right.Pos
                && left.Dim == right.Dim
                && left.UV == right.UV
                && left.Transform == right.Transform
                && left.Color == right.Color;
        }

        public static bool operator !=(SpriteTransformVertex left, SpriteTransformVertex right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return this == (SpriteTransformVertex)obj;
        }

        static SpriteTransformVertex()
        {
            var elements = new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
                new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),
                new VertexElement(36, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 2),
                new VertexElement(44, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 3),

                new VertexElement(52, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            };
            VertexDeclaration = new VertexDeclaration(elements);
        }
    }
}
