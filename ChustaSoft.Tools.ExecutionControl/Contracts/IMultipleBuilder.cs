using ChustaSoft.Common.Helpers;
using System.Collections.Generic;

namespace ChustaSoft.Tools.ExecutionControl.Contracts
{
    public interface IMultipleBuilder<T> : IBuilder<T> where T : class
    {

        IEnumerable<T> BuildAll();

    }
}
