using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;
using System.Text;

namespace RedstoneSim
{

    public class Blocks
    {

        public ushort id;
        public byte orien;
        public ulong data;

        public Blocks(ushort id, byte orien, ulong data)
        {
            this.id = id;
            this.orien = orien;
            this.data = data;
        }       

    }
}
