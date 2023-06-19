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
            DefineBaseClass(writer, baseName, types);
            DefineNodeDefinitions(writer, baseName, types);
        }
    }
    
    private static void DefineBaseClass(StreamWriter writer, string baseName, List<string> types)
    {
        writer.WriteLine("// This file is generated. Do not edit!");
        writer.WriteLine();
        writer.WriteLine("using Campfire.TreeWalkInterpreter;");
        writer.WriteLine();
        writer.WriteLine($"abstract class {baseName} {{");

        DefineVisitor(writer, baseName, types);
        
        writer.WriteLine();
        writer.WriteLine("  public abstract T Accept<T>(Visitor<T> visitor);");

        writer.WriteLine("}");
        writer.WriteLine();
    }

    private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
    {
        writer.WriteLine();
        writer.WriteLine("  public interface Visitor<T> {");
        foreach (var type in types)
        {
            var typeName = type.Split(":")[0].Trim();
            writer.WriteLine($"    T Visit{typeName}{baseName}({typeName} {baseName.ToLower()});");
        }
        writer.WriteLine("  }");
    }

    private static void DefineNodeDefinitions(StreamWriter writer, string baseName, List<string> types)
    {
        foreach (var type in types)
        {
            DefineType(writer, baseName, type);
        }
    }

    private static void DefineType(StreamWriter writer, string baseName, string type)
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
        
        writer.WriteLine();
        writer.WriteLine($"    public override T Accept<T>(Visitor<T> visitor){{");
        writer.WriteLine($"      return visitor.Visit{className}{baseName}(this);");
        writer.WriteLine("    }");
        writer.WriteLine("  }");
        writer.WriteLine();
    }
}