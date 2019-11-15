using System;
using System.Collections.Generic;
using System.Text;

namespace User.Domain
{
    public interface IState<T>
    {
        T Id { get; }
    }
}
