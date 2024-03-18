using System.Runtime.CompilerServices;
using LostNotes;

[assembly: InternalsVisibleTo(AssemblyInfo.NAMESPACE_EDITOR)]

namespace LostNotes {
	internal static class AssemblyInfo {
		public const string NAMESPACE_RUNTIME = "LostNotes";
		public const string NAMESPACE_EDITOR = "LostNotes.Editor";
		public const string NAMESPACE_TESTS = "LostNotes.Tests";
	}
}
