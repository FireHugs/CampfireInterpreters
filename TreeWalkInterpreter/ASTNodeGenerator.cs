using System.IO.MemoryMappedFiles;

namespace Campfire.TreeWalkInterpreter;

public static class ASTNodeDefinitionTextEmitGenerator
{
    public static void GenerateNodeDefinition(string outputDir, string baseName, List<string> types)
    {
        var path = Path.Combine(outputDir, $"{baseName}.cs");

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        using (var writer = new StreamWriter(path))
        {
            writer.WriteLine("// This file is generated. Do not edit!");
            writer.WriteLine();
            writer.WriteLine("using Campfire.TreeWalkInterpreter;");
            writer.WriteLine();
            writer.WriteLine($"abstract class {baseName} {{");
            
            foreach (var type in types)
            {
                var splitType = type.Split(':');
                var className = splitType[0].Trim();
                var fields = splitType[1].Trim();
                
                //Class Definition
                writer.WriteLine($"  class {className} : {baseName} {{");
                
                var splitFields = fields.Split(", ");
                foreach (var field in splitFields)
                {
                    writer.WriteLine($"    public readonly {field};");
                }
                writer.WriteLine();
                
                //Constructor
                writer.WriteLine($"    {className}({fields}){{");
                foreach (var field in splitFields)
                {
                    var name = field.Split(" ")[1]; 
                    writer.WriteLine($"      this.{name} = {name};");
                }
                writer.WriteLine("    }");
                writer.WriteLine("  }");
                writer.WriteLine();
            }
            
            writer.WriteLine("}");
        }
        
        
    }
    
}