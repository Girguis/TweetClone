using Core.Repository;
using FluentValidation;

namespace Application.Helpers;

internal static class UserValidator
{
    public static IRuleBuilderOptions<T, TProperty> UserNameNotExists<T, TProperty>
        (this IRuleBuilder<T, TProperty> ruleBuilder,
        IUserRepo userRepo)
    {
        return ruleBuilder.MustAsync(async (x, userName, ctx, cancel) =>
        {
            var userResult = await userRepo.GetByUserName(userName?.ToString());
            if (!userResult.IsSuccess || userResult.Data == null)
                return true;
            return false;
        });
    }
    public static IRuleBuilderOptions<T, TProperty> EmailNotExists<T, TProperty>
    (this IRuleBuilder<T, TProperty> ruleBuilder,
    IUserRepo userRepo)
    {
        return ruleBuilder.MustAsync(async (x, email, ctx, cancel) =>
        {
            var userResult = await userRepo.GetByEmail(email?.ToString());
            if (!userResult.IsSuccess || userResult.Data == null)
                return true;
            return false;
        });
    }

    public static IRuleBuilderOptions<T, TProperty> UserExists<T, TProperty>
    (this IRuleBuilder<T, TProperty> ruleBuilder,
    IUserRepo userRepo)
    {
        return ruleBuilder.MustAsync(async (x, guid, ctx, cancel) =>
        {
            var userResult = await userRepo.Get(guid?.ToString());
            if (!userResult.IsSuccess || userResult.Data == null)
                return false;
            return true;
        });
    }

}
