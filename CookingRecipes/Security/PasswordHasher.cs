using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CookingRecipes.Security
{
  public  class PasswordHasher
    {


        //simple class to hash password
        public static (string hash, string salt) PasswordHash(string password)
        {
            //creating a random 16 bytes salt
            byte[] saltBytes = new byte[16];

            using (var rndm = RandomNumberGenerator.Create())
            {
                rndm.GetBytes(saltBytes);//filling the pass with random values!
            }

            string salt = Convert.ToBase64String(saltBytes);//converting data in a string in order to store it!

            //using PBKDF2 algorithm to securely hash passwords
            using (var pbkdf2 = new  Rfc2898DeriveBytes(password, saltBytes, 100000)) 
            {
                //taking a 32 bytes hash
                byte[] hashBytes = pbkdf2.GetBytes(32);

                //converting hash bytes in a string format!
                string hash = Convert.ToBase64String(hashBytes);
                return (hash, salt);

           }

        }


        //simple class to confirm whether passwords are the same or not!
        public static bool verifyPass(string enteredPass, string storedHash, string storedSalt) 
        {
            //converting from string to bytes
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            //rehashing the input password with the same salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPass, saltBytes, 100000))
            {
                //taking a 32 bytes hash
                byte[] hashBytes = pbkdf2.GetBytes(32);

                string hash = Convert.ToBase64String(hashBytes);//converting into a base64 string format

                //if stored pass is equal with the entered one return true
                return hash == storedHash;  

                  
            }


        }
        
        
        
        }

    }

