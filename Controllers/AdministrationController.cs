using EmployeeeManagement.Models;
using EmployeeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeeManagement.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdministrationController:Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager )
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult>EditUser(string id)
        {
            var user = await userManager.FindByNameAsync(id);
            if(user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                City = user.City,
                Claims= userClaims.Select(c=> c.Value).ToList(),
                Roles=userRoles
            };

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel _model)
        {
            var user = await userManager.FindByNameAsync(_model.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {_model.Id} cannot be found";
                return View("NotFound");
            }
            else 
            {
                user.Email = _model.Email;
                user.UserName = _model.UserName;
                user.City = _model.City;

                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(_model);
            }

        }



        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={id} cannot be found";
                return View("notFound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel _model)
        {
            var role = await roleManager.FindByIdAsync(_model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={_model.Id} cannot be found";
                return View("notFound");
            }
            else
            {
                role.Name = _model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if(result.Succeeded) 
                {
                    return RedirectToAction("listRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(_model);
            }
         
            
        }


        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(CreateRoleViewModel model)
        {
            if (ModelState.IsValid) 
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await roleManager.CreateAsync(identityRole);
                
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleID) 
        {
            ViewBag.roleID = roleID;
            
            var role =await roleManager.FindByIdAsync(roleID);

            if (roleID == null)
            {
                ViewBag.ErrorMessage = $"Role with id {roleID} cannot be found";
                return View("NotFound");
            }
            var _modeL = new List<UserRoleViewModel>();
            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else 
                {
                    userRoleViewModel.IsSelected = false;
                }
                _modeL.Add(userRoleViewModel);
            }
            return View(_modeL);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);
                
                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole",new { Id = roleId});
        }
        [HttpGet]
        [AllowAnonymous]

        public IActionResult AccessDenied()
        { 
            return View("AccessDenied");//video 83 pragim
            //return View("NotFound");
        }



    }
}
