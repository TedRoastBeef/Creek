using System.Linq.Expressions;

namespace Lib.Tools.TypeBuilder
{
	internal interface _ILoop:
		_IBlockBuilder
	{
		LabelTarget ContinueTarget { get; }
		LabelTarget BreakTarget { get; }
	}
}
