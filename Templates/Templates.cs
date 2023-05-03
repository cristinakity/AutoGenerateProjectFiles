namespace AutoGenerateProjectFiles.Console.Templates
{
    public class Templates
    {
        public static string Property =>                File.ReadAllText(@"Templates/Property.Template");
        public static string Domain_GlobalUsings =>     File.ReadAllText(@"Templates/Domain/Globalusings.Template");
        public static string Domain_Model =>            File.ReadAllText(@"Templates/Domain/Models/Model.Template");
        public static string Domain_Response =>         File.ReadAllText(@"Templates/Domain/Responses/Response.Template");
        public static string Domain_Payload =>          File.ReadAllText(@"Templates/Domain/Payloads/Payload.Template");
        public static string Data_GlobalUsings =>       File.ReadAllText(@"Templates/Data/Globalusings.Template");
        public static string Data_IGenericRepository => File.ReadAllText(@"Templates/Data/Repositories/Core/IGenericRepository.Template");
        public static string Data_GenericRepository =>  File.ReadAllText(@"Templates/Data/Repositories/Core/GenericRepository.Template");
        public static string Data_IRepository =>        File.ReadAllText(@"Templates/Data/Repositories/IRepository.Template");
        public static string Data_Repository =>         File.ReadAllText(@"Templates/Data/Repositories/Repository.Template");
        public static string Business_GlobalUsings =>   File.ReadAllText(@"Templates/Business/Globalusings.Template");
        public static string Business_IGenericService =>File.ReadAllText(@"Templates/Business/Services/Core/IGenericService.Template");
        public static string Business_GenericService => File.ReadAllText(@"Templates/Business/Services/Core/GenericService.Template");
        public static string Business_IService =>       File.ReadAllText(@"Templates/Business/Services/IService.Template");
        public static string Business_Service =>        File.ReadAllText(@"Templates/Business/Services/Service.Template");
        public static string Api_Globalusings =>        File.ReadAllText(@"Templates/Api/Globalusings.Template");
        public static string Api_MappingProfile =>      File.ReadAllText(@"Templates/Api/MappingProfile.Template");
        public static string Api_MapEntityToResponse => File.ReadAllText(@"Templates/Api/MapEntityToResponse.Template");
        public static string Api_MapPayloadToEntity =>  File.ReadAllText(@"Templates/Api/MapPayloadToEntity.Template");
        public static string Api_Configuration =>       File.ReadAllText(@"Templates/Api/Configuration.Template");
        public static string Api_Repositories =>        File.ReadAllText(@"Templates/Api/Repositories.Template");
        public static string Api_Services =>            File.ReadAllText(@"Templates/Api/Services.Template");
        public static string Api_GenericController =>   File.ReadAllText(@"Templates/Api/Controllers/GenericController.Template");
        public static string Api_Controller =>          File.ReadAllText(@"Templates/Api/Controllers/Controller.Template");
    }
}
