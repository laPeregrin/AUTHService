using AUTHENTICATIONService.Models.Wraps;
using DataBaseStaff.Models;
using System;


namespace AUTHENTICATIONService.Services.Extensions
{
    public static class RequestExtension
    {
        public static User ConvertToUser(this RegisterRequest request, string HashPassword)
        {
            UserRole role = UserRole.Reader;
            if (Convert.ToInt32(request.IsAuthor) == 0)
                role = UserRole.Poster;
            else
                role = UserRole.Reader;
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                Role = role,
                UserName = request.UserName,
                PasswordHash = HashPassword
            };
            return user;
        }
    }
}
