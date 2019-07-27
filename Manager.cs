using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore
{
    class Manager
    {
        //move all methods into here
        //validation
        

        //create book class
        //create author class
        //create distributer class

        //methods
        public bool CheckCustomerId(String[,] distributor, String currentDistributerID) {
            bool customerIDExist = false;
            for (int outer = 0; outer < distributor.GetUpperBound(0) + 1; outer++)
            {
                for (int inner = 0; inner < distributor.GetUpperBound(1) + 1; inner++)
                {
                    if (currentDistributerID == distributor[outer, inner])
                    {
                        customerIDExist = true;
                        break;
                    }
                }
            }
            return customerIDExist;
        }
        public string[] Sort(string[] outputSort, double[] tempTotalSort)
        {
            bool swaps = true;
            while (swaps)
            {
                int numSwaps = 0;
                for (int x = 0; x < tempTotalSort.Length - 1; x++)
                {
                    if (tempTotalSort[x + 1] > tempTotalSort[x])
                    {
                        double tempTS = tempTotalSort[x];
                        string tempOS = outputSort[x];

                        tempTotalSort[x] = tempTotalSort[x + 1];
                        tempTotalSort[x + 1] = tempTS;

                        outputSort[x] = outputSort[x + 1];
                        outputSort[x + 1] = tempOS;

                        numSwaps++;
                    }
                }
                if (numSwaps < 1)
                {
                    swaps = false;
                }

            }
            return outputSort;
        }
    }
}
