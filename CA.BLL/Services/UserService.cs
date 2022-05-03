using ABM.DAL.Repository;
using CA.DAL.Entity;
using CA.DTO.Models;
using CA.Infrastucture.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Login(LoginModel model)
        {
            var user = await _repository.Filter<User>(u => u.Email == model.Email).FirstOrDefaultAsync()
                ?? throw new ApplicationException("Email is not registred");

            if (!Utilities.VerifyHashedPassword(user.PasswordHashed, model.Password))
            {
                throw new ApplicationException("Password is incorrect");
            }

            return Utilities.Token(user.Id);
        }

        public async Task<User> CheckPersonById(int id)
        {
            return await _repository.Filter<User>(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<string> Register(RegisterModel model)
        {
            var data = await _repository.Filter<TemporaryUser>(x => x.Id == model.Id && x.EmailVerified)
                .FirstOrDefaultAsync();
            
            var existEmail = await _repository.Filter<User>(x => x.Email == data.Email).FirstOrDefaultAsync();
            if (existEmail != null)
                throw new ApplicationException("Email is already in use");

            var user = await _repository.AddAsync(new User
            {
                Email = data.Email,
                PasswordHashed = Utilities.HashPassword(model.Password),
            });
            await _repository.SaveChanges();

            await _repository.DeleteAsync<TemporaryUser>(data.Id);
            await _repository.SaveChanges();
            return Utilities.Token(user.Id);
        }

        public async Task<int> RegisterEmail(RegisterEmailModel model)
        {
            model.Email = model.Email.ToLower();
            var existUser = await _repository.Filter<User>(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (existUser != null)
                throw new ApplicationException("Email is already in use");

            var existTempUser = await _repository.Filter<TemporaryUser>(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (existTempUser != null)
               await _repository.HardRemove<User>(existTempUser.Id);

            var code = Utilities.KeyGenerator(6);
            var newUser = await _repository.CreateAsync(new TemporaryUser
            {
                Email = model.Email,
                EmailCode = Utilities.HashPassword(code),
            });

            await _repository.SaveChanges();
            await new MailHelper().SendEmail(model.Email, "Verification Code", $"Your verification code is {code}");
            return newUser.Id;
        }

        public async Task<bool> VerifyEmail(VerifyModel model)
        {
            var user = await _repository.Filter<TemporaryUser>(x => x.Id == model.Id).FirstOrDefaultAsync()
                ?? throw new ApplicationException("User not found");

            var isVerified = Utilities.VerifyHashedPassword(user.EmailCode, model.Code);
            if (!isVerified)
                throw new ApplicationException("Invalid Code");


            user.EmailVerified = true;
            await _repository.SaveChanges();
            return true;
        }
    }
}
