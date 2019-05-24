using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Domain.GameCharacter.Commands
{
    public class RemoveCharacterCommand : CharacterCommand
    {
        public RemoveCharacterCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
