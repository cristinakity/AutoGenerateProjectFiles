namespace AutoGenerateProjectFiles.Console.Data
{
    public class Repository
    {
        private readonly string _connectionString;

        public Repository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TableName> GetTableNames()
        {
            using var dbConnection = new SqlConnection(_connectionString);
            dbConnection.Open();
            var query = @"SELECT s.[name] as [Schema], o.[name] as [Table]
                          FROM sys.objects (NOLOCK) o 
                          INNER JOIN sys.schemas (NOLOCK) s
	                          ON o.[schema_id] = s.[schema_id]
                          WHERE o.[type] = 'U' 
	                          AND o.[name] NOT IN ('__RefactorLog','__EFMigrationsHistory','__MigrationHistory', 'sysdiagrams')
                          ORDER BY 1,2";
            var result = dbConnection.Query<TableName>(query);
            return result;
        }
        
        public IEnumerable<Column> GetColumns(TableName tableName)
        {
            using var dbConnection = new SqlConnection(_connectionString);
            dbConnection.Open();
            var query = @"SELECT DISTINCT
                            --Id = ROW_NUMBER() OVER(ORDER BY (SELECT NULL))
	                         c.[COLUMN_NAME]as [Name]
	                        ,c.[DATA_TYPE] as [Type]
	                        ,CASE
		                        WHEN pk.[COLUMN_NAME] IS NULL THEN CONVERT(bit,0)
		                        ELSE CONVERT(bit,1)
	                        END AS [IsPrimaryKey]
	                        ,CASE c.[is_nullable]
		                        WHEN 'NO' THEN CONVERT(bit,0)
		                        ELSE CONVERT(bit,1)
	                        END AS [IsNull]
	                        ,ISNULL(c.CHARACTER_MAXIMUM_LENGTH,0) as [MaxLength]
							,CASE COLUMNPROPERTY (OBJECT_ID(@table),c.COLUMN_NAME ,'IsIdentity') WHEN 1 THEN 1 ELSE 0 END IsIdentity
                        FROM INFORMATION_SCHEMA.COLUMNS as c
                        LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
	                        ON	tc.TABLE_NAME	= c.TABLE_NAME
	                        AND tc.TABLE_SCHEMA = c.TABLE_SCHEMA
                        LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE pk
	                        ON	pk.TABLE_NAME	= tc.TABLE_NAME
	                        AND pk.TABLE_SCHEMA = tc.TABLE_SCHEMA
	                        AND pk.COLUMN_NAME  = c.COLUMN_NAME
	                        AND pk.CONSTRAINT_NAME like 'PK%'
                        WHERE c.TABLE_NAME = @Table and c.TABLE_SCHEMA = @Schema";            
            var parameters = new DynamicParameters();
            parameters.Add("Schema", tableName.Schema, DbType.String);
            parameters.Add("Table", tableName.Table, DbType.String);
            var result = dbConnection.Query<Column>(query, parameters);
            return result;
        }
    }
}
