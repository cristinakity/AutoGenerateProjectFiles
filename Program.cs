// See https://aka.ms/new-console-template for more information
using AutoGenerateProjectFiles.Console.Business;

string connectionString = "Server=(LOCAL);Database=Citamania;User Id=sa;Password=123456789;";
string solutionName = "CitamaniaPOC";
string folderPerSchema = "";
if(args is { Length: > 1 })
{
    connectionString = args[0];
    solutionName = args[1];
    folderPerSchema = args[2];

}
FilesGenerator filesGenerator = new FilesGenerator(connectionString, solutionName, folderPerSchema == "1");
filesGenerator.Start();