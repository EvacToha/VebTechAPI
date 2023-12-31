﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using VebTech.API.Controllers.Constants;
using VebTech.Application.Requests.User;
using VebTech.Domain.Models.Entities;
using VebTech.Domain.Models.Queries;
using VebTech.Domain.Models.Responses;

namespace VebTech.API.Controllers;

public class UserController : BaseController
{
    private readonly IMediator _mediator;
    
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// Добавить пользователя
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     POST /user
    ///     {
    ///        "name" : "Anton",
    ///        "age" : 11,
    ///        "email": "abc@mail.ru",
    ///        "roles": [
    ///            "User", "SuperAdmin", "Admin" 
    ///        ]
    ///     }
    /// 
    /// </remarks>
    /// <param name="request">Пользователь</param>
    /// <returns></returns>
    [ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<User> AddUser([FromBody] AddUser.Request request, CancellationToken cancellationToken) =>
        await _mediator.Send(
            new AddUser.Request 
            { 
                Email = request.Email, 
                Name = request.Name, 
                Age = request.Age,
                Roles = request.Roles
            }, cancellationToken);
    
    /// <summary>                                            
    /// Добавить роль пользователю                              
    /// </summary>                                           
    /// <param name="userRole">Добавляемая роль</param>
    /// <param name="userId">Айди пользователя</param> 
    /// <returns></returns>
    [ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [HttpPost(RouteNames.Id)]
    public async Task<User> AddUserRole(
        [FromQuery] UserRole userRole,
        [FromRoute] long userId,
        CancellationToken cancellationToken) =>
        await _mediator.Send(
            new AddUserRole.Request 
            {  
                UserId = userId,
                UserRole = userRole,
            }, cancellationToken);

    /// <summary>                                            
    /// Получить пользователя                              
    /// </summary>                                           
    /// <param name="userId">Айди пользователя</param> 
    /// <returns></returns>
    [ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [HttpGet(RouteNames.Id)]
    public async Task<User> GetUser([FromRoute] long userId, CancellationToken cancellationToken) =>
        await _mediator.Send(
            new GetUser.Request
            {
                UserId = userId
            }, cancellationToken);
    
    /// <summary>                                            
    /// Получить всех пользователей                              
    /// </summary>                                           
    /// <param name="pageNumber">Номер страницы</param>
    /// <param name="pageSize">Размер страницы</param> 
    /// <param name="modifiers">Модификаторы запроса</param>
    /// <remarks>
    ///
    /// Пример запроса:
    ///
    ///     POST /users
    ///     {
    ///       "sorting": {
    ///         "sortingActions": [
    ///           {
    ///             "property": "name",
    ///             "isAscending": true
    ///           }
    ///         ]
    ///       },
    ///       "filter": {
    ///         "filterActions": [
    ///             {
    ///             "property": "Name",
    ///             "filterValue": "string",
    ///             "method": "Start"
    ///             }
    ///         ]
    ///       }
    ///     }
    /// 
    /// </remarks>
    /// <returns></returns>
    [ProducesResponseType(typeof(PaginatedList<User>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [HttpPost("/api/users")]
    public async Task<PaginatedList<User>> GetUsers(
        [FromQuery] int pageNumber,
        [FromQuery] int pageSize,
        [FromBody] Modifiers modifiers,
        CancellationToken cancellationToken) =>
        await _mediator.Send(
            new GetUsers.Request
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                Modifiers = modifiers
            }, cancellationToken);

    /// <summary>                                            
    /// Обновить пользователя                              
    /// </summary>                                           
    /// <param name="userId">Айди пользователя</param>
    /// <param name="request">Новый пользователь</param>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     POST /user/1
    ///     {
    ///        "name" : "Anton",
    ///        "age" : 11,
    ///        "email": "abc@mail.ru",
    ///        "roles": [
    ///            "User", "SuperAdmin", "Admin" 
    ///        ]
    ///     }
    /// 
    /// </remarks>
    /// <returns></returns>
    [ProducesResponseType(typeof(User),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [HttpPut(RouteNames.Id)]
    public async Task<User> UpdateUser([FromRoute] long userId, [FromBody] UpdateUser.Request request, CancellationToken cancellationToken) =>
        await _mediator.Send(
            new UpdateUser.Request
            {  
                QueryId = userId,
                Age = request.Age,
                Email = request.Email,
                Name = request.Name,
                Roles = request.Roles
            }, cancellationToken);

    /// <summary>                                            
    /// Удалить пользователя                              
    /// </summary>                                           
    /// <param name="userId">Айди пользователя</param>
    /// <returns></returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse),StatusCodes.Status404NotFound)]
    [HttpDelete(RouteNames.Id)]
    public async Task RemoveUser([FromRoute] long userId, CancellationToken cancellationToken) =>
        await _mediator.Send(
            new RemoveUser.Request 
            { 
                UserId = userId, 
            }, cancellationToken);
}