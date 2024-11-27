using MediatR;
using portlocator.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace portlocator.Application.Abstraction.Messaging
{
    public interface ICommand : IRequest<Result>;

    public interface ICommand<T> : IRequest<Result<T>>;
}
