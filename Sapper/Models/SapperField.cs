using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Sapper.Infrastructure.Commands;
using Sapper.ViewModels.Base;

namespace Sapper.Models
{
    public enum Difficulty : byte
    {
        Beginner = 10,
        Amateur = 40,
        Professional = 99
    }
    
    internal class SapperField
    {
        public readonly sbyte[,] Field;

        internal readonly Difficulty Difficulty;

        private Random r = new Random();

        public SapperField(Difficulty difficulty)
        {
            Difficulty = difficulty;
            if (difficulty == Difficulty.Beginner)
                Field = new sbyte[9, 9];
            else if (difficulty == Difficulty.Amateur)
                Field = new sbyte[16, 16];
            else
                Field = new sbyte[16, 30];
            int i = 0;
            int imax = Field.GetLength(0);
            int jmax = Field.GetLength(1);
            while (i < (int)difficulty)
            {
                if(SetMine(imax,jmax))
                    i++;
            }
        }

        private bool SetMine(int imax, int jmax)
        {
            int idx = r.Next(0, imax);
            int jdx = r.Next(0, jmax);
            if (Field[idx, jdx] != -1) 
            {
                Field[idx, jdx] = -1;
                for (int i = idx - 1; i < idx + 2; i++)
                {
                    for (int j = jdx - 1; j < jdx + 2; j++)
                    {
                        try
                        {
                            if (Field[i, j] == -1)
                                continue;
                            Field[i, j]++;
                        }
                        catch { }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
