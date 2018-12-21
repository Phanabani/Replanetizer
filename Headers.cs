﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RatchetEdit.DataFunctions;
using OpenTK.Graphics.OpenGL;

namespace RatchetEdit
{
    class Headers
    {
    }

    public class TieModelHeader
    {
        public const int TIEELEMSIZE = 0x40; 

        public uint vertexPointer;
        public uint UVPointer;
        public uint indexPointer;
        public uint texturePointer;
        public int vertexCount;
        public short textureCount;
        public short modelID;

        public TieModelHeader(byte[] levelBlock, int offset)
        {
            //0x00 Unknown
            //0x04 Unknown
            //0x08 Unknown
            //0x0C Unknown
            vertexPointer =     ReadUint(levelBlock,    (offset * TIEELEMSIZE) + 0x10);
            UVPointer =         ReadUint(levelBlock,    (offset * TIEELEMSIZE) + 0x14);
            indexPointer =      ReadUint(levelBlock,    (offset * TIEELEMSIZE) + 0x18);
            texturePointer =    ReadUint(levelBlock,    (offset * TIEELEMSIZE) + 0x1C);
            //0x20 Unknown
            vertexCount =       ReadInt(levelBlock,     (offset * TIEELEMSIZE) + 0x24);
            textureCount =      ReadShort(levelBlock,   (offset * TIEELEMSIZE) + 0x28);
            //0x2C Unknown
            modelID =           ReadShort(levelBlock,   (offset * TIEELEMSIZE) + 0x30);
            //0x34 Unknown
            //0x38 Unknown
            //0x3C Unknown
        }
    }

    public class MobyModelHeader
    {
        const int MOBYHEADERSIZE = 0x20;

        public float modelSize;
        public int headSize;
        public int texCount;
        public int otherCount;
        public uint texBlockPointer;
        public uint otherBlockPointer;
        public uint vertPointer;
        public uint indexPointer;
        public ushort vertexCount;


        public MobyModelHeader(FileStream fs, uint offset)
        {
            headSize = ReadInt(ReadBlock(fs, offset, 4), 0);
            if(headSize > 0)
            {
                byte[] headBlock = ReadBlock(fs, offset, headSize + MOBYHEADERSIZE);

                //0x00 headSize
                //0x04 null
                //0x08 
                modelSize = ReadFloat(headBlock, 0x24);

                texCount =                  ReadInt(headBlock, headSize + 0x00);
                otherCount =                ReadInt(headBlock, headSize + 0x04);
                texBlockPointer = offset +  ReadUint(headBlock, headSize + 0x08);
                otherBlockPointer = offset+ ReadUint(headBlock, headSize + 0x0C);
                vertPointer = offset +      ReadUint(headBlock, headSize + 0x10);
                indexPointer = offset +     ReadUint(headBlock, headSize + 0x14);
                vertexCount =               ReadUshort(headBlock, headSize + 0x18);
            }
        }
    }

    public class TerrainHeader
    {
        public uint vertexPointer;
        public uint RGBAPointer;
        public uint UVPointer;
        public uint indexPointer;
        public uint vertexCount;
        public List<TerrainFragHeader> heads;

        public TerrainHeader(byte[] terrainBlock, int offset)
        {
            vertexPointer = ReadUint(terrainBlock, offset + 0x08);
            RGBAPointer =   ReadUint(terrainBlock, offset + 0x18);
            UVPointer =     ReadUint(terrainBlock, offset + 0x28);
            indexPointer =  ReadUint(terrainBlock, offset + 0x38);
            heads = new List<TerrainFragHeader>();
        }
    }

    public class TerrainFragHeader
    {
        const int ELEMSIZE = 0x30;

        public uint texturePointer;
        public int textureCount;
        public ushort vertexIndex;
        public ushort vertexCount;
        public ushort slotNum;

        public TerrainFragHeader(FileStream fs, byte[] terrainHeadBlock, int index)
        {
            texturePointer =    ReadUint(terrainHeadBlock,      (index * ELEMSIZE) + 0x10);
            textureCount =      ReadInt(terrainHeadBlock,       (index * ELEMSIZE) + 0x14);
            vertexIndex =       ReadUshort(terrainHeadBlock,    (index * ELEMSIZE) + 0x18);
            vertexCount =       ReadUshort(terrainHeadBlock,    (index * ELEMSIZE) + 0x1A);
            slotNum =           ReadUshort(terrainHeadBlock,    (index * ELEMSIZE) + 0x22);
        }
    }
}






