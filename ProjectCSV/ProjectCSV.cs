using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProjectCSV.Core
{
    public static class ProjectCSV
    {
        // (1 = A, 2 = B...27 = AA...703 = AAA...)
        private static string GetColNameFromIndex(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        // (A = 1, B = 2...AA = 27...AAA = 703...)
        private static int GetColNumberFromName(string columnName)
        {
            char[] characters = columnName.ToUpperInvariant().ToCharArray();
            int sum = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                sum *= 26;
                sum += (characters[i] - 'A' + 1);
            }
            return sum;
        }

        public static Dictionary<String, String>[] ParseCommaSeparateValueFile(String FullFilePath, int ColumnsNeeded, bool containsColumnHeaders) {

            List<Dictionary<String, String>> data = new List<Dictionary<string, string>>();

            try
            {
                FileStream csvStream = File.OpenRead(FullFilePath);
                using (StreamReader csvStreamReader = new StreamReader(csvStream))
                {
                    int rowIter = 1;
                    List<string> columnHeaders = new List<string>();

                    while (!csvStreamReader.EndOfStream)
                    {
                        Dictionary<String, String> keyValueSplits = new Dictionary<string, string>();
                        
                        string line = csvStreamReader.ReadLine();
                        if (line.Contains("\""))
                        {
                            bool quotedTextExists = true;

                            string[] splitsTemp = line.Split(',');
                            if (splitsTemp.Length < ColumnsNeeded)
                            {
                                throw new System.IO.FileFormatException("CSV is in invalid format");
                            }
                            else if (splitsTemp.Length == ColumnsNeeded)
                            {
                                
                            }
                            else
                            {
                                //look for commas
                                List<string> quotedSplits = new List<string>();
                                int index = 0;
                                int columnCount = 0;
                                while (quotedTextExists)
                                {
                                    string columnData = "";
                                    bool endOfColumn = false;

                                    int commaCharIndex = line.IndexOf(",", index);

                                    string textBeforeComma = "";

                                    if (commaCharIndex == -1) // we no longer can find commas in our iteration
                                    {
                                        if (!((quotedSplits.Count + 1) == ColumnsNeeded)) // we must therefore be at the final column
                                        {
                                            throw new System.IO.FileFormatException("CSV is in invalid format"); //if not, error out
                                        }
                                        endOfColumn = true;
                                        textBeforeComma = line.Substring(index);
                                    }
                                    else
                                    {
                                        if (commaCharIndex == index) // we don't allow for empty columns
                                        {
                                            throw new System.IO.FileFormatException("CSV is in invalid format");
                                        }

                                        textBeforeComma = line.Substring(index, commaCharIndex - index);
                                    }

                                    if (!textBeforeComma.Contains("\"")) // collect the data as this is a traditional csv column
                                    {
                                        columnData = textBeforeComma;
                                        quotedSplits.Add(columnData);
                                        ++columnCount;
                                        if (!endOfColumn) //we need to consider if we are at the last column now
                                        {
                                            index = commaCharIndex + 1;
                                        }
                                        else
                                        {
                                            index = index + textBeforeComma.Length;
                                        }
                                    }
                                    else
                                    {
                                        if (!(textBeforeComma.StartsWith("\""))) // we know there is a comma, it must be preceded by a quote character
                                        {
                                            throw new System.IO.FileFormatException("CSV is in invalid format"); //if not, error out
                                        }

                                        string[] quoteCharsInQuotedTextSplitBeforeComma = textBeforeComma.Substring(1).Split(new string[] { @"""" }, StringSplitOptions.None);

                                        foreach (string charsWithQuotedText in quoteCharsInQuotedTextSplitBeforeComma)
                                        {
                                            if (charsWithQuotedText.Contains("\"")) // if we have a odd number of quoted characters
                                            {
                                                throw new System.IO.FileFormatException("CSV is in invalid format"); //error out
                                            }
                                        }

                                        int quotedCharOccurance_InTextBeforeComma = textBeforeComma.Length - textBeforeComma.Replace("\"", "").Length;

                                        //look for terminating quote character
                                        string textAfterComma = line.Substring(commaCharIndex + 1);

                                        int quotedCharOccurance_InTextAfterComma = textAfterComma.Length - textAfterComma.Replace("\"", "").Length;

                                        if ((quotedCharOccurance_InTextAfterComma == 0) || (quotedCharOccurance_InTextAfterComma % 2) == 0) // if we have do not have an odd number of quoted characters
                                        {
                                            throw new System.IO.FileFormatException("CSV is in invalid format"); //error out
                                        }

                                        string[] commaSplitsAfterComma = textAfterComma.Split(',');
                                        int newIndex = index + textBeforeComma.Length;
                                        int commaSplitIter = 0;
                                        int tempNewIndex = 0;
                                        for (; commaSplitIter < commaSplitsAfterComma.Length; ++commaSplitIter)
                                        {
                                            string commaSplit = commaSplitsAfterComma[commaSplitIter];
                                            tempNewIndex = 
                                                newIndex 
                                                + (String.Join(",", commaSplitsAfterComma.Take(commaSplitIter).ToArray()) + ",").Length 
                                                + commaSplit.Length 
                                                + (((commaSplitIter + 1) == commaSplitsAfterComma.Length) ? 0 : 1);
                                            //addition above is designed to get the exact index of the end of the column

                                            if (commaSplit.Length > 0)
                                            {
                                                string[] quotedCharSplitsInbetweenCommas = commaSplit.Split(new string[] { "\"\"" }, StringSplitOptions.None);

                                                bool foundTerminatingQuoteChar = false;
                                                
                                                foreach (string quotedCharSplit in quotedCharSplitsInbetweenCommas)
                                                {
                                                    int typeOfColmnData = 0;

                                                    if ((quotedCharSplit.Replace("\"", "").Length == 0) && quotedCharSplit.EndsWith("\""))
                                                    {
                                                        typeOfColmnData = 2;
                                                    }

                                                    if ((typeOfColmnData != 2) &&(quotedCharSplit.Length - quotedCharSplit.Replace("\"", "").Length) == 1)
                                                    {
                                                        typeOfColmnData = 1;
                                                    }

                                                    if (typeOfColmnData > 0)
                                                    {
                                                        if (quotedCharSplit.EndsWith("\""))
                                                        {
                                                            columnData += quotedCharSplit.Replace("\"", "");
                                                            foundTerminatingQuoteChar = true;
                                                            break; //if we find the single 
                                                        }
                                                        else
                                                        {
                                                            throw new System.IO.FileFormatException("CSV is in invalid format"); //error out
                                                        }
                                                    }
                                                    else
                                                    {
                                                        columnData += quotedCharSplit + "\"\"";   //re-add in splitted out douyble quotes
                                                    }
                                                }

                                                if (!foundTerminatingQuoteChar)
                                                {
                                                    throw new System.IO.FileFormatException("CSV is in invalid format"); //error out
                                                }
                                                else
                                                {
                                                    
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                columnData += ","; //re-add in splitted out comma
                                            }
                                        }

                                        index = tempNewIndex;

                                        string actualStringData = textBeforeComma.Substring(1); // we need to chop off the starting quote character. It is not apart of the data

                                        columnData = actualStringData + "," + columnData; // we need to concatenate the text before and after the comma together 
                                        quotedSplits.Add(columnData);
                                        ++columnCount;
                                    }

                                    if (index == line.Length)
                                    {
                                        quotedTextExists = false;
                                    }
                                    else if (index > line.Length)
                                    {
                                        throw new System.IO.FileFormatException("CSV is in invalid format");
                                    }
                                }

                                if (ColumnsNeeded != quotedSplits.Count)
                                {
                                    throw new System.IO.FileFormatException("CSV is in invalid format");
                                }
                                
                                int columnIter = 1;

                                
                                foreach (string quotedSplit in quotedSplits)
                                {
                                    if (rowIter == 1 && containsColumnHeaders)
                                    {
                                        columnHeaders.Add(quotedSplit);
                                    }
                                    else
                                    {
                                        string key = String.Concat(rowIter, GetColNameFromIndex(columnIter));
                                        if (containsColumnHeaders)
                                        {
                                            key = columnHeaders[columnIter - 1];
                                        }

                                        keyValueSplits.Add(key, quotedSplit);
                                    }
                                    columnIter++;
                                }
                            }
                        }
                        else
                        {
                            string[] splitsTemp = line.Split(',');
                            if (ColumnsNeeded != splitsTemp.Length)
                            {
                                throw new System.IO.FileFormatException("CSV is in invalid format");
                            }

                            int columnIter = 1;
                            foreach (string split in splitsTemp)
                            {
                                if (rowIter == 1 && containsColumnHeaders)
                                {
                                    columnHeaders.Add(split);
                                }
                                else
                                {
                                    string key = String.Concat(rowIter, GetColNameFromIndex(columnIter));
                                    if (containsColumnHeaders) {
                                        key = columnHeaders[columnIter - 1];
                                    }

                                    keyValueSplits.Add(key, split);
                                }
                                columnIter++;
                            }
                            
                        }
                        if (!(rowIter == 1 && containsColumnHeaders))
                        {
                            data.Add(keyValueSplits);
                        }
                        rowIter++;
                    }
                    
                }
            }
            catch (FileFormatException ffe)
            {
                data = null;
            }

            if (data != null)
            {
                return data.ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}
