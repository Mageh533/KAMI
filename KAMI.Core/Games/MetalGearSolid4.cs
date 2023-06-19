using KAMI.Core.Cameras;
using KAMI.Core.Utilities;
using System;

namespace KAMI.Core.Games
{
    public class MetalGearSolid4 : Game<HAVACamera>
    {
        uint aimState;

        uint m_horAddress;
        uint m_vertAddress;

        public MetalGearSolid4(nint ipc) : base(ipc)
        {
        }

        public override void UpdateCamera(int diffX, int diffY)
        {
            aimState = IPCUtils.ReadU32(m_ipc, 0x641B3554); // Checks the aim state of the player

            // MGS4 has the camera in 6 seperate addresses

            if (aimState == 0) // Third person
            {
                m_horAddress = 0x641dea13;
                m_vertAddress = 0x641dea19;
            } 
            else if(aimState == 1) // Over the shoulder
            {
                m_horAddress = 0x6409f930;
                m_vertAddress = 0x6409f92c;

            }
            else // First person
            {
                m_horAddress = 0x6409f704;
                m_vertAddress = 0x6409f700;
            }

            m_camera.Hor = IPCUtils.ReadFloat(m_ipc, m_horAddress);
            m_camera.Vert = IPCUtils.ReadFloat(m_ipc, m_vertAddress);
            m_camera.Update(-diffX * SensModifier, diffY * SensModifier);

            IPCUtils.WriteFloat(m_ipc, m_horAddress, m_camera.Hor);
            IPCUtils.WriteFloat(m_ipc, m_vertAddress, m_camera.Vert);
        }
    }
}
