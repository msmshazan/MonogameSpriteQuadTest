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
        public Vector2 UV0;
        public Vector2 UV1;
        public Mat3x2 Transform;
        public Color Color;
        public static readonly VertexDeclaration VertexDeclaration;
        public SpriteTransformVertex(Vector2 uv0, Vector2 uv1, Mat3x2 transform, Color color)
        {
            UV0 = uv0;
            UV1 = uv1;
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
                var hashCode = UV0.GetHashCode();
                hashCode = (hashCode * 397) ^ UV1.GetHashCode();
                hashCode = (hashCode * 397) ^ Transform.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"UV0: {UV0} , UV1 : {UV1} , Transform: {Transform},Color : {Color}";
        }

        public static bool operator ==(SpriteTransformVertex left, SpriteTransformVertex right)
        {
            return left.UV0 == right.UV0
                && left.UV1 == right.UV1
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
                new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
                new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.Tangent, 0),
                
                new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.Binormal, 0),
                new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.Binormal, 1),
                new VertexElement(32, VertexElementFormat.Vector2, VertexElementUsage.Binormal, 2),
                
                new VertexElement(40, VertexElementFormat.Color, VertexElementUsage.Color, 0)
            };
            VertexDeclaration = new VertexDeclaration(elements);
        }
    }
}
