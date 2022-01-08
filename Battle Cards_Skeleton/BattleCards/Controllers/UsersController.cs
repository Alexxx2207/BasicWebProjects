using BattleCards.Data;
using BattleCards.Services;
using BattleCards.ViewModels;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleCards.Controllers
{
    public class UsersController : Controller
    {
        private IUserService userService;
        

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public HttpResponse Login()
        {

            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost("/Users/Login")]
        public HttpResponse DoLogin(string username, string password)
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            string userId = userService.TryGetUserId(username, password);

            if (userId == null)
            {
                return this.Error("User not found with this credentials!");
            }

            this.SignIn(userId);

            return this.Redirect("/");

        }

        public HttpResponse Register()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost("/Users/Register")]
        public HttpResponse DoRegister(UserInputViewModel userInputViewModel)
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrWhiteSpace(userInputViewModel.Username) || userInputViewModel.Username.Length < 5 || userInputViewModel.Username.Length > 20)
            {
                return this.Error("Username must be between 5 and 20 characters long!");
            }

            if (string.IsNullOrWhiteSpace(userInputViewModel.Email))
            {
                return this.Error("Email is Required!");
            }

            if (string.IsNullOrWhiteSpace(userInputViewModel.Password) || userInputViewModel.Password.Length < 6 || userInputViewModel.Password.Length > 20)
            {
                return this.Error("Password must be between 6 and 20 characters long!");
            }

            if (string.IsNullOrWhiteSpace(userInputViewModel.ConfirmPassword))
            { 
                return this.Error("Insert password confirmation!");
            }

            if (userInputViewModel.Password != userInputViewModel.ConfirmPassword)
            {
                return this.Error("Password and confirmed password are different!");
            }

            userService.AddUser(userInputViewModel.Username, userInputViewModel.Email, userInputViewModel.Password);
            return this.Redirect("/Users/Login");
        }

        [HttpGet("/Logout")]
        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }

    }
}
