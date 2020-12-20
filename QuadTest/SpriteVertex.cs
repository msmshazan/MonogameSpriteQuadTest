using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace QuadTest
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SpriteVertex : IVertexType
    {
        public Vector3 Pos;
        public Vector2 Dim;
        public Vector2 Scale;
        public Vector2 UV;
        public Color Color;
        public static readonly VertexDeclaration VertexDeclaration;
        public SpriteVertex(Vector3 position,Vector2 dimension,Vector2 uv, Vector2 scale,Color color)
        {
            Scale = scale;
            Pos = position;
            Dim = dimension;
            UV = uv;
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
                hashCode = (hashCode * 397) ^ Scale.GetHashCode();
                hashCode = (hashCode * 397) ^ UV.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"Pos: {Pos} , UV : {UV} , Scale: {Scale},Dim : {Dim},Color : {Color}";
        }

        public static bool operator ==(SpriteVertex left, SpriteVertex right)
        {
            return left.Pos == right.Pos
                && left.Dim == right.Dim
                && left.Scale == right.Scale
                && left.UV == right.UV
                && left.Color == right.Color;
        }

        public static bool operator !=(SpriteVertex left, SpriteVertex right)
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
            return this == (SpriteVertex)obj;
        }

        static SpriteVertex()
        {
            var elements = new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector2, VertexElementUsage.Position, 1),
                new VertexElement(20, VertexElementFormat.Vector2, VertexElementUsage.Position, 2),
                new VertexElement(28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(36, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            };
            VertexDeclaration = new VertexDeclaration(elements);
        }
    }
}