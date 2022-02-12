using System;

namespace Sapper
{
    public enum Difficulty : byte
    {
        Beginner = 10,
        Amateur = 40,
        Professional = 99
    }
    public class SapperField
    {
        public sbyte[,] Field { get; }
        private Random r = new Random();
        public SapperField(Difficulty difficulty)
        {
            if (difficulty == Difficulty.Beginner)
                Field = new sbyte[9, 9];
            else if (difficulty == Difficulty.Amateur)
                Field = new sbyte[16, 16];
            else
                Field = new sbyte[30, 16];
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
                #region Increment around values 
                try
                {
                    if (Field[idx - 1, jdx] != -1)
                        Field[idx - 1, jdx]++;
                }
                catch { }
                try
                {
                    if (Field[idx + 1, jdx] != -1)
                        Field[idx + 1, jdx]++;
                }
                catch { }
                try
                {
                    if (Field[idx - 1, jdx - 1] != -1)
                        Field[idx - 1, jdx - 1]++;
                }
                catch { }
                try
                {
                    if (Field[idx + 1, jdx + 1] != -1)
                        Field[idx + 1, jdx + 1]++;
                }
                catch { }
                try
                {
                    if (Field[idx, jdx - 1] != -1)
                        Field[idx, jdx - 1]++;
                }
                catch { }
                try
                {
                    if (Field[idx, jdx + 1] != -1)
                        Field[idx, jdx + 1]++;
                }
                catch { }
                try
                {
                    if (Field[idx - 1, jdx + 1] != -1)
                        Field[idx - 1, jdx + 1]++;
                }
                catch { }
                try
                {
                    if (Field[idx + 1, jdx - 1] != -1)
                        Field[idx + 1, jdx - 1]++;
                }
                catch { }
                #endregion
                return true;
            }
            return false;
        }
    }
}
