using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSIMS.DBContext.Entities;
using WebSIMS.Interfaces;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace WebSIMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: /User/
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            return View(users);
        }

        // GET: /User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,PasswordHash,Role")] Users user)
        {
            if (ModelState.IsValid)
            {
                if (await _userRepository.GetUserByUsername(user.Username) != null)
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(user);
                }

                try
                {
                    await _userRepository.AddAsync(user);
                    await _userRepository.SaveChangeAsync();
                    TempData["SuccessMessage"] = "Account created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error creating account: {ex.Message}";
                    return View(user);
                }
            }
            return View(user);
        }

        // GET: /User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = await _userRepository.GetUserById(id.Value);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,Username,PasswordHash,Role")] Users user)
        {
            if (id != user.UserID) return NotFound();

            if (ModelState.IsValid)
            {
                var existingUser = await _userRepository.GetUserByUsername(user.Username);
                if (existingUser != null && existingUser.UserID != user.UserID)
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                    return View(user);
                }

                try
                {
                    _userRepository.Update(user);
                    await _userRepository.SaveChangeAsync();
                    TempData["SuccessMessage"] = "Account updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error updating account: {ex.Message}";
                    return View(user);
                }
            }
            return View(user);
        }

        // GET: /User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var user = await _userRepository.GetUserById(id.Value);
            if (user == null) return NotFound();
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var user = await _userRepository.GetUserById(id);
                if (user != null)
                {
                    _userRepository.Delete(user);
                    await _userRepository.SaveChangeAsync();
                    TempData["SuccessMessage"] = "Account deleted successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting account: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}