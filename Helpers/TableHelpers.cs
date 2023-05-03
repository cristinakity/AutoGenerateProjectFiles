namespace AutoGenerateProjectFiles.Console.Helpers
{
    public static class TableHelper
    {
        public static string GetSingularName(string tableName)
        {
            var result = tableName;
            var upperTableName = tableName.ToUpper();
            if (upperTableName.EndsWith("IES"))
            {
                result = tableName.Substring(0, tableName.Length - 3) + "y";
            }
            else if (upperTableName.EndsWith("SSES"))
            {
                result = tableName.Substring(0, tableName.Length - 2);
            }
            else if(upperTableName.EndsWith("S"))
            {
                result = tableName.Substring(0, tableName.Length - 1);
            }
            return result;
        }

        public static string GetType(string columnType, bool isNull)
        {
            string type = "";
            switch (columnType)
            {
                case "bigint":
                    type = "long";//nameof(Int64);
                    break;
                case "bit":
                    type = "bool";// typedescriptor.getconverter(typeof(boolean)).convertfromstring(pstrvalue);
                    break;
                case "ntext":
                case "nvarchar":
                case "varchar":
                case "nchar":
                case "text":
                case "char":
                    type = "string";//typedescriptor.getconverter(typeof(string)).convertfromstring(pstrvalue);
                    break;
                case "smalldatetime":
                case "datetime":
                    type = "DateTime";//datetime.parseexact(pstrvalue, pstrdateformat, cultureinfo.invariantculture);
                    break;
                case "money":
                case "smallmoney":
                case "decimal":
                    type = "decimal";//typedescriptor.getconverter(typeof(decimal)).convertfromstring(null, cultureinfo.invariantculture, pstrvalue);
                    break;
                case "float":
                    type = "double";//typedescriptor.getconverter(typeof(double)).convertfromstring(pstrvalue);
                    break;
                case "binary":
                case "varbinary":
                case "timestamp":
                case "image":
                    type = "byte[]";//typedescriptor.getconverter(typeof(byte[])).convertfromstring(pstrvalue);
                    break;
                case "int":
                    type = "int";//typedescriptor.getconverter(typeof(int32)).convertfromstring(pstrvalue);
                    break;
                case "real":
                    type = "single";//typedescriptor.getconverter(typeof(single)).convertfromstring(pstrvalue);
                    break;
                case "smallint":
                    type = "int16";//typedescriptor.getconverter(typeof(int16)).convertfromstring(pstrvalue);
                    break;
                case "tinyint":
                    type = "byte";//typedescriptor.getconverter(typeof(byte)).convertfromstring(pstrvalue);
                    break;
                case "uniqueidentifier":
                    type = "Guid";//typedescriptor.getconverter(typeof(byte)).convertfromstring(pstrvalue);
                    break;
            }
            return isNull ?  $"{type}?" : type;
        }
    }
}
