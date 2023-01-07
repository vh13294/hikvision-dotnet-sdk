using System.Reflection;
using System.Runtime.CompilerServices;

//General information about assemblies is controlled by the following set of attributes. Changing these property values modifies the information associated with the assembly.
//
[assembly: AssemblyTitle("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]		

//
// The version information for an assembly consists of the following four values:
//
//      main version
//      sub version 
//      internal version
//      build date
//
// You can specify all of these values, or you can use the default values for Revision Number and Build Number by using '*' as follows:

[assembly: AssemblyVersion("1.0.*")]

//
// To sign an assembly, you must specify which key to use. For more information on assembly signing, please refer to the Microsoft .NET Framework documentation.
//
// Use the following properties to control the key used for signing.
//
// Notice:
//   (*) 如果未指定密钥，则程序集不会被签名。
//   (*) KeyName 是指已经安装在计算机上的
//      加密服务提供程序(CSP)中的密钥。KeyFile 是指包含
//       密钥的文件。
//   (*) 如果 KeyFile 和 KeyName 值都已指定，则 
//       发生下列处理:
//       (1) 如果在 CSP 中可以找到 KeyName，则使用该密钥。
//       (2) 如果 KeyName 不存在而 KeyFile 存在，则 
//           KeyFile 中的密钥安装到 CSP 中并且使用该密钥。
//   (*) 要创建 KeyFile，可以使用 sn.exe(强名称)实用工具。
//       在指定 KeyFile 时，KeyFile 的位置应该相对于
//       项目输出目录，即
//       %Project Directory%\obj\<configuration>。例如，如果 KeyFile 位于
//       该项目目录，应将 AssemblyKeyFile 
//       属性指定为 [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) “延迟签名”是一个高级选项 - 有关它的更多信息，请参阅 Microsoft .NET Framework
//       文档。
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
