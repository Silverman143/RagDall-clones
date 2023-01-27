using Mono.Data.Sqlite;
using Mono.Data;
using System.Data;
using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public enum DataType
{
    gems, coins, activeLevel, progressLevel, energy
}

public static class DataBaseHandler 
{
    private static string _dataBaseName = "DataBase";
    private static string _filePath;




    private static int GetIntData(string query)
    {
        int value = -1;

        _filePath = Application.persistentDataPath + "/" + _dataBaseName;

        string conn = "URI=file:" + _filePath + ".db";

        IDbConnection dbConn;
        IDbCommand dbCMD;
        IDataReader reader;

        dbConn = new SqliteConnection(conn);
        dbConn.Open();
        dbCMD = dbConn.CreateCommand();

        dbCMD.CommandText = query;

        reader = dbCMD.ExecuteReader();

        while (reader.Read())
        {
            value = reader.GetInt32(0);
        }

        reader.Close();
        reader = null;
        dbCMD.Dispose();
        dbCMD = null;
        dbConn.Close();
        dbConn = null;

        return value;
    }
    private static void UpdateData(string query)
    {
        _filePath = Application.persistentDataPath + "/" + _dataBaseName;

        string conn = "URI=file:" + _filePath + ".db";

        IDbConnection dbConn;
        IDbCommand dbCMD;
        IDataReader reader;

        dbConn = new SqliteConnection(conn);
        dbConn.Open();
        dbCMD = dbConn.CreateCommand();

        dbCMD.CommandText = query;

        reader = dbCMD.ExecuteReader();

        //while (reader.Read())
        //{

        //}

        reader.Close();
        reader = null;
        dbCMD.Dispose();
        dbCMD = null;
        dbConn.Close();
        dbConn = null;
    }

    public static List<InventoryItem> GetItems()
    {
        List<InventoryItem> items = new List<InventoryItem>();

        _filePath = Application.persistentDataPath + "/" + _dataBaseName;

        string query = $"Select * from Items;";


        string conn = "URI=file:" + _filePath + ".db";

        IDbConnection dbConn;
        IDbCommand dbCMD;
        IDataReader reader;

        dbConn = new SqliteConnection(conn);
        dbConn.Open();
        dbCMD = dbConn.CreateCommand();

        dbCMD.CommandText = query;

        reader = dbCMD.ExecuteReader();

        while (reader.Read())
        {
            InventoryItem item = ScriptableObject.CreateInstance<InventoryItem>();

            item.name = Enum.Parse<ItemName>( reader.GetString(6));
            item.id = reader.GetInt32(0);
            item.level = reader.GetInt32(1);
            item.strength = reader.GetInt32(2);
            item.inventoryId = reader.GetInt32(3);
            item.holderId = reader.GetInt32(4);
            item.amount = reader.GetInt32(5);

            items.Add(item);
        }

        reader.Close();
        reader = null;
        dbCMD.Dispose();
        dbCMD = null;
        dbConn.Close();
        dbConn = null;


        for (int i = 0; i < items.Count; i++)
        {
            Debug.Log($"item id = {items[i].id}, holder id = {items[i].holderId}");
        }

        return items;
    }

    public static void UploadItemData(InventoryItem item)
    {
        _filePath = Application.persistentDataPath + "/" + _dataBaseName;

        string conn = "URI=file:" + _filePath + ".db";

        string query = $"update Items set level = {item.level}, strength = {item.strength}, inventoryId = {item.inventoryId}, itemHolderId = {item.holderId}, amount = {item.amount} where id = {item.id}; ";

        IDbConnection dbConn;
        IDbCommand dbCMD;
        IDataReader reader;

        dbConn = new SqliteConnection(conn);
        dbConn.Open();
        dbCMD = dbConn.CreateCommand();

        dbCMD.CommandText = query;

        reader = dbCMD.ExecuteReader();

        reader.Close();
        reader = null;
        dbCMD.Dispose();
        dbCMD = null;
        dbConn.Close();
        dbConn = null;
    }
    public static void RemoveItem(InventoryItem item)
    {
        _filePath = Application.persistentDataPath + "/" + _dataBaseName;

        string conn = "URI=file:" + _filePath + ".db";

        string query = $"DELETE from Items where id = {item.id} ;";

        IDbConnection dbConn;
        IDbCommand dbCMD;
        IDataReader reader;

        dbConn = new SqliteConnection(conn);
        dbConn.Open();
        dbCMD = dbConn.CreateCommand();

        dbCMD.CommandText = query;

        reader = dbCMD.ExecuteReader();

        reader.Close();
        reader = null;
        dbCMD.Dispose();
        dbCMD = null;
        dbConn.Close();
        dbConn = null;
    }

    public static void ChangeItemHolderId(int itemId, int holderId)
    {
        _filePath = Application.persistentDataPath + "/" + _dataBaseName;

        string conn = "URI=file:" + _filePath + ".db";

        string query = $"update Items set itemHolderId = {holderId} where id = {itemId}; ";

        IDbConnection dbConn;
        IDbCommand dbCMD;
        IDataReader reader;

        dbConn = new SqliteConnection(conn);
        dbConn.Open();
        dbCMD = dbConn.CreateCommand();

        dbCMD.CommandText = query;

        reader = dbCMD.ExecuteReader();

        reader.Close();
        reader = null;
        dbCMD.Dispose();
        dbCMD = null;
        dbConn.Close();
        dbConn = null;
    }
}
