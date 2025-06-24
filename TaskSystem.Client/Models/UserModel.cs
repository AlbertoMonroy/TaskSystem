﻿namespace TaskSystem.Client.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsOnline { get; set; }
    }
}
