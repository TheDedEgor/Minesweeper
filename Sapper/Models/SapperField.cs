using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Sapper.Data;
using Sapper.Infrastructure.Commands;
using Sapper.ViewModels.Base;

namespace Sapper.Models
{
    internal class SapperField
    {
        public readonly sbyte[,] Field;

        internal readonly Difficulty Difficulty;

        public int FontSize { get; private set; }

        private Random rand = new Random();

        public SapperField(Difficulty difficulty)
        {
            Difficulty = difficulty;
            if (difficulty == Difficulty.Beginner)
            {
                Field = new sbyte[9, 9];
                FontSize = 22;
            }
            else if (difficulty == Difficulty.Intermediate)
            {
                Field = new sbyte[16, 16];
                FontSize = 18;
            }
            else
            {
                Field = new sbyte[16, 30];
                FontSize = 17;
            }
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
            int idx = rand.Next(0, imax);
            int jdx = rand.Next(0, jmax);
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
