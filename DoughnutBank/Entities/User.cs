﻿namespace DoughnutBank.Entities
{
    public class User
    {
        public string Email { get; set; }   
        public string Password {  get; set; }
        public OTP? OTP { get; set; }    
    }
}
