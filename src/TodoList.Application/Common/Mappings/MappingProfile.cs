using System.Reflection;
using AutoMapper;

namespace TodoList.Application.Common.Mappings;

// 继承了Profile的类在注入AutoMapper后会创建实例对象(调用构造函数)
public class MappingProfile : Profile
{
    public MappingProfile() => ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        // Assembly.GetExportedTypes() 获取此程序集中定义的公共类型
        // Type.GetInterfaces() 指示当前类型是否是泛型类型
        // Type.GetGenericTypeDefinition() 返回一个表示可用于构造当前泛型类型的泛型类型定义的 Type 对象
        // 获取程序集中所有实现了IMapFrom<>接口的类
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        // 创建这些类的实例并调用Mapping方法，未实现Mapping方法则调用IMapFrom的Mapping方法
        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                             ?? type.GetInterface("IMapFrom`1")!.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}