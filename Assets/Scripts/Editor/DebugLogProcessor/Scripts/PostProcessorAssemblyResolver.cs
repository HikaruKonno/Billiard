using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Unity.CompilationPipeline.Common.ILPostProcessing;

//public class PostProcessorAssemblyResolver : IAssemblyResolver
//{
//    private readonly Dictionary<string, AssemblyDefinition> _assemblies = new();

//    public PostProcessorAssemblyResolver(ICompiledAssembly compiledAssembly)
//    {
//        foreach (var reference in compiledAssembly.References.Distinct())
//        {
//            if (!File.Exists(reference)) continue;
//            try
//            {
//                var asm = AssemblyDefinition.ReadAssembly(reference);
//                _assemblies[asm.Name.Name] = asm;
//            }
//            catch { }
//        }
//    }

//    public AssemblyDefinition Resolve(AssemblyNameReference name) =>
//        _assemblies.TryGetValue(name.Name, out var asm) ? asm : null;

//    public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters) =>
//        Resolve(name);

//    public void Dispose() { foreach (var asm in _assemblies.Values) asm.Dispose(); }
//}
