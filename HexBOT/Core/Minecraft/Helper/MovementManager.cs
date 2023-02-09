using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexBOT.Core.Minecraft.Helper
{
    public class MovementManager
    {
        public MovementManager(MinecraftClient Client)
        {
            Bot = Client;
        }

        private readonly MinecraftClient Bot;
    }
}
