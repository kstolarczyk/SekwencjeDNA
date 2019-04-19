using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bioinformatyka
{
    class SequenceAlignment
    {
        public static void Align(ref string refSeq, ref string alineSeq)
        {
            int refSeqCnt = refSeq.Length + 1;
            int alineSeqCnt = alineSeq.Length + 1;

            int[,] scoringMatrix = new int[alineSeqCnt, refSeqCnt];

            //Initailization Step - filled with 0 for the first row and the first column of matrix
            for (int i = 0; i < alineSeqCnt; i++)
            {
                for (int j = 0; j < refSeqCnt; j++)
                {
                    scoringMatrix[i, j] = 0;
                }
                // scoringMatrix[i, 0] = 0; 
            }

            //Matrix Fill Step
            for (int i = 1; i < alineSeqCnt; i++)
            {
                for (int j = 1; j < refSeqCnt; j++)
                {
                    int scroeDiag = 0;
                    if (refSeq.Substring(j - 1, 1) == alineSeq.Substring(i - 1, 1))
                        scroeDiag = scoringMatrix[i - 1, j - 1] + 2;
                    else
                        scroeDiag = scoringMatrix[i - 1, j - 1] + -1;

                    int scroeLeft = scoringMatrix[i, j - 1] - 2;
                    int scroeUp = scoringMatrix[i - 1, j] - 2;

                    int maxScore = Math.Max(Math.Max(scroeDiag, scroeLeft), scroeUp);

                    scoringMatrix[i, j] = maxScore;
                }
            }


            //Console.ReadLine();

            //Traceback Step
            char[] alineSeqArray = alineSeq.ToCharArray();
            char[] refSeqArray = refSeq.ToCharArray();

            string AlignmentA = string.Empty;
            string AlignmentB = string.Empty;
            int m = alineSeqCnt - 1;
            int n = refSeqCnt - 1;
            while (m > 0 || n > 0)
            {
                int scroeDiag = 0;

                if (m == 0 && n > 0)
                {
                    AlignmentA = refSeqArray[n - 1] + AlignmentA;
                    AlignmentB = "-" + AlignmentB;
                    n = n - 1;
                }
                else if (n == 0 && m > 0)
                {
                    AlignmentA = "-" + AlignmentA;
                    AlignmentB = alineSeqArray[m - 1] + AlignmentB;
                    m = m - 1;
                }
                else
                {
                    //Remembering that the scoring scheme is +2 for a match, -1 for a mismatch, and -2 for a gap
                    if (alineSeqArray[m - 1] == refSeqArray[n - 1])
                        scroeDiag = 2;
                    else
                        scroeDiag = -1;

                    if (m > 0 && n > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n - 1] + scroeDiag)
                    {
                        AlignmentA = refSeqArray[n - 1] + AlignmentA;
                        AlignmentB = alineSeqArray[m - 1] + AlignmentB;
                        m = m - 1;
                        n = n - 1;
                    }
                    else if (n > 0 && scoringMatrix[m, n] == scoringMatrix[m, n - 1] - 2)
                    {
                        AlignmentA = refSeqArray[n - 1] + AlignmentA;
                        AlignmentB = "-" + AlignmentB;
                        n = n - 1;
                    }
                    else //if (m > 0 && scoringMatrix[m, n] == scoringMatrix[m - 1, n] + -2)
                    {
                        AlignmentA = "-" + AlignmentA;
                        AlignmentB = alineSeqArray[m - 1] + AlignmentB;
                        m = m - 1;
                    }
                }
            }
            refSeq = AlignmentA;
            alineSeq = AlignmentB;
        }

        public static float Score(string A, string B)
        {
            float max = A.Length;
            SequenceAlignment.Align(ref A, ref B);
            float match = 0.0f;
            for(int i = 0; i < max; i++)
            {
                if (A[i] == B[i]) match++;
            }

            return match/max;
        }
    }

}
