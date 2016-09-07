using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HexagonService.Contracts
{
    interface IUnitOfWork : IDisposable
    {
        void RollBack();
        void Commit();
    }
}
