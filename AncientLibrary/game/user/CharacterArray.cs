using ancient.game.entity.player;
using ancientlib.game.constants;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.user
{
    public class CharactersArray
    {
        private int maxCharacters;
        private List<EntityPlayer> characters;

        public int Count
        {
            get { return characters.Count; }
        }

        public int MaxCharacters
        {
            get { return this.maxCharacters; }
        }

        public CharactersArray(int maxCharacters)
        {
            this.maxCharacters = maxCharacters;
            this.characters = new List<EntityPlayer>();
        }

        public CharactersArray() : this(GameConstants.MAX_CHARACTERS)
        { }

        public EntityPlayer GetCharacter(int index)
        {
            if (index >= Count)
                return null;

            return characters[index];
        }

        public void AddCharacter(EntityPlayer character)
        {
            if (characters.Count < maxCharacters)
                characters.Add(character);
        }

        public void SetCharacters(EntityPlayer[] characters)
        {
            this.characters = characters.ToList();
        }

        public void Read(BinaryReader reader)
        {
            this.maxCharacters = reader.ReadByte();
            int count = reader.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                EntityPlayer character = new EntityPlayer(null);
                character.Read(reader);
                AddCharacter(character);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((byte)maxCharacters);
            writer.Write(Count);

            for (int i = 0; i < Count; i++)
                characters.ElementAt(i).Write(writer);
        }
    }
}
