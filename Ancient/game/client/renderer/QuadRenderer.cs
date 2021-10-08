using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer
{
    class QuadRenderer
    {
        private GraphicsDevice GraphicsDevice;

        private VertexBuffer vb;
        private IndexBuffer ib;

        public QuadRenderer()
        {
            GraphicsDevice = Ancient.ancient.GraphicsDevice;

            VertexPosition[] vertices =
            {
                new VertexPosition(new Vector3(1, -1, -0.5F)),
                new VertexPosition(new Vector3(-1, -1, -0.5F)),
                new VertexPosition(new Vector3(-1, 1, -0.5F)),
                new VertexPosition(new Vector3(1, 1, -0.5F))
            };

            vb = new VertexBuffer(GraphicsDevice, VertexPosition.VertexDeclaration,
            vertices.Length, BufferUsage.None);
            vb.SetData<VertexPosition>(vertices);

            ushort[] indices = { 0, 1, 2, 2, 3, 0 };

            ib = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits,
           indices.Length, BufferUsage.None);
            ib.SetData<ushort>(indices);
        }

        public void Draw()
        {
            GraphicsDevice.SetVertexBuffer(vb);
            GraphicsDevice.Indices = ib;
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
        }

        public void ReadyBuffers()
        {
            GraphicsDevice.SetVertexBuffer(vb);
            GraphicsDevice.Indices = ib;
        }

        public void JustDraw()
        {
            GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
        }
    }
}
