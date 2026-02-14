using MelonLoader;
using System.Reflection;
using System.Runtime.InteropServices;
using BuildInfo = TLDTestMod.BuildInfo;

[assembly: AssemblyTitle(BuildInfo.Name)]
[assembly: AssemblyDescription(BuildInfo.Description)]
[assembly: AssemblyCompany(BuildInfo.Company)]
[assembly: AssemblyProduct(BuildInfo.Product)]
[assembly: AssemblyCopyright(BuildInfo.Copyright)]
[assembly: AssemblyTrademark(BuildInfo.Trademark)]
[assembly: AssemblyCulture(BuildInfo.Culture)]

[assembly: AssemblyVersion(BuildInfo.Version)]
[assembly: AssemblyFileVersion(BuildInfo.Version)]

[assembly: ComVisible(false)]
[assembly: Guid("336CB1E0-AC1D-41BA-ACB7-A61A838D35C6")]

[assembly: MelonInfo(typeof(TLDTestMod.Main), BuildInfo.GUIName, BuildInfo.Version, BuildInfo.Author, BuildInfo.DownloadLink)]
[assembly: MelonGame("Hinterland", "TheLongDark")]

[assembly: MelonPriority(BuildInfo.Priority)]
[assembly: MelonIncompatibleAssemblies(null)]