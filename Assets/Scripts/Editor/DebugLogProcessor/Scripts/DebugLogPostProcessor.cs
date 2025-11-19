using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.CompilationPipeline.Common.ILPostProcessing;
using Unity.CompilationPipeline.Common.Diagnostics;

//public class DebugLogPostProcessor : ILPostProcessor
//{
//    public override ILPostProcessor GetInstance() => this;      
//    public override bool WillProcess(ICompiledAssembly compiledAssembly) =>
//        !compiledAssembly.Name.StartsWith("Unity") && !compiledAssembly.Name.Contains("Editor");

//    public override ILPostProcessResult Process(ICompiledAssembly compiledAssembly)
//    {
//        var diagnostics = new List<DiagnosticMessage>();
//        var resolver = new PostProcessorAssemblyResolver(compiledAssembly);
//        var readerParams = new ReaderParameters 
//        { 
//            ReadWrite = false, 
//            AssemblyResolver = resolver 
//        };

//        // InMemoryAssembly から PE データを取得する
//        using var peStream = new MemoryStream(compiledAssembly.InMemoryAssembly.PeData);
//        var assemblyDefinition = AssemblyDefinition.ReadAssembly(peStream, readerParams);

//        foreach (var type in assemblyDefinition.MainModule.Types)
//        {
//            foreach (var method in type.Methods.Where(m => m.HasCustomAttributes))
//            {
//                if (method.CustomAttributes.Any(a => a.AttributeType.Name == nameof(DebugLogAttribute)))
//                {
//                    InjectDebugLog(method, type);
//                }
//            }
//        }

//        using var ms = new MemoryStream();
//        assemblyDefinition.Write(ms);
//        var modifiedPe = ms.ToArray();

//        var inMemoryAssembly = new InMemoryAssembly(modifiedPe, compiledAssembly.InMemoryAssembly.PdbData);

//        return new ILPostProcessResult(inMemoryAssembly, diagnostics);
//    }

//    private void InjectDebugLog(MethodDefinition method, TypeDefinition type)
//    {
//        var il = method.Body.GetILProcessor();
//        var first = method.Body.Instructions[0];
//        var msg = $"[{type.Name}.{method.Name}] called";

//        // UnityEngine.Debug.Log を直接参照せずに、文字列で指定して IL に注入する
//        var unityEngineRef = method.Module.AssemblyReferences.FirstOrDefault(ar => ar.Name == "UnityEngine");
//        if (unityEngineRef == null) { return; } // UnityEngine 参照がない場合はスキップ

//        var unityEngine = method.Module.AssemblyResolver.Resolve(unityEngineRef);
//        var debugType = unityEngine.MainModule.Types.First(t => t.FullName == "UnityEngine.Debug");
//        var logMethod = debugType.Methods.First(m =>
//            m.Name == "Log" &&
//            m.Parameters.Count == 1 &&
//            m.Parameters[0].ParameterType.FullName == "System.Object"
//        );

//        var importedLog = method.Module.ImportReference(logMethod);

//        il.InsertBefore(first, il.Create(OpCodes.Ldstr, msg));
//        il.InsertBefore(first, il.Create(OpCodes.Call, importedLog));
//    }
//}

