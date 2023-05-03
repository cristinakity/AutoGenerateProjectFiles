namespace AutoGenerateProjectFiles.Console.Business
{
    public class FilesGenerator
    {
        private readonly Repository _repository;
        private readonly string _solutionName;
        private readonly bool _folderPerSchema;
        public FilesGenerator(string connectionString, string solutionName, bool folderPerSchema)
        {
            _repository = new Repository(connectionString);
            _solutionName = solutionName;
            _folderPerSchema = folderPerSchema;
        }
        public void Start()
        {
            var tableNames = _repository.GetTableNames();
            var tables = GetTables(tableNames.ToList());
            GenerateFiles(tables);
        }

        private List<Table> GetTables(List<TableName> tableNames)
        {
            int id = 0;
            var tables = new List<Table>();
            foreach(var tableName in tableNames)
            {
                id++;
                var table = new Table();
                table.Id = id;
                table.Schema = tableName.Schema;
                table.OriginalName = tableName.Table;
                table.Name = TableHelper.GetSingularName(tableName.Table).FirstLetterUpper();
                table.Folders = tableName.Schema.FirstLetterUpper();
                table.Columns = _repository.GetColumns(tableName).ToList();
                tables.Add(table);
            }
            return tables;
        }

        #region Gereate Files

        private void GenerateFiles(List<Table> tables)
        {
            var rootFolderPrependName = $"{DateTime.Now:yyyyMMdd_hhmmss}";
            System.Console.WriteLine($"Generating files in:{rootFolderPrependName}");
            var config = new GenerateFileConfig() 
            { 
                SolutionName = _solutionName, 
                RootPath = rootFolderPrependName, 
                Tables = tables 
            };
            //Domain
            config.Section = "Domain";
            //Create GlobalUsings Folder
            config.Template = Templates.Templates.Domain_GlobalUsings;
            GenerateSingleFile(config, fileName: "GlobalUsings");
            //  Model (Entity)
            config.Template = Templates.Templates.Domain_Model;
            GenerateFiles(config, "Models");
            //  Responses (EntityResponse)
            config.Template = Templates.Templates.Domain_Response;
            GenerateFiles(config, "Responses");
            //  Responses (EntityPayload)
            config.Template = Templates.Templates.Domain_Payload;
            GenerateFiles(config, "Payloads");

            //Data
            config.Section = "Data";
            //Create GlobalUsings Folder
            config.Template = Templates.Templates.Data_GlobalUsings;
            GenerateSingleFile(config, fileName: "GlobalUsings");
            //Repositories
            var folder = "Repositories";
            //Repsitories/Core/ GenericRepository
            var subFolder = "Core";
            config.Template = Templates.Templates.Data_IGenericRepository;
            GenerateSingleFile(config, fileName: "IGenericRepository", folder: $"{folder}/{subFolder}");
            config.Template = Templates.Templates.Data_GenericRepository;
            GenerateSingleFile(config, fileName: "GenericRepository", folder: $"{folder}/{subFolder}");
            // Repositories(EntityRepository, IEntityRepository)
            config.Template = Templates.Templates.Data_IRepository;
            GenerateFiles(config, "Repositories", true);
            config.Template = Templates.Templates.Data_Repository;
            GenerateFiles(config, "Repositories");
            //Business
            config.Section = "Business";
            config.Template = Templates.Templates.Business_GlobalUsings;
            GenerateSingleFile(config, fileName: "GlobalUsings");
            folder = "Services";
            //Repositories/Core/ GenericRepository
            config.Template = Templates.Templates.Business_IGenericService;
            GenerateSingleFile(config, fileName: "IGenericService", folder: $"{folder}/{subFolder}");
            config.Template = Templates.Templates.Business_GenericService;
            GenerateSingleFile(config, fileName: "GenericService", folder: $"{folder}/{subFolder}");
            //  Services(EntityService, IEntityService)
            config.Template = Templates.Templates.Business_IService;
            GenerateFiles(config, "Services", true);
            config.Template = Templates.Templates.Business_Service;
            GenerateFiles(config, "Services");
            //Api
            config.Section = "API";
            config.Template = Templates.Templates.Api_Globalusings;
            GenerateSingleFile(config, fileName: "GlobalUsings");
            config.Template = Templates.Templates.Api_MappingProfile;
            GenerateSingleFile(config, fileName: "MappingProfile");
            config.Template = Templates.Templates.Api_Configuration;
            GenerateSingleFile(config, fileName: "Configuration");
            config.Template = Templates.Templates.Api_Configuration;
            GenerateSingleFile(config, fileName: "Configuration");
            folder = "Controllers";
            config.Template = Templates.Templates.Api_GenericController;
            GenerateSingleFile(config, fileName: "GenericController", folder: $"{folder}/{subFolder}");
            config.Template = Templates.Templates.Api_Controller;
            GenerateFiles(config, "Controllers");
        }

        #region Domain files

        #endregion Domain files

        private void GenerateFiles(GenerateFileConfig config, string folder, bool isInterface = false)
        {
            var rootFolder = $"{config.RootPath}/{_solutionName}.{config.Section}";
            if (!string.IsNullOrEmpty(folder))
            {
                rootFolder += $"/{folder}";
            }
            System.Console.WriteLine($"     Folder:{rootFolder}");
            foreach(var table in config.Tables)
            {
                var fileName = (isInterface ? "I" : "") + $"{table.Name}";
                if(folder != "Models")
                {
                    fileName +=  TableHelper.GetSingularName(folder);
                }
                var filePath = _folderPerSchema ? $"{rootFolder}/{table.Folders}" : rootFolder;
                System.Console.WriteLine($"         Genrating file {filePath}/{fileName} for table {table.Schema}.{table.OriginalName}");
                
                Directory.CreateDirectory(filePath);
                var x = 2 != 7;
                var template = config.Template;
                template = template.Replace("{{ProjectName}}", config.SolutionName);
                template = template.Replace("{{Section}}", config.Section);
                template = template.Replace("{{Folder}}", folder);
                template = template.Replace("{{EntityName}}", table.Name);
                template = template.Replace("{{TableName}}", table.OriginalName);
                if (template.Contains("{{Properties}}") && table.Columns != null && table.Columns.Any())
                {
                    template = template.Replace("{{Properties}}", GetProperties(table.Columns));
                }
                File.WriteAllText($"{filePath}/{fileName}.cs", template);
            }
        }

        private void GenerateSingleFile(GenerateFileConfig config, string fileName, string folder = "")
        {
            var rootFolder = $"{config.RootPath}/{_solutionName}.{config.Section}";
            if (!string.IsNullOrEmpty(folder))
            {
                rootFolder += $"/{folder}";
            }
            System.Console.WriteLine($"     Folder:{rootFolder}");
            System.Console.WriteLine($"         Genratin file {rootFolder}/{fileName}");

            Directory.CreateDirectory(rootFolder);
            var x = 2 != 7;
            var template = config.Template;
            template = template.Replace("{{ProjectName}}", config.SolutionName);
            template = template.Replace("{{Section}}", config.Section);
            template = template.Replace("{{Folder}}", folder.Replace("/","."));
            template = template.Replace("{{ClassName}}", fileName);
            if (template.Contains("{{MapEntityToResponse}}") && config.Tables.Any())
            {
                template = template.Replace("{{MapEntityToResponse}}", GetMapEntityToResponse(config.Tables));
            }
            if (template.Contains("{{MapPayloadToEntity}}") && config.Tables.Any())
            {
                template = template.Replace("{{MapPayloadToEntity}}", GetMapPayloadToEntity(config.Tables));
            }
            if (template.Contains("{{Repositories}}") && config.Tables.Any())
            {
                template = template.Replace("{{Repositories}}", GetRepositories(config.Tables));
            }
            if (template.Contains("{{Services}}") && config.Tables.Any())
            {
                template = template.Replace("{{Services}}", GetServices(config.Tables));
            }
            File.WriteAllText($"{rootFolder}/{fileName}.cs", template);
        }

        private string GetRepositories(List<Table> tables)
        {
            var result = "";
            foreach (var table in tables)
            {
                var template = Templates.Templates.Api_Repositories;
                template = template.Replace("{{EntityName}}", table.Name);
                result += template + Environment.NewLine;
            }
            return result;
        }

        private string GetServices(List<Table> tables)
        {
            var result = "";
            foreach (var table in tables)
            {
                var template = Templates.Templates.Api_Services;
                template = template.Replace("{{EntityName}}", table.Name);
                result += template + Environment.NewLine;
            }
            return result;
        }

        private string GetMapPayloadToEntity(List<Table> tables)
        {
            var result = "";
            foreach (var table in tables)
            {
                var template = Templates.Templates.Api_MapPayloadToEntity;
                template = template.Replace("{{EntityName}}", table.Name);
                result += template + Environment.NewLine;
            }
            return result;
        }

        private string GetMapEntityToResponse(List<Table> tables)
        {
            var result = "";
            foreach(var table in tables)
            {
                var template = Templates.Templates.Api_MapEntityToResponse;
                template = template.Replace("{{EntityName}}", table.Name);
                result += template + Environment.NewLine;
            }
            return result;
        }

        private string GetProperties(List<Column> columns)
        {
            string properties = "";
            foreach(var column in columns.OrderByDescending(x=> x.IsPrimaryKey))
            {
                var template = Templates.Templates.Property;
                template = template.Replace("{{Attributes}}", GetAttributes(column));
                template = template.Replace("{{DataType}}", TableHelper.GetType(column.Type, column.IsNull));
                template = template.Replace("{{Name}}", column.Name);
                properties += template + Environment.NewLine;
            }
            return properties;
        }

        private string GetAttributes(Column column)
        {
            string attributes = "";
            var keyAttribute        =  "[Key]\n";
            var identityAttribute   = "[DatabaseGenerated(DatabaseGeneratedOption.Identity)]\n";
            var requiredAttribute   =  "        [Required]\n";
            var maxLengthAttribute  = $"        [MaxLength({column.MaxLength})]\n";
            maxLengthAttribute = column.MaxLength == -1 ? "" : maxLengthAttribute;
            attributes += column.IsPrimaryKey ? keyAttribute : "";
            attributes += column.IsIdentity ? identityAttribute : "";
            attributes += column.IsNull == false ? requiredAttribute : "";
            attributes += (new List<string>{ "ntext", "nvarchar", "varchar", "nchar", "text", "char" }).Any(x=> x == column.Type)
                          ? maxLengthAttribute : "";
            return attributes;
        }

        #endregion Gereate Files
    }
}
