using ancientlib.game.constants;
using ancientlib.game.network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.user
{
    public class User
    {
        private string username;
        private string password;

        private CharactersArray charactersArray;

        public User()
        {
            this.charactersArray = new CharactersArray(GameConstants.MAX_CHARACTERS);
        }

        public User(string username, string password) : this()
        {
            this.username = username;
            this.password = password;
        }

        public string GetUsername()
        {
            return this.username;
        }

        public void SetUsername(string username)
        {
            this.username = username;
        }

        public string GetPassword()
        {
            return this.password;
        }

        public void SetPassword(string password)
        {
            this.password = password;
        }

        public CharactersArray GetCharactersArray()
        {
            return this.charactersArray;
        }

        public void SetCharactersArray(CharactersArray charactersArray)
        {
            this.charactersArray = charactersArray;
        }

        public override string ToString()
        {
            return username;
        }
    }
}
