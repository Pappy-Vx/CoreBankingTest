using CoreBankingTest.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBankingTest.Application.Common.Interfaces
{
    public interface ICommand :IRequest<Result> { }
    public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
    public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }


}
