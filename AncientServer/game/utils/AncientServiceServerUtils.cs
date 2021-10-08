using ancient.game.entity.player;
using ancientlib.AncientService;
using ancientlib.game.classes;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientserver.game.utils
{
    public class AncientServiceServerUtils
    {
        public static string path = @"\players\";
        public static string fileExtension = ".txt";

        public static void Initialize()
        {
            if (!Directory.Exists(Directory.GetCurrentDirectory() + path))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + path);
        }

        public static void CreateUserDirectory(string username)
        {
            Directory.CreateDirectory(GetPathForUser(username));
        }

        /*    public static void SaveCharacter(string username, EntityPlayer character)
            {
                if (!UserDirectoryExists(username))
                    CreateUserDirectory(username);

                FileStream stream = File.Create(GetPathForUser(username) + character.GetName() + fileExtension);
                BinaryWriter writer = new BinaryWriter(stream);
                character.WriteToDisk(writer);
                stream.Close();
            }*/

        private static bool UserDirectoryExists(string username)
        {
            return Directory.Exists(GetPathForUser(username));
        }

        /*        public static EntityPlayer LoadCharacter(string username, string path)
                {
                    EntityPlayer character = new EntityPlayer(null);

                    if (UserDirectoryExists(username))
                    {
                        StreamReader streamReader = File.OpenText(path);
                        BinaryReader reader = new BinaryReader(streamReader.BaseStream);
                        character.ReadFromDisk(reader);
                        streamReader.Close();

                        return character;
                    }

                    return null;
                }*/

        public static bool CharacterExists(string name)
        {
            return AncientServer.ancientServer.service.NicknameExists(name);
            //return Directory.GetFiles(Directory.GetCurrentDirectory() + path, name + fileExtension, SearchOption.AllDirectories).Length > 0;
        }

        public static bool UserHasCharacters(string username)
        {
            if (UserDirectoryExists(username))
            {
                int charactersAmount = GetCharactersPaths(username).Length;
                return charactersAmount > 0;
            }

            return false;
        }

        public static EntityPlayer[] GetUserCharacters(string username)
        {
            /*string[] characterPaths = GetCharactersPaths(username);

              EntityPlayer[] players = new EntityPlayer[characterPaths.Length];

              for (int i = 0; i < players.Length; i++)
                  players[i] = LoadCharacter(username, characterPaths[i]);

              return players;*/

            Player[] serviceChars = AncientServer.ancientServer.service.GetUserCharacters(username);
            EntityPlayer[] characters = new EntityPlayer[serviceChars.Length];

            Console.WriteLine("Characters on DB: " + serviceChars.Length);

            for (int i = 0; i < serviceChars.Length; i++)
            {
                EntityPlayer character = new EntityPlayer(null);
                characters[i] = GetEntityPlayerFromServicePlayer(serviceChars[i]);
            }

            Console.WriteLine("Charactes Array: " + characters.Length);

            return characters;
        }

        private static string[] GetCharactersPaths(string username)
        {
            return Directory.GetFiles(GetPathForUser(username), "*" + fileExtension);
        }

        private static string GetPathForUser(string username)
        {
            return @Directory.GetCurrentDirectory() + path + username + @"\";
        }

        public static Player GetServicePlayerFromEntityPlayer(EntityPlayer character)
        {
            Player serviceChar = new Player();

            serviceChar.Nickname = character.GetName();
            serviceChar.MaxHealth = character.GetMaxHealth();
            serviceChar.MaxMana = character.GetMaxMana();
            serviceChar.Level = character.GetLevel();
            serviceChar.Exp = character.GetExp();
            serviceChar.Str = character.GetStr();
            serviceChar.Wsd = character.GetWsd();
            serviceChar.Dex = character.GetDex();
            serviceChar.Luk = character.GetLuk();
            serviceChar.SkinColor = character.GetSkinColor().PackedValueInt();
            serviceChar.HairID = character.GetHairID();
            serviceChar.HairColor = character.GetHairColor().PackedValueInt();
            serviceChar.EyesID = character.GetEyesID();
            serviceChar.EyesColor = character.GetEyesColor().PackedValueInt();
            serviceChar.ClassName = character.GetClass().GetName();

            return serviceChar;
        }

        public static EntityPlayer GetEntityPlayerFromServicePlayer(Player character)
        {
            EntityPlayer player = new EntityPlayer(null);

            player.SetName(character.Nickname);
            player.SetMaxHealth(character.MaxHealth);
            player.SetMaxMana(character.MaxMana);
            player.SetLevel(character.Level);
            player.SetExp(character.Exp);
            player.SetStr(character.Str);
            player.SetWsd(character.Wsd);
            player.SetDex(character.Dex);
            player.SetLuk(character.Luk);
            player.SetSkinColor(character.SkinColor.Color());
            player.SetHairID((byte)character.HairID);
            player.SetHairColor(character.HairColor.Color());
            player.SetEyesID((byte)character.EyesID);
            player.SetEyesColor(character.EyesColor.Color());
            player.SetClass(Classes.GetClassFromName(character.ClassName));

            return player;
        }
    }
}
