using System;
using System.Collections.Generic;
using ImportTrackSimulatorDataUtil;

namespace Train_System_Simulator
{
    class Program
    {

        static void Main(string[] args)
        {
            #region Unit Test 1
            //Succeeded
            //Dictionary<string, string>[] data = ProjectCSV.Core.ProjectCSV.ParseCommaSeparateValueFile("G:\\Users\\Ahmed\\Documents\\My Documents\\unitTest1.csv", 3);

            //Console.WriteLine("Unit Test 1");

            //foreach (Dictionary<string, string> line in data)
            //{
            //    foreach (KeyValuePair<string, string> keyValue in line)
            //    {
            //        Console.Write("Column : {0} Data: {1},", keyValue.Key, keyValue.Value);
            //    }

            //    Console.WriteLine("\n-------------------------");
            //}

            //Console.WriteLine("\n============================");

            #endregion 

            #region Unit Test 2
            //Succeeded

            //Dictionary<string, string>[] data2 = ProjectCSV.Core.ProjectCSV.ParseCommaSeparateValueFile("G:\\Users\\Ahmed\\Documents\\My Documents\\unitTest2.csv", 3);

            //Console.WriteLine("Unit Test 2");

            //foreach (Dictionary<string, string> line in data2)
            //{
            //    foreach (KeyValuePair<string, string> keyValue in line)
            //    {
            //        Console.Write("Column : {0} Data: {1},", keyValue.Key, keyValue.Value);
            //    }

            //    Console.WriteLine("\n-------------------------");
            //}

            //Console.WriteLine("\n============================");

            #endregion

            #region Unit Test 3
            //Succeeded

            //Dictionary<string, string>[] data3 = ProjectCSV.Core.ProjectCSV.ParseCommaSeparateValueFile("G:\\Users\\Ahmed\\Documents\\My Documents\\unitTest3.csv", 29);

            //Console.WriteLine("Unit Test 3");

            //foreach (Dictionary<string, string> line in data3)
            //{
            //    foreach (KeyValuePair<string, string> keyValue in line)
            //    {
            //        Console.Write("Column : {0} Data: {1},", keyValue.Key, keyValue.Value);
            //    }

            //    Console.WriteLine("\n-------------------------");
            //}

            //Console.WriteLine("\n============================");

            #endregion

            #region Unit Test 4
            //Failed

            //Dictionary<string, string>[] data4 = ProjectCSV.Core.ProjectCSV.ParseCommaSeparateValueFile("G:\\Users\\Ahmed\\Documents\\My Documents\\unitTest4.csv", 4);

            //Console.WriteLine("Unit Test 4");

            //if (data4 != null)
            //{

            //    foreach (Dictionary<string, string> line in data4)
            //    {
            //        foreach (KeyValuePair<string, string> keyValue in line)
            //        {
            //            Console.Write("Column : {0} Data: {1},", keyValue.Key, keyValue.Value);
            //        }

            //        Console.WriteLine("\n-------------------------");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Failed");
            //}

            //Console.WriteLine("\n============================");

            #endregion

            #region Unit Test 5
            //Failed

            //Dictionary<string, string>[] data5 = ProjectCSV.Core.ProjectCSV.ParseCommaSeparateValueFile("G:\\Users\\Ahmed\\Documents\\My Documents\\unitTest5.csv", 5);

            //Console.WriteLine("Unit Test 5");














































            //if (data5 != null)
            //{

            //    foreach (Dictionary<string, string> line in data5)
            //    {
            //        foreach (KeyValuePair<string, string> keyValue in line)
            //        {
            //            Console.Write("Column : {0} Data: {1},", keyValue.Key, keyValue.Value);
            //        }

            //        Console.WriteLine("\n-------------------------");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Failed");
            //}

            //Console.WriteLine("\n============================");

            #endregion

            #region Unit Test 6
            //Failed

            //Dictionary<string, string>[] data6 = ProjectCSV.Core.ProjectCSV.ParseCommaSeparateValueFile("G:\\Users\\Ahmed\\Documents\\My Documents\\unitTest6.csv", 4);

            //Console.WriteLine("Unit Test 6");

            //if (data6 != null)
            //{

            //    foreach (Dictionary<string, string> line in data6)
            //    {
            //        foreach (KeyValuePair<string, string> keyValue in line)
            //        {
            //            Console.Write("Column : {0} Data: {1},", keyValue.Key, keyValue.Value);
            //        }

            //        Console.WriteLine("\n-------------------------");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("Failed");
            //}

            //Console.WriteLine("\n============================");

            #endregion

            //RailVehicle DX34 = new RailVehicle("DX34");
            //RailVehicle TS10 = new RailVehicle("TS10");
            //RailVehicle SQ11 = new RailVehicle("SQ11");

            //Line BankstownLine = new Line(new List<RailVehicle>() { DX34, TS10, SQ11 });

            //Console.Write("Train: {0} Position: {1}", DX34.Name, 1);

            new ImportTrackSimulatorDataUtil.ImportTrackSimulatorData().TrackPointsWithStationImport();

            Console.ReadLine();
        }
    }
}
