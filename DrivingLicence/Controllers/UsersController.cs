using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrivingLicence.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DrivingLicence.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            // tài khoản admin: admin@gmail.com / Password@123
            if (ModelState.IsValid)
            {
                // tiến hành kiểm tra và đăng nhập
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // đăng nhập thành công sẽ chuyển đến trang thống kê
                    return RedirectToAction("index", "dashboard");
                }
            }
            // đăng nhập không thành công sẽ trở về trang chủ
            return RedirectToAction("index", "home", new { message = "Đăng nhập không thành công!" });
        }
        public async Task<IActionResult> Register(string message)
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!string.IsNullOrEmpty(message))
            {
                ViewBag.Message = message;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            string message = string.Empty;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = register.Email, Email = register.Email, PhoneNumber = register.PhoneNumber };
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    // tạo quyền admin
                    if (!await _roleManager.RoleExistsAsync("admin"))
                    {
                        var role = new IdentityRole
                        {
                            Name = "admin"
                        };
                        await _roleManager.CreateAsync(role);
                    }
                    // tạo quyền thành viên
                    if (!await _roleManager.RoleExistsAsync("member"))
                    {
                        var role = new IdentityRole
                        {
                            Name = "member"
                        };
                        await _roleManager.CreateAsync(role);
                    }
                    // Gán quyền cho tài khoản
                    await _userManager.AddToRoleAsync(user, "member"); // thay member = admin nếu muốn tạo 1 admin khác
                    // tạo token cho tài khoản
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // kích hoạt tài khoản
                    await _userManager.ConfirmEmailAsync(user, code);
                    // chuyển đến trang đăng nhập
                    return RedirectToAction("index", "home", new { message = "Đăng ký thành công!" });
                }
                else
                {
                    message = string.Join(",", result.Errors.Select(x => x.Description));
                    ViewBag.Message = "toastr[\"error\"]('" + message.Replace("'", "") + "')";
                    return View();
                }
            }
            ViewBag.Message = "toastr[\"error\"]('Có lỗi xảy ra, xin vui lòng thử lại')";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("Không tìm thấy thí sinh chỉ định");
            }
            var user = await _userManager.FindByIdAsync(id);
            var res = await _userManager.DeleteAsync(user);
            if (res.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound("Xóa thất bại");
        }
    }
}