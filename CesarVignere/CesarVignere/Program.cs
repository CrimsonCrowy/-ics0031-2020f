using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace CesarVignere
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("ics0031-2020f Tino.Apostolovski HW01");
            while (true)
            {
                PrintMenu();
                var userInput = GetInput();
                if (CheckExit(userInput)) return;
                switch (userInput)
                {
                    case "1":
                        CesarE();
                        break;
                    case "2":
                        VigenereE();
                        break;
                    case "3":
                        CesarD();
                        break;
                    case "4":
                        VigenereD();
                        break;
                    default:
                        Console.WriteLine("Seems like I do not posses this option, try again.");
                        break;

                }
            }
        }
        private static void PrintMenu()
        {
            Console.WriteLine("Welcome to the en/decryption menu");
            Console.WriteLine("What method are we doing today:");
            Console.WriteLine("1: Cesar Encrypt");
            Console.WriteLine("2: Vigenere Encrypt");
            Console.WriteLine("3: Cesar Decrypt");
            Console.WriteLine("4: Vigenere Decrypt");
            Console.WriteLine("X: Or would you like to exit");
        }
        
        private static bool CheckExit(string input)
        {
            if(input.ToLower() == "x") return true;
            return false;

        }
        private static string GetInput()
        {
            var userinput = "";
            userinput = Console.ReadLine().Trim();
            return userinput;
        }

        private static void CesarE()
        {
            var input = "";
            Console.WriteLine("Please input your key value or 'x' to cancel:");
            Initiate:
            input = GetInput();
            if (CheckExit(input)) return;
            if (int.TryParse(input, out var key))
            {
                key = key % 255;
                if (key == 0)
                {
                    Console.WriteLine("Multiples of 255 is not a cipher, it would not do anything");
                    return;
                }
                else Console.WriteLine($"Your key is {key}");
            }
            else
            {
                Console.WriteLine("Please input a valid integer");
                goto Initiate;
            }
            Console.WriteLine("Please input your plaintext or 'X' to cancel:");
            var plaintext = GetInput();
            if (CheckExit(plaintext)) return;
            if (plaintext != null) Console.WriteLine($"The lenght of your plaintext is {plaintext.Length}");
            ShowEncoding(plaintext, Encoding.Default);
            var encryptedBytes = CesarEncrypt(plaintext, (byte) key, Encoding.Default);
            Console.Write("Encrypted bytes: ");
            foreach (var encryptedByte in encryptedBytes)
            {
                Console.Write(encryptedByte + " ");
            }
            Console.WriteLine();
            Console.WriteLine("base64: " + Convert.ToBase64String(encryptedBytes));
        }

        private static byte[] CesarEncrypt(string plaintext, byte key, Encoding encoding)
        {
            var inputBytes = encoding.GetBytes(plaintext);
            var result = new byte[inputBytes.Length];
            if (key == 0)
            {
                for (var i = 0; i < inputBytes.Length; i++)
                {
                    result[i] = inputBytes[i];
                }
            }
            else
            {
                for (var i = 0; i < inputBytes.Length; i++)
                {
                    var newCharValue = (inputBytes[i] + key);
                    if (newCharValue > byte.MaxValue)
                    {
                        newCharValue = newCharValue - byte.MaxValue;
                    }

                    result[i] = (byte)newCharValue;
                }
            }
            return result;
        }

        private static void CesarD()
        {
            var userInput = "";
            byte[] cipherText = null;
            Console.WriteLine("Please input your base64 encoded ciphertext or 'X' to cancel:");
            CText:
            userInput = GetInput();
            if(CheckExit(userInput)) return;
            try
            {
                cipherText = Convert.FromBase64String(userInput);
            }
            catch
            {
                Console.WriteLine("Uh Oh, Seems like that is not a valid base64, try again:");
                goto CText;
            }
            Console.WriteLine("Encrypted bytes:");
            foreach (var b in cipherText)
            {
                Console.Write(b + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Please input the key value or 'X' to cancel:");
            KeyValue:
            var key = GetInput();
            if (CheckExit(key)) return;
            if (int.TryParse(key, out var keyI))
            {
                keyI = keyI % 255;
                if (keyI == 0)
                {
                    Console.WriteLine("multiples of 255 is no cipher, this would not do anything!");
                    goto KeyValue;
                }
                else
                {
                    Console.WriteLine($"Cesar key is: {key}");
                }
            }
            else
            {
                Console.WriteLine("Please input a valid integer value:");
                goto KeyValue;
            }

            var plainText = CesarDecode(cipherText, keyI, Encoding.ASCII);
            Console.WriteLine($"Your plaintext is: {plainText}");
        }

        private static string CesarDecode(byte[] cipherText, int key, Encoding encoding)
        {
            var result = new byte[cipherText.Length];
            if (key == 0)
            {
                for (var i = 0; i < cipherText.Length; i++)
                {
                    result[i] = cipherText[i];
                }
            }
            else
            {
                for (int i = 0; i < cipherText.Length; i++)
                {
                    var newCharValue = (cipherText[i] - key);
                    if (newCharValue > byte.MaxValue)
                    {
                        newCharValue = newCharValue - byte.MaxValue;
                    }

                    result[i] = (byte)newCharValue; // drop the first 3 bytes of int, just use the last one
                }
            }

            return encoding.GetString(result);
        }

        private static void VigenereE()
        {
            string input = null;
            Initial:
            do
            {
                Console.WriteLine("Please input your keyword value or 'x' to cancel:");
                input = GetInput();
                if (CheckExit(input)) return;
            } while (input == null);

            var key = "";
            if (Regex.IsMatch(input, @"^[a-zA-Z]+$"))
            {
                key = input;
            }
            else
            {
                Console.WriteLine("Please input only a letter based string");
                goto Initial;
            }
            Console.WriteLine($"Your keyword is: {key}");
            ShowEncoding(key, Encoding.Default);
            
            Console.WriteLine("Please input your plaintext or 'X' to cancel:");
            var plaintext = GetInput();
            if (CheckExit(plaintext)) return;
            if (plaintext != null) Console.WriteLine($"The lenght of your plaintext is {plaintext.Length}");
            ShowEncoding(plaintext, Encoding.Default);
            var encryptedBytes = VigenereEncrypt(plaintext, key, Encoding.Default);
            Console.Write("Encrypted bytes: ");
            foreach (var encryptedByte in encryptedBytes)
            {
                Console.Write(encryptedByte + " ");
            }
            Console.WriteLine();
            Console.WriteLine("base64: " + Convert.ToBase64String(encryptedBytes));
        }
        private static byte[] VigenereEncrypt(string plaintext, string key, Encoding encoding)
        {
            var plaintextBytes = encoding.GetBytes(plaintext);
            var keyBytes = encoding.GetBytes(key);
            var result = new byte[plaintext.Length];
            for (var i = 0; i < plaintextBytes.Length; i++)
            {
                var newCharValue = (plaintextBytes[i] + keyBytes[i % keyBytes.Length]);
                if (newCharValue > byte.MaxValue)
                {
                    newCharValue = newCharValue - byte.MaxValue;
                }

                result[i] = (byte) newCharValue;
            }

            return result;
        }

        private static void VigenereD()
        {
            var userInput = "";
            byte[] cipherText = null;
            Console.WriteLine("Please input your base64 encoded ciphertext or 'X' to cancel:");
            CText:
            userInput = GetInput();
            if(CheckExit(userInput)) return;
            try
            {
                cipherText = Convert.FromBase64String(userInput);
            }
            catch
            {
                Console.WriteLine("Uh Oh, Seems like that is not a valid base64, try again:");
                goto CText;
            }
            Console.WriteLine("Encrypted bytes:");
            foreach (var b in cipherText)
            {
                Console.Write(b + " ");
            }
            Console.WriteLine();
            Console.WriteLine("Please input the key value or 'X' to cancel:");
            KeyValue:
            userInput = GetInput();
            if (CheckExit(userInput)) return;
            var key = "";
            if (Regex.IsMatch(userInput, @"^[a-zA-Z]+$"))
            {
                key = userInput;
            }
            else
            {
                Console.WriteLine("Seems like that was not a proper keyword, try again.");
                goto KeyValue;
            }
            

            var plainText = VignereDecode(cipherText, key, Encoding.ASCII);
            Console.WriteLine($"Your plaintext is: {plainText}");
        }

        private static string VignereDecode(byte[] cipherText, string key, Encoding encoding)
        {
            var result = new byte[cipherText.Length];
            var keyBytes = Encoding.Default.GetBytes(key);
            for (int i = 0; i < cipherText.Length; i++)
            {
                var newCharValue = (cipherText[i] - keyBytes[i % keyBytes.Length]);
                if (newCharValue < byte.MinValue)
                {
                    newCharValue = newCharValue + byte.MinValue;
                }

                result[i] = (byte) newCharValue;
            }

            return encoding.GetString(result);
        }
        
        private static void ShowEncoding(string text, Encoding encoding)
        {
            Console.WriteLine(encoding.EncodingName);
            
            Console.Write("Preamble ");
            foreach (var preambleByte in encoding.Preamble)
            {
                Console.Write(preambleByte + " ");
            }
            Console.WriteLine();
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write($"{text[i]} "); 
                foreach (var byteValue in encoding.GetBytes(text.Substring(i,1)))
                {
                    Console.Write(byteValue + " ");
                }
            }
            Console.WriteLine();
        }
    }
}