using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.input
{
    public interface IPlayerInput
    {
        void Update();

        void OnLeftButtonHeld();

        void OnLeftButtonPressed();

        void OnLeftButtonReleased();

        void OnRightButtonHeld();

        void OnRightButtonPressed();

        void OnRightButtonReleased();
    }
}
