﻿using System;
namespace OpcodeTools
{
    public abstract class FormulasBase
    {
        public abstract uint CalcCryptedFromOpcode(uint opcode);
        public abstract uint CalcSpecialFromOpcode(uint opcode);
        public abstract uint CalcAuthFromOpcode(uint opcode);
        protected abstract bool NormalCheck(uint opcode);
        protected abstract bool SpecialCheck(uint opcode);
        protected abstract bool AuthCheck(uint opcode);
        protected virtual uint BaseOffset { get { return 1376; } }
        protected bool HasSpecialGroups;
        protected string SpecialGroupName;

        public FormulasBase()
        {
            this.HasSpecialGroups = false;
            this.SpecialGroupName = "None";
        }

        public bool IsAuthOpcode(uint opcode)
        {
            return AuthCheck(opcode);
        }

        public bool IsNormalOpcode(uint opcode)
        {
            return !IsSpecialOpcode(opcode) && !IsAuthOpcode(opcode) && NormalCheck(opcode);
        }
        
        public bool IsSpecialOpcode(uint opcode)
        {
            return !IsAuthOpcode(opcode) && SpecialCheck(opcode);
        }

        public bool HasBuildSpecialGroups()
        {
            return HasSpecialGroups;
        }

        public string GetSpecialGroupName()
        {
            return SpecialGroupName;
        }

        public uint CalcOffsetFromOpcode(uint opcode)
        {
            uint crypted = CalcCryptedFromOpcode(opcode);
            return (crypted * 4) + BaseOffset;
        }

        public uint CalcOpcodeFromSpecial(uint offset)
        {
            for (uint i = 1; i < 0xFFFF; ++i)
            {
                if (IsSpecialOpcode(i))
                {
                    if (CalcSpecialFromOpcode(i) == offset)
                        return i;
                }
            }
            return 0;
        }

        public uint CalcOpcodeFromOffset(uint offset)
        {
            for (uint i = 1; i < 0xFFFF; ++i)
            {
                if (IsNormalOpcode(i))
                {
                    if (CalcOffsetFromOpcode(i) == offset)
                        return i;
                }
            }
            return 0;
        }

        public uint CalcOpcodeFromCrypted(uint crypted)
        {
            return CalcOpcodeFromOffset((crypted * 4) + BaseOffset);
        }

        public uint CalcOpcodeFromAuth(uint auth)
        {
            for (uint i = 1; i < 0xFFFF; ++i)
            {
                if (IsAuthOpcode(i) &&
                    CalcAuthFromOpcode(i) == auth)
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
