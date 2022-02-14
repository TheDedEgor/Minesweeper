using System;
using System.Windows;

namespace Sapper.Models
{
    public class Cell
    {
        public sbyte Value;
        public Visibility Visibility;
    }
    public enum Difficulty : byte
    {
        Beginner = 10,
        Amateur = 40,
        Professional = 99
    }
    public class SapperField
    {
        public Cell[,] Field { get; }
        private Random r = new Random();
        public SapperField(Difficulty difficulty)
        {
            if (difficulty == Difficulty.Beginner)
            {
                Field = new Cell[9, 9];
                for (int i1 = 0; i1 < 9; i1++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        Field[i1, j] = new Cell();
                        Field[i1, j].Visibility = Visibility.Visible;
                    }
                }
            }
            else if (difficulty == Difficulty.Amateur)
            {
                Field = new Cell[16, 16];
                for (int i1 = 0; i1 < 16; i1++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        Field[i1, j] = new Cell();
                        Field[i1, j].Visibility = Visibility.Visible;
                    }
                }
            }
            else
            {
                Field = new Cell[30, 16];
                for (int i1 = 0; i1 < 30; i1++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        Field[i1, j] = new Cell();
                        Field[i1, j].Visibility = Visibility.Visible;
                    }
                }
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
            int idx = r.Next(0, imax);
            int jdx = r.Next(0, jmax);
            if (Field[idx, jdx].Value != -1)
            {
                Field[idx, jdx].Value = -1;
                #region Increment around values 
                try
                {
                    if (Field[idx - 1, jdx].Value != -1)
                        Field[idx - 1, jdx].Value++;
                }
                catch { }
                try
                {
                    if (Field[idx + 1, jdx].Value != -1)
                        Field[idx + 1, jdx].Value++;
                }
                catch { }
                try
                {
                    if (Field[idx - 1, jdx - 1].Value != -1)
                        Field[idx - 1, jdx - 1].Value++;
                }
                catch { }
                try
                {
                    if (Field[idx + 1, jdx + 1].Value != -1)
                        Field[idx + 1, jdx + 1].Value++;
                }
                catch { }
                try
                {
                    if (Field[idx, jdx - 1].Value != -1)
                        Field[idx, jdx - 1].Value++;
                }
                catch { }
                try
                {
                    if (Field[idx, jdx + 1].Value != -1)
                        Field[idx, jdx + 1].Value++;
                }
                catch { }
                try
                {
                    if (Field[idx - 1, jdx + 1].Value != -1)
                        Field[idx - 1, jdx + 1].Value++;
                }
                catch { }
                try
                {
                    if (Field[idx + 1, jdx - 1].Value != -1)
                        Field[idx + 1, jdx - 1].Value++;
                }
                catch { }
                #endregion
                return true;
            }
            return false;
        }
    }
}
