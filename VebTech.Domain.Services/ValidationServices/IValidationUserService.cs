﻿namespace VebTech.Domain.Services.ValidationServices;

public interface IValidationUserService
{
    public Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken);
    
    public Task<bool> IsExists(long userId , CancellationToken cancellationToken);
}