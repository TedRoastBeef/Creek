using System.Linq.Expressions;

namespace Lib.Tools.TypeBuilder
{
    using Creek.Tools;

    internal interface _ICompilable
	{
		Expression _CompileToExpression();
	}
}
