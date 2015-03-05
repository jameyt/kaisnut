using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;

namespace utilities.data
{

    /// <summary>
    /// Generic data access layer.
    /// Planned for generic reusability.
    /// </summary>
    public class DataInteraction
    {
        public static IDbConnection CreateLocalSqlConnection(string databaseLocation)
        {
            var cs = string.Format(
                        @"Data Source=(LocalDB)\v11.0;
                          AttachDbFilename={0};
                          Integrated Security=True;
                          Connect Timeout=30;"
                        , databaseLocation);
            var cn = new SqlConnection(cs);
            cn.Open();
            return cn;
        }

        //public enum ConnectionType
        //{
        //    OleDb,
        //    SqlClient
        //}

        ////public static IDbConnection CreateConnection(string databasePath, string user, string password) //ConnectionType ct
        ////{
        ////    IDbConnection cn = null;
        ////    ConnectionType ct = ConnectionType.OleDb;

        ////    if (ct == ConnectionType.OleDb)
        ////    {
        ////        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databasePath +
        ////            ";User Id=" + user + ";Password=" + password + ";";
        ////        cn = new System.Data.OleDb.OleDbConnection();
        ////        cn.ConnectionString = connectionString;
        ////    }

        ////    return cn;
        ////}

        //public static IDbConnection CreateAccessConnection(string databasePath, string workgroupPath, string user, string password) //ConnectionType ct
        //{
        //    IDbConnection cn = null;
        //    ConnectionType ct = ConnectionType.OleDb;

        //    if (ct == ConnectionType.OleDb)
        //    {
        //        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databasePath +
        //            ";Jet OLEDB:System database=" + workgroupPath + ";User Id=" + user + ";Password=" + password + ";";
        //        cn = new OleDbConnection();
        //        cn.ConnectionString = connectionString;
        //    }

        //    return cn;
        //}

        //public static string CreateTulSqlConnectionString()
        //{
        //    var tulSqlConnectionString = "Data Source=TULSQL;Initial Catalog=ECAT;Persist Security Info=True;User ID=ecatadmin;Password=goldeagle1010";
        //    return tulSqlConnectionString;
        //}


        //public static IDbConnection CreateTulSqlConnection()
        //{
        //    var tulSqlConnectionString = CreateTulSqlConnectionString();
        //    var sqlConnection = new SqlConnection(tulSqlConnectionString);
        //    sqlConnection.Open();
        //    return sqlConnection;
        //}

        //public static IDbConnection CreateSqlConnection(string serverAddress, string database, string user,
        //    string password) //ConnectionType ct
        //{
        //    IDbConnection cn = null;

        //    string connectionString = "Server=" + serverAddress +
        //                              ";Database=" + database + ";User Id=" + user + ";Password=" + password + ";";
        //    cn = new SqlConnection();
        //    cn.ConnectionString = connectionString;

        //    return cn;
        //}

        //public static IDbCommand CreateCommand(string cmt, IDbConnection cn)
        //{
        //    IDbCommand cm = cn.CreateCommand();
        //    cm.CommandText = cmt;
        //    cm.Connection = cn;
        //    return cm;
        //}

        //public static IDbCommand CreateCommand(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        //{
        //    IDbCommand cm = cn.CreateCommand();

        //    foreach (OleDbParameter p in pc)
        //    {
        //        if (cn is SqlConnection)
        //        {
        //            cm.Parameters.Add(new SqlParameter(p.ParameterName, p.Value));
        //        }
        //        else
        //        {
        //            cm.Parameters.Add(p);
        //        }
        //    }

        //    cm.CommandText = cmt;
        //    cm.Connection = cn;
        //    return cm;
        //}

        //public static IDataReader CreateReader(string cmt, IDbConnection cn)
        //{
        //    IDataReader r = null;

        //    using (IDbCommand cm = CreateCommand(cmt, cn))
        //    {
        //        r = cm.ExecuteReader();
        //    }

        //    return r;
        //}

        //public static IDataReader CreateReader(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        //{
        //    IDataReader r = null;

        //    using (IDbCommand cm = CreateCommand(cmt, pc, cn))
        //    {
        //        r = cm.ExecuteReader();
        //    }

        //    return r;
        //}

        //public static int ExecuteNonQuery(string cmt, IDbConnection cn)
        //{
        //    int er = -1;

        //    using (IDbCommand cm = CreateCommand(cmt, cn))
        //    {
        //        er = cm.ExecuteNonQuery();
        //    }

        //    return er;
        //}

        //public static object ExecuteScalar(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        //{
        //    object o;

        //    using (IDbCommand cm = CreateCommand(cmt, pc, cn))
        //    {
        //        o = cm.ExecuteScalar();
        //    }

        //    return o;
        //}

        //public static object ExecuteScalar(string cmt, IDbConnection cn)
        //{
        //    object o;

        //    using (IDbCommand cm = CreateCommand(cmt, cn))
        //    {
        //        o = cm.ExecuteScalar();
        //    }

        //    return o;
        //}

        //public static int ExecuteNonQuery(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        //{
        //    int er = -1;

        //    //StringBuilder sb1 = new StringBuilder();
        //    //StringBuilder sb2 = new StringBuilder();
        //    //StringBuilder sb3 = new StringBuilder();


        //    //foreach (OleDbParameter p in pc)
        //    //{
        //    //    sb1.Append(p.ParameterName + ", ");
        //    //    sb2.Append(p.OleDbType.ToString() + ", ");
        //    //    sb3.Append(p.Value + ", ");
        //    //}

        //    //Console.WriteLine(sb1.ToString());
        //    //Console.WriteLine(sb2.ToString()); 
        //    //Console.WriteLine(sb3.ToString());

        //    using (IDbCommand cm = CreateCommand(cmt, pc, cn))
        //    {
        //        er = cm.ExecuteNonQuery();
        //    }

        //    return er;
        //}

        //public static DataTable CreateDataTable(string cmt, IDbConnection cn)
        //{
        //    DataTable dt = new DataTable();

        //    using (IDataReader r = CreateReader(cmt, cn))
        //    {
        //        dt.Load(r);
        //    }

        //    return dt;
        //}

        //public static DataTable CreateDataTable(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        //{
        //    DataTable dt = new DataTable();

        //    using (IDataReader r = CreateReader(cmt, pc, cn))
        //    {
        //        dt.Load(r);
        //    }

        //    return dt;
        //}

        //public static DataRow CreateDataRow(string cmt, IDbConnection cn)
        //{
        //    DataRow dr = null;

        //    using (DataTable dt = CreateDataTable(cmt, cn))
        //    {
        //        dr = dt.Rows[0];
        //    }

        //    return dr;
        //}

        //public static DataRow CreateDataRow(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        //{
        //    DataRow dr = null;

        //    using (DataTable dt = CreateDataTable(cmt, pc, cn))
        //    {
        //        dr = dt.Rows[0];
        //    }

        //    return dr;
        //}

        //public static OleDbParameter CreateOleDbParameter(string name, object value, OleDbType type)
        //{
        //    if (value == null || value.Equals(string.Empty))
        //    {
        //        value = System.DBNull.Value;
        //    }
        //    OleDbParameter p = new OleDbParameter();
        //    p.ParameterName = name;
        //    p.Value = value;
        //    p.OleDbType = type;
        //    return p;
        //}

        ////public static OleDbParameter CreateOleDbParameter(string name, object value, OleDbType type)
        ////{
        ////    OleDbParameter p = new OleDbParameter();
        ////    p.ParameterName = name;
        ////    p.Value = value;
        ////    p.OleDbType = type;
        ////    return p;
        ////}

        //public static OleDbParameter CreateOleDbParameter(string name, object value)
        //{
        //    OleDbParameter p = new OleDbParameter();
        //    p.ParameterName = name;
        //    p.Value = value;

        //    return p;
        //}

        ///// <summary>
        ///// Creates a DataSet of all of the data in all of the tables in the database.
        ///// </summary>
        ///// <param name="cn">Connection to the database</param>
        ///// <returns>Returns a DataSet of all of the data in all of the tables in the database.</returns>
        //public static DataSet CreateDataSet(OleDbConnection cn)
        //{
        //    var dataSet = new DataSet();
        //    var tableNames = ListTableNames(cn);

        //    foreach (var tableName in tableNames)
        //    {
        //        var cmt = "SELECT * FROM " + tableName;

        //        using (var dt = CreateDataTable(cmt, cn))
        //        {
        //            if (dt != null && dt.Rows.Count > 0)
        //            {
        //                dt.TableName = tableName;
        //                dataSet.Tables.Add(dt);
        //            }
        //        }
        //    }

        //    return dataSet;
        //}

        //public static IList<string> ListTableNames(OleDbConnection cn)
        //{
        //    //var tables = new List<string>();
        //    //DataTable dt = cn.GetSchema("Tables");
        //    //foreach (DataRow row in dt.Rows)
        //    //{
        //    //    var tablename = (string)row[2];
        //    //    tables.Add(tablename);
        //    //}

        //    System.Collections.Generic.List<string> tables = GetTables(cn);
        //    return tables;
        //}


        ////DatabaseInfoCollector

        //public static System.Collections.Generic.List<string> GetTables(OleDbConnection cn)
        //{
        //    System.Data.DataTable tables;

        //    tables = cn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

        //    System.Collections.Generic.List<string> Tables = new System.Collections.Generic.List<string>();
        //    for (int i = 0; i < tables.Rows.Count; i++)
        //    {
        //        Tables.Add(tables.Rows[i][2].ToString());
        //    }
        //    return Tables;
        //}

        //public static System.Collections.Generic.List<string> GetColumnNames(OleDbConnection cn, string table)
        //{
        //    System.Data.DataTable dataSet = new System.Data.DataTable();

        //    System.Data.OleDb.OleDbCommand c = new System.Data.OleDb.OleDbCommand("SELECT * FROM " + table, cn);
        //    using (System.Data.OleDb.OleDbDataAdapter dataAdapter = new System.Data.OleDb.OleDbDataAdapter(c))
        //    {
        //        dataAdapter.Fill(dataSet);
        //    }

        //    System.Collections.Generic.List<string> columns = new System.Collections.Generic.List<string>();
        //    for (int i = 0; i < dataSet.Columns.Count; i++)
        //    {
        //        columns.Add(dataSet.Columns[i].ColumnName);
        //    }
        //    return columns;
        //}

        //public static bool CreateNewAccessDatabase(string fileName, int x)
        //{
        //    bool result = false;

        //    ADOX.Catalog cat = new ADOX.Catalog();
        //    ADOX.Table table = new ADOX.Table();

        //    //Create the table and it's fields. 
        //    table.Name = "Table1";

        //    table.Columns.Append("Field1");
        //    table.Columns.Append("Field2");

        //    try
        //    {
        //        cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + "; Jet OLEDB:Engine Type=5");
        //        cat.Tables.Append(table);

        //        //Now Close the database
        //        //ADODB.Connection x = cat.ActiveConnection;
        //        //  ADODB.Connection con = cat.ActiveConnection as ADODB.Connection;
        //        // if (con != null)
        //        // con.Close();

        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //    }
        //    cat = null;
        //    return result;
        //}

        //public static bool CreateNewAccessDatabase(string connectionString, DataSet dataSet)
        //{
        //    bool result = false;
        //    ADOX.Catalog cat = new ADOX.Catalog();
        //    DataColumn dc;

        //    try
        //    {

        //        cat.Create(connectionString);

        //        foreach (DataTable dataTable in dataSet.Tables)
        //        {
        //            ADOX.Table table = new ADOX.Table();
        //            //Create the table and it's fields. 
        //            table.Name = dataTable.TableName;

        //            foreach (DataColumn column in dataTable.Columns)
        //            {
        //                //table.Columns.Append(column.ColumnName);
        //                ADOX.Column accessColumn = new ADOX.Column();
        //                accessColumn.Name = column.ColumnName;
        //                accessColumn.ParentCatalog = cat;
        //                accessColumn.Type = GetAdoxDataType(column);

        //                if (accessColumn.Name.Equals("SpecialInfo")
        //                    || accessColumn.Name.Equals("ExhByCFM")
        //                    || accessColumn.Name.Equals("HeatExhByCFM")
        //                    || accessColumn.Name.Equals("RFCurbSpecial1")
        //                    || accessColumn.Name.Equals("RFCurbSpecial2")
        //                    || accessColumn.Name.Equals("CoolLWT")
        //                    || accessColumn.Name.Equals("Split1Length")
        //                    || accessColumn.Name.Equals("Split2Length")
        //                    || accessColumn.Name.Equals("PartNo")
        //                    || accessColumn.Name.Equals("CoolGPM2")
        //                    || accessColumn.Name.Equals("BuildPD")
        //                    || accessColumn.Name.Equals("InitTemp")
        //                    || accessColumn.Name.Equals("FinalTemp")
        //                    || accessColumn.Name.Equals("ExternalVol")
        //                    || accessColumn.Name.Equals("PipingOpWeight")
        //                    || accessColumn.Name.Equals("PipingShipWeight")
        //                    || accessColumn.Name.Equals("PipingPrice")
        //                    || accessColumn.Name.Equals("Unit_ShipDate")
        //                    || accessColumn.Name.Equals("Unit_Type")
        //                    || accessColumn.Name.Equals("Unit_Size")
        //                    || accessColumn.Name.Equals("AirFlow")
        //                    || accessColumn.Name.Equals("Voltage")
        //                    || accessColumn.Name.Equals("Assembly")
        //                    || accessColumn.Name.Equals("Wiring")
        //                    || accessColumn.Name.Equals("Painting")
        //                    || accessColumn.Name.Equals("Unit_Length")
        //                    || accessColumn.Name.Equals("Unit_Height")
        //                    || accessColumn.Name.Equals("Unit_Width")
        //                    || accessColumn.Name.Equals("Rail")
        //                    || accessColumn.Name.Equals("UnitSPA")
        //                    || accessColumn.Name.Equals("Wheel_Model")
        //                    || accessColumn.Name.Equals("TotalModule")
        //                    || accessColumn.Name.Equals("ListpriceM1")
        //                    || accessColumn.Name.Equals("ListpriceM2")
        //                    || accessColumn.Name.Equals("ListpriceM3")
        //                    || accessColumn.Name.Equals("ListpriceM4")
        //                    || accessColumn.Name.Equals("ListpriceM5")
        //                    || accessColumn.Name.Equals("ListpriceM6")
        //                    || accessColumn.Name.Equals("ListpriceM7")
        //                    || accessColumn.Name.Equals("ListpriceM8")
        //                    || accessColumn.Name.Equals("ListpriceM9")
        //                    || accessColumn.Name.Equals("ListTotal")
        //                    || accessColumn.Name.Equals("TDirtDP")
        //                    || accessColumn.Name.Equals("TFilterDp")
        //                    || accessColumn.Name.Equals("TCoolDp")
        //                    || accessColumn.Name.Equals("THeatDp")
        //                    || accessColumn.Name.Equals("TReheatDp")
        //                    || accessColumn.Name.Equals("TWheelDp")
        //                    || accessColumn.Name.Equals("TotalDp1")
        //                    || accessColumn.Name.Equals("TotalDp2")
        //                    || accessColumn.Name.Equals("OADB0")
        //                    || accessColumn.Name.Equals("OAWB0")
        //                    || accessColumn.Name.Equals("OADB1")
        //                    || accessColumn.Name.Equals("OAWB1")
        //                    || accessColumn.Name.Equals("RADB0")
        //                    || accessColumn.Name.Equals("RAWB0")
        //                    || accessColumn.Name.Equals("RADB1")
        //                    || accessColumn.Name.Equals("RAWB1")
        //                    || accessColumn.Name.Equals("SACFM")
        //                    || accessColumn.Name.Equals("RABPCFM")
        //                    || accessColumn.Name.Equals("MXBPCFM")
        //                    || accessColumn.Name.Equals("DesireDB0")
        //                    || accessColumn.Name.Equals("RelHM")
        //                    || accessColumn.Name.Equals("SAESP")
        //                    || accessColumn.Name.Equals("EAESP")
        //                    || accessColumn.Name.Equals("Altitude")
        //                    || accessColumn.Name.Equals("ReturnFanData_2")
        //                    || accessColumn.Name.Equals("PR2_Code")
        //                    || accessColumn.Name.Equals("HeatGPM2")
        //                    || accessColumn.Name.Equals("HeatBuildPD")
        //                    || accessColumn.Name.Equals("HeatInitTemp")
        //                    || accessColumn.Name.Equals("HeatFinalTemp")
        //                    || accessColumn.Name.Equals("HeatExternalVol")
        //                    || accessColumn.Name.Equals("HeatPipingOpWeight")
        //                    || accessColumn.Name.Equals("HeatPipingShipWeight")
        //                    || accessColumn.Name.Equals("HeatPipingPrice")
        //                    || accessColumn.Name.Equals("ReducedCFM")
        //                    || accessColumn.Name.Equals("ReducedOACFM")
        //                    || accessColumn.Name.Equals("ReducedMADB")
        //                    || accessColumn.Name.Equals("ReducedMAWB")
        //                    || accessColumn.Name.Equals("V208")
        //                    || accessColumn.Name.Equals("OutsideSP")
        //                    || accessColumn.Name.Equals("ExhaustSP")
        //                    || accessColumn.Name.Equals("CompCap")
        //                    || accessColumn.Name.Equals("SplitSysID")
        //                    || accessColumn.Name.Equals("HeatSizing")
        //                    || accessColumn.Name.Equals("DischargeLoss")
        //                    || accessColumn.Name.Equals("SuctionLoss")
        //                    || accessColumn.Name.Equals("LiquidLoss")
        //                    || accessColumn.Name.Equals("LineSizeData")
        //                    || accessColumn.Name.Equals("FLLeavingTemp")
        //                    || accessColumn.Name.Equals("WSE_Operation")
        //                    || accessColumn.Name.Equals("WSE_FLTemp")
        //                    || accessColumn.Name.Equals("DesireGasDB0")
        //                    || accessColumn.Name.Equals("MinVAVSP")//JobID	JobName	JobNumber	JobDescription	SpecialNotes	CustomerContact	AAONContact	CustomerPO	ShopOrderNo	Rep1	Rep2	ReqShipDate	HFA	RTP	ShipVia	Allowed	PPDAdd	Collect	NotifyName	NotifyHours	NotifyPhone	BTU	CustomerID	CustomerName	CustomerAddress1	CustomerAddress2	CustomerCity	CustomerState	CustomerCountry	CustomerZip	
        //                    || accessColumn.Name.Equals("CustomerTel")
        //                    || accessColumn.Name.Equals("CustomerFax")
        //                    || accessColumn.Name.Equals("ShipName")
        //                    || accessColumn.Name.Equals("ShipAddress1")
        //                    || accessColumn.Name.Equals("ShipAddress2")
        //                    || accessColumn.Name.Equals("ShipCity")
        //                    || accessColumn.Name.Equals("ShipState")
        //                    || accessColumn.Name.Equals("ShipCountry")
        //                    || accessColumn.Name.Equals("ShipZip")
        //                    || accessColumn.Name.Equals("RepMult")
        //                    || accessColumn.Name.Equals("MarkUp")
        //                    || accessColumn.Name.Equals("ShipZone")
        //                    || accessColumn.Name.Equals("SiteAltitude")
        //                    || accessColumn.Name.Equals("ActivityDate")
        //                    || accessColumn.Name.Equals("RepContact")
        //                    || accessColumn.Name.Equals("OrderedBy")
        //                    || accessColumn.Name.Equals("PriceVer")
        //                    || accessColumn.Name.Equals("TotalListPrice")
        //                    || accessColumn.Name.Equals("ShippingCharge")
        //                    || accessColumn.Name.Equals("SalesTax")
        //                    || accessColumn.Name.Equals("TaxID")
        //                    || accessColumn.Name.Equals("NetItem1_Desc")
        //                    || accessColumn.Name.Equals("NetItem1_Price")
        //                    || accessColumn.Name.Equals("NetItem2_Desc")
        //                    || accessColumn.Name.Equals("NetItem2_Price")
        //                    || accessColumn.Name.Equals("NetItem3_Desc")
        //                    || accessColumn.Name.Equals("NetItem3_Price")
        //                    || accessColumn.Name.Equals("NetItem4_Desc")
        //                    || accessColumn.Name.Equals("NetItem4_Price")
        //                    || accessColumn.Name.Equals("RepCommission")
        //                    || accessColumn.Name.Equals("RepTotalOrderPrice")
        //                    || accessColumn.Name.Equals("Rep3")
        //                    || accessColumn.Name.Equals("Rep4")
        //                    || accessColumn.Name.Equals("Rep1ComPer")
        //                    || accessColumn.Name.Equals("Rep2ComPer")
        //                    || accessColumn.Name.Equals("Rep3ComPer")
        //                    || accessColumn.Name.Equals("Rep4ComPer")
        //                    || accessColumn.Name.Equals("VersionCheck")
        //                    || accessColumn.Name.Equals("OrigSerial")
        //                    || accessColumn.Name.Equals("EngName")
        //                    || accessColumn.Name.Equals("EngAddress1")
        //                    || accessColumn.Name.Equals("EngAddress2")
        //                    || accessColumn.Name.Equals("EngCity")
        //                    || accessColumn.Name.Equals("EngState")
        //                    || accessColumn.Name.Equals("EngCountry")
        //                    || accessColumn.Name.Equals("EngZip")
        //                    || accessColumn.Name.Equals("MarketingCode")
        //                    || accessColumn.Name.Equals("PreHeatLVTemp")
        //                    || accessColumn.Name.Equals("OptElectricPreheat")
        //                    || accessColumn.Name.Equals("UnitID")
        //                    || accessColumn.Name.Equals("SupplyFanData")
        //                    || accessColumn.Name.Equals("ReturnFanData")
        //                    )
        //                //if (!accessColumn.Name.Equals("ItemNo") && !accessColumn.Name.Equals("ConditionsChanged") && !accessColumn.Name.Equals("DraftUnit"))
        //                {
        //                    accessColumn.Attributes = ColumnAttributesEnum.adColNullable;
        //                }
        //                //&& !accessColumn.Name.Contains("ItemNo")
        //                //&& !accessColumn.Name.Contains("UnitID")
        //                //&& !accessColumn.Name.Contains("ID")) //&& !accessColumn.Name.Contains("ID")

        //                if (accessColumn.Type == DataTypeEnum.adNumeric)
        //                {
        //                    accessColumn.Precision = 10;
        //                }
        //                table.Columns.Append(accessColumn);  //column.ColumnName);

        //            }
        //            Console.WriteLine(table.Name);
        //            cat.Tables.Append(table);
        //        }

        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //    }

        //    //Now Close the database
        //    //ADODB.Connection x = cat.ActiveConnection;
        //    //  ADODB.Connection con = cat.ActiveConnection as ADODB.Connection;
        //    // if (con != null)
        //    // con.Close();

        //    cat = null;
        //    return result;
        //}

        //public static ADOX.DataTypeEnum GetAdoxDataType(DataColumn dc)
        //{
        //    ADOX.DataTypeEnum dte = new ADOX.DataTypeEnum();

        //    if (dc.DataType == typeof(string))
        //    {
        //        dte = DataTypeEnum.adLongVarWChar;
        //    }
        //    else if (dc.DataType == typeof(byte))
        //    {
        //        dte = DataTypeEnum.adUnsignedTinyInt;
        //    }
        //    else if (dc.DataType == typeof(bool))
        //    {
        //        dte = DataTypeEnum.adBoolean;
        //    }
        //    else if (dc.DataType == typeof(DateTime))
        //    {
        //        dte = DataTypeEnum.adDate;
        //    }
        //    else if (dc.DataType == typeof(decimal))
        //    {
        //        dte = DataTypeEnum.adCurrency; //adNumeric
        //    }
        //    else if (dc.DataType == typeof(double))
        //    {
        //        dte = DataTypeEnum.adDouble;
        //    }
        //    else if (dc.DataType == typeof(Guid))
        //    {
        //        dte = DataTypeEnum.adGUID;
        //    }
        //    else if (dc.DataType == typeof(int))
        //    {
        //        dte = DataTypeEnum.adInteger;
        //    }
        //    else if (dc.DataType == typeof(byte[]))
        //    {
        //        dte = DataTypeEnum.adLongVarBinary;
        //    }
        //    else if (dc.DataType == typeof(float))
        //    {
        //        dte = DataTypeEnum.adSingle;
        //    }
        //    else if (dc.DataType == typeof(short))
        //    {
        //        dte = DataTypeEnum.adSmallInt;
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }

        //    return dte;
        //}

        //public static OleDbType GetOleDbDataType(DataColumn dc)
        //{
        //    OleDbType odt = new OleDbType();

        //    if (dc.DataType == typeof(string))
        //    {
        //        odt = OleDbType.LongVarWChar;
        //    }
        //    else if (dc.DataType == typeof(byte))
        //    {
        //        odt = OleDbType.UnsignedTinyInt;
        //    }
        //    else if (dc.DataType == typeof(bool))
        //    {
        //        odt = OleDbType.Boolean;
        //    }
        //    else if (dc.DataType == typeof(DateTime))
        //    {
        //        odt = OleDbType.Date;
        //    }
        //    else if (dc.DataType == typeof(decimal))
        //    {
        //        odt = OleDbType.Numeric;
        //    }
        //    else if (dc.DataType == typeof(double))
        //    {
        //        odt = OleDbType.Double;
        //    }
        //    else if (dc.DataType == typeof(Guid))
        //    {
        //        odt = OleDbType.Guid;
        //    }
        //    else if (dc.DataType == typeof(int))
        //    {
        //        odt = OleDbType.Integer;
        //    }
        //    else if (dc.DataType == typeof(byte[]))
        //    {
        //        odt = OleDbType.LongVarBinary;
        //    }
        //    else if (dc.DataType == typeof(float))
        //    {
        //        odt = OleDbType.Single;
        //    }
        //    else if (dc.DataType == typeof(short))
        //    {
        //        odt = OleDbType.SmallInt;
        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }

        //    return odt;
        //}

        //public static decimal GetDecimal(string stringValue, decimal defaultValue = 0)
        //{
        //    decimal output = defaultValue;
        //    output = decimal.TryParse(stringValue, out output) ? output : defaultValue;

        //    return output;
        //}

        ///// <summary>
        ///// Sometimes the existing ecat access databases require a nonempty string for a field.  
        ///// Since changing the database is not currently a good option, I enter a string containing on blank space.
        ///// </summary>
        ///// <param name="inputString">The string that is being entered in to the access database into a field that requires a nonempty string.</param>
        ///// <returns>A non empty string.  Either the orginal string or a single space.</returns>
        //public static string AvoidEmptyOrNullString(string inputString)
        //{
        //    inputString = inputString.Equals(null) || inputString.Equals(string.Empty) ? " " : inputString;
        //    //inputString = null;
        //    return inputString;
        //}

        public enum ConnectionType
        {
            OleDb,
            SqlClient
        }

        //public static IDbConnection CreateConnection(string databasePath, string user, string password) //ConnectionType ct
        //{
        //    IDbConnection cn = null;
        //    ConnectionType ct = ConnectionType.OleDb;

        //    if (ct == ConnectionType.OleDb)
        //    {
        //        string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databasePath +
        //            ";User Id=" + user + ";Password=" + password + ";";
        //        cn = new System.Data.OleDb.OleDbConnection();
        //        cn.ConnectionString = connectionString;
        //    }

        //    return cn;
        //}

        public static IDbConnection CreateAccessConnection(string databasePath, string workgroupPath, string user, string password) //ConnectionType ct
        {
            IDbConnection cn = null;
            ConnectionType ct = ConnectionType.OleDb;

            if (ct == ConnectionType.OleDb)
            {
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + databasePath +
                    ";Jet OLEDB:System database=" + workgroupPath + ";User Id=" + user + ";Password=" + password + ";";
                cn = new OleDbConnection();
                cn.ConnectionString = connectionString;
            }

            return cn;
        }

        public static IDbConnection CreateSqlConnection(string serverAddress, string database, string user, string password) //ConnectionType ct
        {
            IDbConnection cn = null;
            ConnectionType ct = ConnectionType.SqlClient;

            if (ct == ConnectionType.SqlClient)
            {
                string connectionString = "Server=" + serverAddress +
                    ";Database=" + database + ";User Id=" + user + ";Password=" + password + ";";
                cn = new SqlConnection();
                cn.ConnectionString = connectionString;
                cn.Open();
            }

            return cn;
        }

        public static string CreateTulSqlConnectionString()
        {
            var tulSqlConnectionString = "Data Source=TULSQL;Initial Catalog=ECAT;Persist Security Info=True;User ID=ecatadmin;Password=goldeagle1010";
            return tulSqlConnectionString;
        }

        public static IDbConnection CreateTulSqlConnection()
        {
            var tulSqlConnectionString = CreateTulSqlConnectionString();
            var sqlConnection = new SqlConnection(tulSqlConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public static IDbCommand CreateCommand(string cmt, IDbConnection cn)
        {
            IDbCommand cm = cn.CreateCommand();
            cm.CommandText = cmt;
            cm.Connection = cn;
            return cm;
        }

        public static IDbCommand CreateCommand(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        {
            IDbCommand cm = cn.CreateCommand();

            foreach (OleDbParameter p in pc)
            {
                cm.Parameters.Add(p);
            }

            cm.CommandText = cmt;
            cm.Connection = cn;
            return cm;
        }

        public static IDbCommand CreateCommand(string cmt, List<SqlParameter> pc, IDbConnection cn)
        {
            IDbCommand cm = cn.CreateCommand();

            foreach (SqlParameter p in pc)
            {
                cm.Parameters.Add(p);
            }

            cm.CommandText = cmt;
            cm.Connection = cn;
            return cm;
        }

        public static IDataReader CreateReader(string cmt, IDbConnection cn)
        {
            IDataReader r = null;
            if (cn.State == ConnectionState.Closed) { cn.Open(); }  //TODO: wtf
            using (IDbCommand cm = CreateCommand(cmt, cn))
            {
                r = cm.ExecuteReader();
            }
            return r;
        }

        public static IDataReader CreateReader(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        {
            IDataReader r = null;
            if (cn.State == ConnectionState.Closed) { cn.Open(); } //TODO:wtf
            using (IDbCommand cm = CreateCommand(cmt, pc, cn))
            {
                r = cm.ExecuteReader();
            }

            return r;
        }

        public static IDataReader CreateReader(string cmt, List<SqlParameter> pc, IDbConnection cn)
        {
            IDataReader r = null;

            using (IDbCommand cm = CreateCommand(cmt, pc, cn))
            {
                r = cm.ExecuteReader();
            }

            return r;
        }

        public static int ExecuteNonQuery(string cmt, IDbConnection cn)
        {
            int er = -1;

            using (IDbCommand cm = CreateCommand(cmt, cn))
            {
                er = cm.ExecuteNonQuery();
            }

            return er;
        }

        public static object ExecuteScalar(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        {
            object o;

            using (IDbCommand cm = CreateCommand(cmt, pc, cn))
            {
                o = cm.ExecuteScalar();
            }

            return o;
        }

        public static object ExecuteScalar(string cmt, List<SqlParameter> pc, IDbConnection cn)
        {
            object o;

            using (IDbCommand cm = CreateCommand(cmt, pc, cn))
            {
                o = cm.ExecuteScalar();
            }

            return o;
        }

        public static object ExecuteScalar(string cmt, IDbConnection cn)
        {
            object o;

            using (IDbCommand cm = CreateCommand(cmt, cn))
            {
                o = cm.ExecuteScalar();
            }

            return o;
        }

        public static int ExecuteNonQuery(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        {
            int er = -1;

            //StringBuilder sb1 = new StringBuilder();
            //StringBuilder sb2 = new StringBuilder();
            //StringBuilder sb3 = new StringBuilder();


            //foreach (OleDbParameter p in pc)
            //{
            //    sb1.Append(p.ParameterName + ", ");
            //    sb2.Append(p.OleDbType.ToString() + ", ");
            //    sb3.Append(p.Value + ", ");
            //}

            //Console.WriteLine(sb1.ToString());
            //Console.WriteLine(sb2.ToString()); 
            //Console.WriteLine(sb3.ToString());

            using (IDbCommand cm = CreateCommand(cmt, pc, cn))
            {
                er = cm.ExecuteNonQuery();
            }

            return er;
        }

        public static int ExecuteNonQuery(string cmt, List<SqlParameter> pc, IDbConnection cn)
        {
            int er = -1;

            //StringBuilder sb1 = new StringBuilder();
            //StringBuilder sb2 = new StringBuilder();
            //StringBuilder sb3 = new StringBuilder();


            //foreach (OleDbParameter p in pc)
            //{
            //    sb1.Append(p.ParameterName + ", ");
            //    sb2.Append(p.OleDbType.ToString() + ", ");
            //    sb3.Append(p.Value + ", ");
            //}

            //Console.WriteLine(sb1.ToString());
            //Console.WriteLine(sb2.ToString()); 
            //Console.WriteLine(sb3.ToString());

            using (IDbCommand cm = CreateCommand(cmt, pc, cn))
            {
                er = cm.ExecuteNonQuery();
            }

            return er;
        }

        public static DataTable CreateDataTable(string cmt, IDbConnection cn)
        {
            DataTable dt = new DataTable();

            using (IDataReader r = CreateReader(cmt, cn))
            {
                dt.Load(r);
            }

            return dt;
        }

        public static DataTable CreateDataTable(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        {
            DataTable dt = new DataTable();

            using (IDataReader r = CreateReader(cmt, pc, cn))
            {
                dt.Load(r);
            }

            return dt;
        }

        public static DataTable CreateDataTable(string cmt, List<SqlParameter> pc, IDbConnection cn)
        {
            DataTable dt = new DataTable();

            using (IDataReader r = CreateReader(cmt, pc, cn))
            {
                dt.Load(r);
            }

            return dt;
        }

        public static DataRow CreateDataRow(string cmt, IDbConnection cn)
        {
            DataRow dr = null;

            using (DataTable dt = CreateDataTable(cmt, cn))
            {
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                }
            }

            return dr;
        }

        public static DataRow CreateDataRow(string cmt, List<OleDbParameter> pc, IDbConnection cn)
        {
            DataRow dr = null;

            using (DataTable dt = CreateDataTable(cmt, pc, cn))
            {
                if (dt.Rows.Count > 0)
                {
                    dr = dt.Rows[0];
                }
            }

            return dr;
        }

        public static OleDbParameter CreateOleDbParameter(string name, object value, OleDbType type)
        {
            if (value == null || value.Equals(string.Empty))
            {
                value = System.DBNull.Value;
            }
            OleDbParameter p = new OleDbParameter();
            p.ParameterName = name;
            p.Value = value;
            p.OleDbType = type;
            return p;
        }

        public static OleDbParameter CreateOleDbParameter(string name, object value)
        {
            OleDbParameter p = new OleDbParameter();
            p.ParameterName = name;
            p.Value = value;

            return p;
        }

        public static SqlParameter CreateSqlParameter(string name, object value)
        {
            SqlParameter p = new SqlParameter();
            p.ParameterName = name;
            p.Value = value;

            return p;
        }

        /// <summary>
        /// Creates a DataSet of all of the data in all of the tables in the database.
        /// </summary>
        /// <param name="cn">Connection to the database</param>
        /// <returns>Returns a DataSet of all of the data in all of the tables in the database.</returns>
        public static DataSet CreateDataSet(OleDbConnection cn)
        {
            var dataSet = new DataSet();
            var tableNames = ListTableNames(cn);

            foreach (var tableName in tableNames)
            {
                var cmt = "SELECT * FROM " + tableName;

                using (var dt = CreateDataTable(cmt, cn))
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dt.TableName = tableName;
                        dataSet.Tables.Add(dt);
                    }
                }
            }

            return dataSet;
        }

        public static IList<string> ListTableNames(OleDbConnection cn)
        {
            //var tables = new List<string>();
            //DataTable dt = cn.GetSchema("Tables");
            //foreach (DataRow row in dt.Rows)
            //{
            //    var tablename = (string)row[2];
            //    tables.Add(tablename);
            //}

            System.Collections.Generic.List<string> tables = GetTables(cn);
            return tables;
        }

        //DatabaseInfoCollector

        public static List<string> GetTables(OleDbConnection cn)
        {
            System.Data.DataTable tables;

            tables = cn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            System.Collections.Generic.List<string> Tables = new System.Collections.Generic.List<string>();
            for (int i = 0; i < tables.Rows.Count; i++)
            {
                Tables.Add(tables.Rows[i][2].ToString());
            }
            return Tables;
        }

        public static List<string> GetColumnNames(OleDbConnection cn, string table)
        {
            System.Data.DataTable dataSet = new System.Data.DataTable();

            System.Data.OleDb.OleDbCommand c = new System.Data.OleDb.OleDbCommand("SELECT * FROM " + table, cn);
            using (System.Data.OleDb.OleDbDataAdapter dataAdapter = new System.Data.OleDb.OleDbDataAdapter(c))
            {
                dataAdapter.Fill(dataSet);
            }

            System.Collections.Generic.List<string> columns = new System.Collections.Generic.List<string>();
            for (int i = 0; i < dataSet.Columns.Count; i++)
            {
                columns.Add(dataSet.Columns[i].ColumnName);
            }
            return columns;
        }

        //public static bool CreateNewAccessDatabase(string fileName, string workgroup, string username, string password)
        //{
        //    bool result = false;

        //    ADOX.Catalog cat = new ADOX.Catalog();
        //    ADOX.Table table = new ADOX.Table();

        //    //Create the table and it's fields. 
        //    table.Name = "Table1";

        //    table.Columns.Append("Field1");
        //    table.Columns.Append("Field2");

        //    try
        //    {
        //        //cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + "; Jet OLEDB:Engine Type=5");
        //        cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + ";Jet OLEDB:System Database=" +
        //                   workgroup + ";User ID=" + username + ";Password=" + password + ";");
        //        cat.Tables.Append(table);

        //        //Now Close the database
        //        //ADODB.Connection x = cat.ActiveConnection;
        //        //  ADODB.Connection con = cat.ActiveConnection as ADODB.Connection;
        //        // if (con != null)
        //        // con.Close();

        //        result = true;
        //    }
        //    catch (Exception)
        //    {
        //        result = false;
        //    }
        //    cat = null;
        //    return result;
        //}


        public static OleDbType GetOleDbDataType(DataColumn dc)
        {
            OleDbType odt = new OleDbType();

            if (dc.DataType == typeof(string))
            {
                odt = OleDbType.LongVarWChar;
            }
            else if (dc.DataType == typeof(byte))
            {
                odt = OleDbType.UnsignedTinyInt;
            }
            else if (dc.DataType == typeof(bool))
            {
                odt = OleDbType.Boolean;
            }
            else if (dc.DataType == typeof(DateTime))
            {
                odt = OleDbType.Date;
            }
            else if (dc.DataType == typeof(decimal))
            {
                odt = OleDbType.Numeric;
            }
            else if (dc.DataType == typeof(double))
            {
                odt = OleDbType.Double;
            }
            else if (dc.DataType == typeof(Guid))
            {
                odt = OleDbType.Guid;
            }
            else if (dc.DataType == typeof(int))
            {
                odt = OleDbType.Integer;
            }
            else if (dc.DataType == typeof(byte[]))
            {
                odt = OleDbType.LongVarBinary;
            }
            else if (dc.DataType == typeof(float))
            {
                odt = OleDbType.Single;
            }
            else if (dc.DataType == typeof(short))
            {
                odt = OleDbType.SmallInt;
            }
            else
            {
                throw new Exception();
            }

            return odt;
        }

        public static decimal GetDecimal(string stringValue, decimal defaultValue = 0)
        {
            decimal output = defaultValue;
            output = decimal.TryParse(stringValue, out output) ? output : defaultValue;

            return output;
        }

        //public static string SetFileLocation(string defaultExt, string defaultDirectory)
        //{
        //    var dialog = new OpenFileDialog
        //    {
        //        DefaultExt = defaultExt,
        //        InitialDirectory = defaultDirectory,
        //        FileName = defaultDirectory.Split('\\')[defaultDirectory.Split('\\').Length - 1].ToString()
        //    };
        //    dialog.ShowDialog();

        //    return dialog.FileName;
        //}

        //public static string SetFolderLocation(string defaultDirectory)
        //{
        //    var dialog = new FolderBrowserDialog
        //    {
        //        Description = "Location of ecat 4", 
        //        SelectedPath = defaultDirectory
        //    };
        //    dialog.ShowDialog();

        //    return dialog.SelectedPath;
        //}

        /// <summary>
        /// Sometimes the existing ecat access databases require a nonempty string for a field.  
        /// Since changing the database is not currently a good option, I enter a string containing on blank space.
        /// </summary>
        /// <param name="inputString">The string that is being entered in to the access database into a field that requires a nonempty string.</param>
        /// <returns>A non empty string.  Either the orginal string or a single space.</returns>
        public static string AvoidEmptyOrNullString(string inputString)
        {
            inputString = inputString.Equals(null) || inputString.Equals(string.Empty) ? " " : inputString;
            //inputString = null;
            return inputString;
        }

        public static IDbConnection MakeConnection(string DBNameLoc, string WGNameLoc, string UserID, string PWD)
        {
            if (!System.IO.File.Exists(DBNameLoc))
                throw new Exception("Database file does not exist." + "\n" + DBNameLoc);
            if (!System.IO.File.Exists(WGNameLoc))
                throw new Exception("System file does not exist." + "\n" + WGNameLoc);

            IDbConnection Connection = new System.Data.OleDb.OleDbConnection();
            Connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + DBNameLoc + ";" + "Jet OLEDB:System database=" + WGNameLoc + ";" + "User Id=" + UserID + ";" + "Password=" + PWD + ";";

            Connection.Open();

            return Connection;

        }

        public static IDbConnection MakeConnection(string OpenDatabaseName)
        {

            IDbConnection Connection = new System.Data.OleDb.OleDbConnection();

            Connection.ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\" + OpenDatabaseName + ".accdb;";
            Connection.Open();

            //MsgBox(Connection.ConnectionString & " ||| " & My.Application.Info.DirectoryPath & " ||| " & System.Reflection.Assembly.GetExecutingAssembly.Location())

            return Connection;

        }

        public static Int32 GetIntegerField(IDataRecord dr, string field)
        {
            if (dr.IsDBNull(dr.GetOrdinal(field)))
            {
                return 0;
            }
            else
            {
                return dr.GetInt32(dr.GetOrdinal(field));
            }
        }

        public static decimal GetDecimalField(IDataRecord dr, string field)
        {
            if (dr.IsDBNull(dr.GetOrdinal(field)))
            {
                return 0;
            }
            else
            {
                return dr.GetDecimal(dr.GetOrdinal(field));
            }
        }

        public static string GetStringField(IDataRecord dr, string field)
        {
            if (dr.IsDBNull(dr.GetOrdinal(field)))
            {
                return string.Empty;
            }
            else
            {
                return dr.GetString(dr.GetOrdinal(field));
            }
        }

        public static double GetDoubleField(IDataRecord dr, string field)
        {
            if (dr.IsDBNull(dr.GetOrdinal(field)))
            {
                return double.NaN;
            }
            else
            {
                return dr.GetDouble(dr.GetOrdinal(field));
            }
        }

        public static float GetSingleField(IDataRecord dr, string field)
        {
            if (dr.IsDBNull(dr.GetOrdinal(field)))
            {
                return float.NaN;
            }
            else
            {

                return dr.GetFloat(dr.GetOrdinal(field));
            }
        }

        public static bool GetBooleanField(IDataRecord dr, string field)
        {
            if (dr.IsDBNull(dr.GetOrdinal(field)))
            {
                return false;
            }
            else
            {
                return dr.GetBoolean(dr.GetOrdinal(field));
            }
        }

        public static object GetValueField(IDataRecord dr, string field)
        {
            if (dr.IsDBNull(dr.GetOrdinal(field)))
            {
                return null;
            }
            else
            {
                return dr.GetValue(dr.GetOrdinal(field));
            }
        }

        public static IDataReader RemoveReader(IDataReader r)
        {
            if ((r != null))
            {
                if (!r.IsClosed)
                {
                    r.Close();
                }
                r.Dispose();
                r = null;
            }
            return r;
        }

        public static IDbCommand RemoveCommand(IDbCommand cm)
        {
            if ((cm != null))
            {
                cm.Dispose();
                cm = null;
            }
            return cm;
        }

        //public static IDbCommand CreateCommand(string cmt, IDbConnection cn)
        //{

        //    IDbCommand cm = cn.CreateCommand();
        //    cm.CommandText = cmt;
        //    cm.Connection = cn;
        //    return cm;

        //}

        public static IDataReader CreateReader(IDbCommand cm)
        {

            IDataReader r = cm.ExecuteReader();
            return r;

        }

        //public static IDataReader CreateReader(string cmt, IDbConnection cn)
        //{
        //    IDataReader r = null;

        //    using (IDbCommand cm = CreateCommand(cmt, cn))
        //    {
        //        r = cm.ExecuteReader();
        //    }

        //    return r;
        //}

        //public static DataTable CreateDataTable(string cmt, IDbConnection cn)
        //{
        //    DataTable dt = new DataTable();

        //    using (IDataReader r = CreateReader(cmt, cn))
        //    {
        //        dt.Load(r);
        //    }

        //    return dt;
        //}


        public static string[] getResourceNames(Type Dtype)
        {
            return System.Reflection.Assembly.GetAssembly(Dtype).GetManifestResourceNames();
        }

        public static bool getResourceFile(string EmbeddedFileName, string OutputFileName)
        {

            string dir = Directory.GetCurrentDirectory() + "\\";

            if (string.IsNullOrEmpty(OutputFileName))
            {
                OutputFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + EmbeddedFileName;
            }
            else if (OutputFileName.LastIndexOf("\\") < 0)
            {
                OutputFileName = Directory.GetCurrentDirectory() + "\\" + OutputFileName;
            }
            else if (!System.IO.Directory.Exists(OutputFileName.Substring(0, OutputFileName.LastIndexOf("\\"))))
            {
                OutputFileName = Directory.GetCurrentDirectory() + "\\" + OutputFileName;
            }

            if (System.IO.File.Exists(dir + EmbeddedFileName))
            {
                System.IO.File.Copy(dir + EmbeddedFileName, OutputFileName);
                return true;
            }
            else
            {
                return false;
            }

        }

        public static bool getResourceFile(string EmbeddedFileName, List<Type> Dtype, string OutputFileName)
        {
            bool Success = false;
            foreach (var T in Dtype)
            {
                Success = getResourceFile(EmbeddedFileName, T, OutputFileName);
                if (Success)
                    return true;
            }

            if (getResourceFile(EmbeddedFileName, OutputFileName))
                return true;

            return false;

        }

        public static bool getResourceFile(string EmbeddedFileName, Type Dtype, string OutputFileName)
        {

            if (string.IsNullOrEmpty(OutputFileName))
            {
                OutputFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + EmbeddedFileName;
            }
            else if (OutputFileName.LastIndexOf("\\") < 0)
            {

                OutputFileName = Directory.GetCurrentDirectory() + "\\" + OutputFileName;
            }
            else if (!System.IO.Directory.Exists(OutputFileName.Substring(0, OutputFileName.LastIndexOf("\\"))))
            {
                OutputFileName = Directory.GetCurrentDirectory() + "\\" + OutputFileName;
            }

            System.IO.Stream s = getResourceStream(EmbeddedFileName, Dtype);
            if (s == null)
                return false;

            byte[] bytes = new byte[s.Length];

            File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\" + OutputFileName, bytes);

            return true;

        }

        public static Stream getResourceStream(string EmbeddedFileName, Type Dtype)
        {

            System.Reflection.Assembly _objAssemble = System.Reflection.Assembly.GetAssembly(Dtype);

            System.IO.Stream s = System.IO.Stream.Null;
            s = _objAssemble.GetManifestResourceStream(Dtype, EmbeddedFileName);

            return s;

        }

        /// <summary>
        /// Given a table name and a database connection, returns a string containing comma separated values for the data of a sql datatable
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="cn"></param>
        /// <returns>A string containing comma separated values for the data of a sql datatable</returns>
        public static string GetTableAsString(string tableName, IDbConnection cn)
        {
            var sb = new StringBuilder();
            DataTable dt = CreateDataTable("SELECT * FROM [" + tableName + "]", cn);

            foreach (DataColumn column in dt.Columns)
            {
                sb.Append(column.ColumnName + ",");
            }
            sb.Append("\r\n");

            foreach (DataRow dr in dt.Rows)
            {
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    sb.Append("\"" + dr[i].ToString() + "\"" + ",");
                    //sb.Append(dr[i].ToString().Replace(',', ';') + ",");
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cn"></param>
        /// <returns>A list of string containing all of the user tables in a sql database sorted alphabetically.</returns>
        public static List<string> GetSqlTables(IDbConnection cn)
        {
            var dt = CreateDataTable("SELECT * FROM information_schema.tables", cn);
            var tables = (from DataRow dr in dt.Rows select dr["TABLE_NAME"].ToString()).ToList();
            tables.Sort();
            return tables;
        }

        public static bool CheckColumnExistence(DataTable dt, params string[] columns)
        {
            return (from column in columns
                    where dt.Columns.Contains(column)
                    select column).Any();
        }

    }
}
