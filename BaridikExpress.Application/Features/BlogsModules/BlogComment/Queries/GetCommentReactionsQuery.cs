using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using MediatR;


namespace BaridikExpress.Application.Features.BlogsModules.BlogComment.Queries;

public class GetCommentReactionsQuery : IRequest<Result<CommentReactionsResponse>>
{
    public Guid CommentId { get; set; }
}
