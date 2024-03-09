using MedicalRecordsData.Entities.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MedicalRecordsData.Entities.AuthEntity
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public int StaffId { get; set; }
        public bool IsSettings { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public string ActionTaken { get; set; }

        public User(
        string email,
        string username,
        int roleId,
        int staffId,
        string token,
        string refreshToken,
        byte[] passwordHash,
        byte[] passwordSalt,
        int modifiedBy,
        string actionTaken)
        {
            Email = email;
            Username = username;
            RoleId = roleId;
            StaffId = staffId;
            IsSettings = true;
            Token = token;
            RefreshToken = refreshToken;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Status = 1;
            CreatedBy = Id;
            ModifiedBy = modifiedBy;
            ActionTaken = actionTaken;
        }

        public User(UserCreateDTO user)
        {
            Email = user.Email;
            Username = user.Username;
            RoleId = user.RoleId;
            StaffId = user.StaffId;
            IsSettings = true;
            CreatedAt = DateTime.Now;
            Status = 1;
            CreatedBy = Id;
            ActionTaken = "Record Creation";
        }

        public User(UserUpdateDTO user)
        {
            Email = user.Email;
            Username = user.Username;
            RoleId = user.RoleId;
            StaffId = user.StaffId;
            IsSettings = true;
            Status = 1;
            ModifiedBy = Id;
            ActionTaken = "Record Updated";
        }

        public User()
        {
        }
    }

    public class UserCreateDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Username should be between 6 and 100 characters.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password should be between 8 and 100 characters.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "StaffId is required.")]
        public int StaffId { get; set; }
        [Required(ErrorMessage = "Picture is required.")]
        public string Picture { get; set; }
    }

    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Username should be between 6 and 100 characters.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Password should be between 8 and 100 characters.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "StaffId is required.")]
        public int StaffId { get; set; }
        [Required(ErrorMessage = "Picture is required.")]
        public string Picture { get; set; }
    }
}
