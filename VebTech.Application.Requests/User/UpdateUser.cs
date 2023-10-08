﻿using FluentValidation;
using MediatR;
using VebTech.Domain.Services;
using UserModel = VebTech.Domain.Models.Entities.User;

namespace VebTech.Application.Requests.User;

public class UpdateUser
{
    public class Request : UserModel, IRequest<UserModel>
    {
        public long QueryId { get; set; }
    }

    public class GetStudentHandler : IRequestHandler<Request, UserModel>, IPipelineBehavior<Request, UserModel>
    {
        private readonly IUserService _studentService;

        public GetStudentHandler(IUserService studentService)
        {
            _studentService = studentService;
        }

        public async Task<UserModel> Handle(Request request, CancellationToken cancellationToken)
        {
            request.UserId = request.QueryId;
            return await _studentService.UpdateUser(request, cancellationToken);
        }

        public async Task<UserModel> Handle(
            Request request,
            RequestHandlerDelegate<UserModel> next,
            CancellationToken cancellationToken)
        {
            return await next();
        }
    }
}