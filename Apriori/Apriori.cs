using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Apriori
{
    class Apriori
    {
        private StreamReader inStream;  //read the data from file
        private StreamWriter outStream; //write results to the file
        private int minSupport; //minimun support of a item (set)
        private string separator;   //separate items within a transaction in the input file

        private List<List<string>> listTransactions = new List<List<string>>(); //transactions list

        private List<ItemSet> itemSets = new List<ItemSet>();   //store composition of item sets and theire support
        //ItemSet is defined in ItemSet.cs

        public Apriori(string inFileName, string outFileName, string separator, int minSupport)
        {
            Console.WriteLine("initializing");
            inStream = new StreamReader(inFileName, System.Text.Encoding.Default);
            outStream = new StreamWriter(outFileName, false, System.Text.Encoding.Default);
            this.separator = separator;
            this.minSupport = minSupport;
            Console.WriteLine("init done");

            ReadFile();


            GenerateItemList();
            
            //number of terms within the item set
            int setDimension = 1;

            do
            {
                CountSupport(); //Count support of each item (set, after the first time) in the item (set) list
                GenerateItemSets(setDimension); //Generate next item set list
                setDimension++;
            } while (itemSets.Count > 0);



            outStream.Close();
            outStream.Dispose();

            Console.Read();
        }

        // Read data from the file, store it into listTransactions
        private void ReadFile()
        {
            Console.WriteLine("ReadFile() begins");
            try
            {
                string line;
                while (!inStream.EndOfStream)
                {
                    line = inStream.ReadLine();
                    List<string> transaction = new List<string>();
                    while (line.IndexOf(separator) > 0) //items within a transaction are divided by separator 
                    {
                        string item = line.Substring(0, line.IndexOf(separator));
                        transaction.Add(item);
                        line = line.Substring(line.IndexOf(separator) + separator.Length);  //last item of a transaction
                    }

                    transaction.Add(line);

                    listTransactions.Add(transaction);

                }
                inStream.Close();
                inStream.Dispose();
                Console.WriteLine("ReadFile() ends");
            }
            catch (Exception e)
            {
                Console.Write(e.Message.ToString());
                inStream.Close();
                inStream.Dispose();
            }
        }

        private void GenerateItemList() //generate a list that includes all the items
        {
            Console.WriteLine("GenerateItemList() begins");
            for (int i = 0; i < listTransactions.Count; i++)
            {
                for (int j = 0; j < listTransactions[i].Count; j++)
                {
                    bool exist = false; //whether the item already exist in the item list

                    for (int k = 0; k < itemSets.Count; k++)
                    {
                        if (Equals(itemSets[k].names[0], listTransactions[i][j]))
                        {
                            exist = true;
                            break;
                        }
                    }
                    if (exist == false)
                    {
                        ItemSet currentItem = new ItemSet();
                        currentItem.names.Add(listTransactions[i][j]);
                        currentItem.support = 0;
                        itemSets.Add(currentItem);
                    }
                }
            }
            Console.WriteLine("GenerateItemList() ends");
        }

        private void CountSupport() //Count support of each item set in the item set list 
        {
            Console.WriteLine("CountSupport() begins");
            for (int i = 0; i < itemSets.Count; i++)
            {
                for (int j = 0; j < listTransactions.Count; j++)
                {
                    if (itemSets[i].names.Except(listTransactions[j]).ToList().Count == 0)  //if we find the item set in a transaction
                    {
                        itemSets[i].support++;  //support of the item set + 1
                    }
                }
            }

            int count = itemSets.Count;
            for (int i = 0; i < count; i++)
            {
                if (itemSets[i].support < minSupport)
                {
                    itemSets.Remove(itemSets[i]);   //remove the item with support lower than min support
                    i--;
                    count--;
                }
            }

            for (int i = 0; i < itemSets.Count; i++)    //write the result into the file
            {
                outStream.Write("{");
                foreach (string str in itemSets[i].names)
                {
                    outStream.Write(str + " ");
                }
                outStream.WriteLine("} : " + itemSets[i].support);
            }
            outStream.WriteLine();


            Console.WriteLine("CountSupport() ends");
        }

        private bool equals(List<string> list1, List<string> list2) //Compare two string lists to determine whether their elements are the same
        {
            if(list1.Except(list2).ToList().Count == 0
                && list2.Except(list1).ToList().Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GenerateItemSets(int setDimension) //generate a item set list, each item set has (setDimension+1) items
        {
            Console.WriteLine("GenerateItemSets(" + setDimension + ") begins");
            List<ItemSet> itemSetsTemp = new List<ItemSet>(); // a temporary item set list

            for (int i = 0; i < itemSets.Count - 1; i++)
            {
                for (int j = i + 1; j < itemSets.Count; j++)
                {
                    List<string> namesTemp = new List<string>();
                    namesTemp = itemSets[i].names.Union(itemSets[j].names).ToList(); //generate item sets from tow existing ones

                    if (namesTemp.Count == setDimension + 1)
                    {
                        bool exist = false; //whether the item set already exist in the item set list
                        for (int k = 0; k < itemSetsTemp.Count; k++)
                        {
                            if (equals(itemSetsTemp[k].names, namesTemp))
                            {
                                exist = true;
                                break;
                            }
                        }
                        if (exist == false) //if it doesn't exist, add it to the temporary item set list
                        {
                            ItemSet itemSetTemp = new ItemSet();
                            itemSetTemp.names = namesTemp;
                            itemSetTemp.support = 0;
                            itemSetsTemp.Add(itemSetTemp);
                        }
                    }
                }
            }
            itemSets.Clear();
            itemSets = itemSetsTemp;    //replace the existing item set list by the temporary one

            Console.WriteLine("GenerateItemSets(" + setDimension + ") ends");
        }



    }
}
