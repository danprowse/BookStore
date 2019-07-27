using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UofSarrePublishing
{
    public partial class EPOS : Form
    {
        //Declaring variables 
        string[,] books = new string[5, 4];
        string[,] distributor = new string[3, 2];

        double[,] dealerSales = new double[3, 5];
        int[,] dealerNoOfBooks = new int[3, 5];
        int[] currentStock = new int[5];
        string[,] authorSales = new string[3, 2];

        public EPOS()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //initialising values
            //0 = Book Name, 1 = Author, 2 = Price, 3 = Stock
            books[0, 0] = "1000 best jokes in Milling and Grinding";
            books[0, 1] = "S Presso";
            books[0, 2] = "6.47";
            books[0, 3] = "8";
            books[1, 0] = "10000 Worst Jokes in Milling and Grinding";
            books[1, 1] = "S Presso";
            books[1, 2] = "0.99";
            books[1, 3] = "3238";
            books[2, 0] = "Civet Cat Husbandry in the UK";
            books[2, 1] = "S Presso";
            books[2, 2] = "29.14";
            books[2, 3] = "3";
            books[3, 0] = "When Milling Goes Wrong: stories of death in Milling";
            books[3, 1] = "Win D Power";
            books[3, 2] = "10.50";
            books[3, 3] = "37";
            books[4, 0] = "Everything you ever wanted to know about Mill Construction... and then some: The world's biggest bumper book of mill construction";
            books[4, 1] = "Dr Footing";
            books[4, 2] = "319.56";
            books[4, 3] = "2";

            //0 = Shop Name, 1 = shop ID
            distributor[0, 0] = "Sarre Bookshop";
            distributor[0, 1] = "1";
            distributor[1, 0] = "Amaze-oon.co.uk";
            distributor[1, 1] = "2";
            distributor[2, 0] = "Earthmarble Bookshop";
            distributor[2, 1] = "3";

            
            // indexing is parallel with index of [3,5]
            // index 0 = sarre bookshop, 1 = amaze-oon.co.uk, 2 = earthmarble bookshop 
            // index 0 = 1000bestJokesStock, 1 = 10000worstJokesStock, 2 = civetCatStock, 3 = whenMillingGoesWrongStock, 4 = millConstructionStock

            currentStock = new int[5] { Convert.ToInt32(books[0, 3]), Convert.ToInt32(books[1, 3]), Convert.ToInt32(books[2, 3]), Convert.ToInt32(books[3, 3]), Convert.ToInt32(books[4, 3]) };
            dealerNoOfBooks = new int[3, 5] { { 0 , 0 , 0 , 0 , 0 }, { 0 , 0 , 0 , 0 , 0 }, { 0 , 0 , 0 , 0 , 0 } };
            dealerSales = new double[3, 5] { {0.00, 0.00, 0.00, 0.00, 0.00 }, {0.00, 0.00, 0.00, 0.00, 0.00 }, {0.00, 0.00, 0.00, 0.00, 0.00 } };
            authorSales = new string[3, 2]{ { "S Presso", "0.00" }, { "Win D Power", "0.00" }, { "Dr Footing", "0.00" } };

            //output Current stock values
            uiStock1000TextBox.Text = currentStock[0].ToString();
            uiStock10000TextBox.Text = currentStock[1].ToString();
            uiStockCatTextBox.Text = currentStock[2].ToString();
            uiStockWhenTextBox.Text = currentStock[3].ToString();
            uiStockEverythingTextBox.Text = currentStock[4].ToString();
        }

 

        private void uiSaleButton_Click(object sender, EventArgs e)
        {
            string[] bookCopies = new string[5] { uiBasket1000TextBox.Text, uiBasket10000TextBox.Text, uiBasketCatTextBox.Text, uiBasketWhenTextBox.Text, uiBasketEverythingTextBox.Text };
            string currentDistributerID = uiCustomerNumberTextBox.Text;
            bool customerIDExist = false;
            bool makeSale = false;
            bool instock;

            //check if customer ID exist
            for(int outer = 0; outer < distributor.GetUpperBound(0) + 1; outer++)
            {
                for(int inner = 0; inner < distributor.GetUpperBound(1) + 1; inner++)
                {
                    if(currentDistributerID == distributor[outer, inner])
                    {
                        customerIDExist = true;
                        break;
                    }
                }
            }
            
            for(int i = 0; i < bookCopies.Length; i++)
            {
                bool emptyString = true;

                //checking for unwanted or empty strings
                for(int x = 0; x < bookCopies.Length; x++)
                {
                    bool result = !int.TryParse(bookCopies[x], out int throwOut);

                    if (result)
                    {
                        emptyString = true;
                        break;
                    }
                    else
                    {
                        emptyString = false;
                    }
                }

                //validation testing
                if (emptyString)
                {
                    MessageBox.Show("Enter numbers only!");
                    break;
                }
                else
                {
                    instock = CheckStock(bookCopies);

                    if (customerIDExist == false)
                    {
                        MessageBox.Show("Customer does not Exist!");
                        break;
                    }
                    else if (Convert.ToInt32(bookCopies[i]) < 0)
                    {
                        MessageBox.Show("Quantity of Books must be above 0");
                        makeSale = false;
                        break;
                    }
                    else if (instock == false)
                    {
                        for (int x = 0; x < bookCopies.Length; x++)
                        {
                            if (currentStock[x] - Convert.ToInt32(bookCopies[x]) < 0)
                            {
                                MessageBox.Show("Not enough stock for your Request, we have " + currentStock[x].ToString() + ": " + books[x, 0]);
                            }
                        }
                        makeSale = false;
                        break;
                    }
                    else
                    {
                        makeSale = true;
                    }

                }


            }
                if(makeSale == true)
            {
                ConfirmSale(currentDistributerID, bookCopies);
            }
           
            

        }



        private void uiAuthorChartButton_Click(object sender, EventArgs e)
        {
            //working in parallel when sorted
            string[] tempOutput = new string[3];
            double[] tempAuthorSales = new double[3];

            uiAuthorChartTextBox.Clear();

            for(int outer = 0; outer < tempOutput.Length; outer++)
            {
                tempOutput[outer] = authorSales[outer, 0] + string.Format(": {0:C}", Convert.ToDouble(authorSales[outer, 1]));
                tempAuthorSales[outer] = Convert.ToDouble(authorSales[outer, 1]);  
            }

            //sorting
            tempOutput = Sort(tempOutput, tempAuthorSales);

            for (int i = 0; i <= authorSales.GetUpperBound(0); i++)
            {
                uiAuthorChartTextBox.Text += "\r\n" + tempOutput[i];  
            }
            
        }



        private void uiDealerChartButton_Click(object sender, EventArgs e)
        {
            uiDealerChartTextBox.Clear();

            // arrays to work in parallel when sorted
            const int posOfBookTitle = 0;
            double[] tempTotalSort = new double[3] { 0.00, 0.00, 0.00 };
            string[] outputSort = new string[3] { distributor[0,0], distributor[1,0] , distributor[2,0] };

            //inputting outputSort && tempTotalSort
            for(int x = 0; x < dealerNoOfBooks.GetLength(0); x++)
            {

                for (int y = 0; y < dealerNoOfBooks.GetLength(1); y++)
                {

                    tempTotalSort[x] += Convert.ToDouble(dealerSales[x, y]);
                    if(y != 4)
                    {
                        outputSort[x] += "\r\n" + dealerNoOfBooks[x, y] + "          " + books[y, posOfBookTitle];
                    } else
                    {
                        outputSort[x] += "\r\n" + dealerNoOfBooks[x, y] + "          " + books[y, posOfBookTitle] + "-- Total: " + tempTotalSort[x] + "\r\n";
                    }
                    
                }
                    
            }
            outputSort = Sort(outputSort, tempTotalSort);
                
            for (int inner = 0; inner < outputSort.Length; inner++)
            {
                uiDealerChartTextBox.Text += outputSort[inner]; 
            }
 
        }

        private void uiCustomerNumberTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        public bool CheckStock(string[] bookCopies)
        {
            bool inStock = true;
            int[] tempStockValues = new int[5];

            for (int i = 0; i < tempStockValues.Length; i++)
            {
                tempStockValues[i] = currentStock[i] - Convert.ToInt32(bookCopies[i]);
            }

            // checking to see if request is more than stock levels
            for(int i = 0; i <= tempStockValues.Length - 1; i++)
            {
                if(tempStockValues[i] < 0)
                {
                    inStock = false;
                    break;
                } else
                {
                    inStock = true;
                }
            }
            return inStock;
        }

        public void ConfirmSale(string currentDistributerID, string[] bookCopies)
        {
            double[] currentSale = new double[5];
            double totalSum = 0.00;
            string distributerId = currentDistributerID;

            for (int i = 0; i < currentSale.Length; i++)
            {
                currentSale[i] = Convert.ToDouble(bookCopies[i]) * Convert.ToDouble(books[i, 2]);//2 = Location of Book Price
            }
            totalSum = currentSale.Sum();

            //adjusting stock levels
            for (int i = 0; i < currentStock.Length; i++)
            {
                currentStock[i] =  (currentStock[i] - Convert.ToInt32(bookCopies[i]));
            }
            
            uiStock1000TextBox.Text = currentStock[0].ToString();        
            uiStock10000TextBox.Text = currentStock[1].ToString();
            uiStockCatTextBox.Text = currentStock[2].ToString();
            uiStockWhenTextBox.Text = currentStock[3].ToString();
            uiStockEverythingTextBox.Text = currentStock[4].ToString();

            //author sales
            for (int i = 0; i <= currentSale.Length - 1; i++)
            {
                if (currentSale[i] > 0.00)
                {
                    // 1 = poistionOfAuthor in books array
                    byte posOfAuthor = 1;
                    if (books[i, posOfAuthor] == "S Presso")
                    {
                        authorSales[0, 1] = (Math.Round(Convert.ToDouble(authorSales[0,1]) + currentSale[i], 2)).ToString();
                    }
                    else if (books[i, posOfAuthor] == "Win D Power")
                    {
                        authorSales[1, 1] = (Math.Round(Convert.ToDouble(authorSales[1, 1]) + currentSale[i], 2)).ToString();
                    }
                    else if (books[i, posOfAuthor] == "Dr Footing")
                    {
                        authorSales[2, 1] = (Math.Round(Convert.ToDouble(authorSales[2, 1]) + currentSale[i], 2)).ToString();
                    }
                }
                else
                {
                    continue;
                }
            }

            //distributer sales
            if (distributerId == "1") //index 0 in distributerSales
            {
                for (int i = 0; i <= currentSale.Length - 1; i++)
                {
                    dealerSales[0, i] += currentSale[i];
                    dealerNoOfBooks[0, i] += Convert.ToInt32(bookCopies[i]);
                }
            } else if (distributerId == "2") //index 1 in distributerSales
            {
                for(int i = 0; i <= currentSale.Length - 1; i++)
                {
                    dealerSales[1, i] += currentSale[i];
                    dealerNoOfBooks[1, i] += Convert.ToInt32(bookCopies[i]);
                }
            } else if (distributerId == "3") //index 2 in distributerSales
            {
                for (int i = 0; i <= currentSale.Length - 1; i++)
                {
                    dealerSales[2, i] += currentSale[i];
                    dealerNoOfBooks[2, i] += Convert.ToInt32(bookCopies[i]);
                }
            }
            
            //confirmed message 
            MessageBox.Show(string.Format("SALE CONFIRMED \n   Total: {0:C} ", totalSum));
            uiBasket1000TextBox.Text = "0";
            uiBasket10000TextBox.Text = "0";
            uiBasketCatTextBox.Text = "0";
            uiBasketWhenTextBox.Text = "0";
            uiBasketEverythingTextBox.Text = "0";
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
